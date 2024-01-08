using System.Data;
using DataStore.Factory;
using DataStore.Interface;
using DS_Generator.Generators;

namespace DS_Generator.Database;

/// <summary>
/// TODO: Creare controlli per la selezione, cosi da non avere la stessa scelta muultiple volte
/// TODO: Transformare tutti gli errori in eccezioni e finestrelle di errore
/// TODO: Creare un file di configurazione per il programma
/// TODO: Rimuovere datastore type (?)
/// </summary>

public class DataBaseManager
{
    private string mCurrentDataStoreType;
    private string mCurrentId;
    private DataSet mConfigDataSet;
    private string mOutputConfigFilePath;
    private IDataStore? mDataStore;
    private string mConfigFilePath;
    public List<string>? AvailableDatastores { get; private set; }

    public List<string>? AvailableTables { get; private set; }
    
    public string ConfigFilePath { set => mConfigFilePath = value;}

    public string OutputConfigFilePath
    {
        set => mOutputConfigFilePath = value;
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

    public string Database {
        set {
            mCurrentId = value.Split(" - ")[0].Split(": ")[1];
            Console.WriteLine(mCurrentId);
            SetAvailableTables();
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
        mDataStore = null;
    }
    public void FetchDatabasesFromConfigurationFile() {
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

        AvailableDatastores = new List<string>();
        
        foreach (DataRow row in mConfigDataSet.Tables[0].Rows) {
            AvailableDatastores.Add($"{row["DATA_STORE_TYPE"]}: {row["ID"]} - {row["SCHEMA"]}");
        }
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

        mCurrentDataStoreType = (
            from DataRow dataProvider in mConfigDataSet.Tables[0].Rows
            where TagPickerXml(dataProvider, "ID") == mCurrentId
            select TagPickerXml(dataProvider, "DATA_STORE_TYPE")
        ).ToList()[0];
        
        Console.WriteLine(mCurrentDataStoreType);
        
        // Pass the connection string and schema to the data store factory to get the available tables and views from the DataStore
        mDataStore = DataStoreFactory.GetDataStore(mCurrentDataStoreType, connStr: cnnStr[0], schema: schema[0]);
        mDataStore.DataProviderType = mCurrentDataStoreType;
        AvailableTables = mDataStore.GetExistingTables(owner: schema[0]).ToList();
        foreach (var view in mDataStore.GetExistingViews())
        {
            AvailableTables.Add(view);
        }

        AvailableTables.Sort();
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