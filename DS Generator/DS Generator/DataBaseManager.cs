using System.ComponentModel.Composition.Registration;
using System.Data;
using System.Windows.Documents;
using DataStore.Factory;
using DataStore.Interface;

namespace DS_Generator;

public class DataBaseManager
{
    private string mCurrentDataStoreType;
    private string mCurrentId;
    private DataSet mConfigDataSet;
    private string mConfigFilePath;
    private string mOutputConfigFilePath;
    private IDataStore? mDataStore;

    public List<string>? AvailableDataProvider { get; private set; }

    public List<string>? AvailableDatabases { get; private set; }

    public List<string>? AvailableTables { get; private set; }

    public string ConfigFilePath
    {
        set => mConfigFilePath = value;
    }

    public string OutputConfigFilePath
    {
        set => mOutputConfigFilePath = value;
    }

    public string DataStoreType
    {
        set
        {
            mCurrentDataStoreType = value;
            SetAvailableDatabase();
        }
    }

    public string Database
    {
        set
        {
            mCurrentId = value;
            SetAvailableTables();
        }
    }

    public string[] Tables
    {
        set
        {
            // Get the selected tables from selected database
            try
            {
                if (mDataStore == null) throw new Exception("DataStore is null");
                var sqlGen = new SqlGenerator(mDataStore);
                sqlGen.Generate(mOutputConfigFilePath, value);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }

    /// <summary>
    ///  Read the config file and set the dataset.
    ///  Set the available database types in DatabaseManager.AvailableDataProvider.
    /// </summary>
    public DataBaseManager()
    {
        mCurrentDataStoreType = "";
        mCurrentId = "";
        mConfigDataSet = new DataSet();
        mConfigFilePath = "";
        mOutputConfigFilePath = "";
        IDataStore mDataStore = null!;
        try
        {
            var dataset = new DataSet();
            dataset.ReadXml(mConfigFilePath);
            mConfigDataSet = dataset;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        // Get the available data store types from the config file
        AvailableDataProvider = (
            from DataRow dataProvider in mConfigDataSet.Tables[0].Rows
            select TagPickerXml(dataProvider, "DATA_STORE_TYPE")
        ).ToList();
    }

    /// <summary>
    ///  Set the available tables from the chosen database in DataBaseManager.AvailableTables.
    /// </summary>
    private void SetAvailableTables()
    {
        var cnnStr = (
            from DataRow dataProvider in mConfigDataSet.Tables[0].Rows
            where TagPickerXml(dataProvider, "ID") == mCurrentId
            select TagPickerXml(dataProvider, "CONN_STR")
        ).ToList();

        var schema = (
            from DataRow dataProvider in mConfigDataSet.Tables[0].Rows
            where TagPickerXml(dataProvider, "ID") == mCurrentId
            select TagPickerXml(dataProvider, "SCHEMA")
        ).ToList();

        // Pass the connection string and schema to the data store factory to get the available tables and views from the DataStore
        mDataStore = DataStoreFactory.GetDataStore(mCurrentDataStoreType, connStr: cnnStr[0], schema: schema[0]);
        mDataStore.DataProviderType = mCurrentDataStoreType;
        AvailableTables = mDataStore.GetExistingTables(owner: schema[0]).ToList();
        foreach (var view in mDataStore.GetExistingViews())
        {
            AvailableDatabases.Add(view);
        }

        AvailableTables.Sort();
    }

    /// <summary>
    ///  Set the available database types in DataBaseManager.AvailableDatabases.
    /// </summary>
    /// <exception cref="Exception">Count Mismatch for ID and SCHEMA in configuration file.</exception>
    private void SetAvailableDatabase()
    {
        // Get the ID and SCHEMA from the config file
        var schemeId = (
            // Get the ID from the config file
            from DataRow dataProvider in mConfigDataSet.Tables[0].Rows
            where TagPickerXml(dataProvider, "DATA_STORE_TYPE") == mCurrentDataStoreType
            select TagPickerXml(dataProvider, "ID"),
            // Get the SCHEMA from the config file
            from DataRow dataProvider in mConfigDataSet.Tables[0].Rows
            where TagPickerXml(dataProvider, "DATA_STORE_TYPE") == mCurrentDataStoreType
            select TagPickerXml(dataProvider, "SCHEMA")
        );

        // Check if the count of ID and SCHEMA are equal, if not, the config file is malformed -> throw exception
        if (schemeId.Item1.Count() != schemeId.Item2.Count()) throw new Exception("ID and SCHEMA count mismatch");

        // Zip the ID and SCHEMA together and add them to the available database list in the format "ID: SCHEMA"
        var availableDb = (
            from valueTuple in schemeId.Item1.Zip(schemeId.Item2, (id, schema) => (id, schema))
            let id = valueTuple.Item1
            let schema = valueTuple.Item2
            select id + ": " + schema
        ).ToList();

        AvailableDatabases = availableDb;
    }

    /// <summary>
    ///  Static method to pick a single value of a given tag from a given DataRow.
    /// </summary>
    /// <param name="row">The DataRow to search the tag from.</param>
    /// <param name="tag">The Tag to search for.</param>
    /// <returns></returns>
    /// <exception cref="Exception">Tag not found</exception>
    private static string TagPickerXml(DataRow row, string tag)
    {
        try
        {
            var value = row[tag].ToString();

            if (string.IsNullOrEmpty(value)) throw new Exception("Tag not found");

            return value;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}