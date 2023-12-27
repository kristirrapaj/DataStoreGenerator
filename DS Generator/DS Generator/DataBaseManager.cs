using System.Data;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using DataStore.Interface;
using DataStore.SQLServerDataStore;

namespace DS_Generator;

public class DatabaseManager
{
    private static string FolderPath;
    private static string FinalPath;
    
    public Dictionary<int, Tuple<string, string, string>> DatabaseList = new Dictionary<int, Tuple<string, string, string>>();
    public void SetPaths(string configPath, string finalPath)
    {
        FolderPath = configPath;
        FinalPath = finalPath;
        
        string[] files = Directory.GetFiles(FolderPath, "*.xml");
        int index = 1;
        
        
        foreach (string file in files)
        {
            var dataStoreType = GetDataStoreProperties(file, index,"DATA_STORE_TYPE");
            var connectionString = GetDataStoreProperties(file, index,"CONN_STR");
            var schema = GetDataStoreProperties(file, index,"SCHEMA");
            
            var tuple = new Tuple<string, string, string>(dataStoreType, connectionString, schema);
            DatabaseList.Add(index, tuple);
            
            index++;
        }
    }

    private string GetDataStoreProperties(string file, int index, string tag)
    {
        var dataSet = new DataSet();
        dataSet.ReadXml(file);

        try
        {
            var value = dataSet.Tables[0].Rows[0][tag].ToString();

            if (string.IsNullOrEmpty(value))
            {
                throw new Exception("DataStoreType is null or empty");
            }

            return value;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    /*private void GetDataStoreType(string file, int index)
    {
        DataSet dataSet = new DataSet();
        dataSet.ReadXml(file);

        var dataStoreTypeTag = "DATA_STORE_TYPE";
        var dataStoreType = "";

        try
        {
            dataStoreType = dataSet.Tables[0].Rows[0][dataStoreTypeTag].ToString();

            if (string.IsNullOrEmpty(dataStoreType))
            {
                throw new Exception("DataStoreType is null or empty");
            }

            DataBaseNames.Add(index, dataStoreType);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }*/

    public void DbSelector(int selectedIndex)
    {
        
        if (selectedIndex == 0) return;
        
        //todo: scegliere all user
        string[] tables = new string[] { "EXT_MAP_NOTE_POINT", "EXT_MAP_NOTE_LINE", "EXT_MAP_NOTE_POLY" };
        
        switch (DataBaseNames[selectedIndex])
        {
            case "SQL_SERVER":
                IDataStore dataStore = new DataStore.SQLServerDataStore.SQLServerGeomDataStore(connStr: SchemaType:)
                dataStore.DataProviderType = "SQL_SERVER";
                var generator = new SqlGenerator(dataStore);
                generator.Generate(FinalPath, tables);
                break;
        }
    }
    

    /*private IDataStore CreateDataStore(string dataProviderType, string connectionString, string schema)
    {
        switch (dataProviderType)
        {
            case "SQL_SERVER":
                return new DataStore.SQLServerDataStore.SQLServerGeomDataStore(connStr: connectionString, schema: schema);
            default:
                throw new ArgumentException($"Unsupported data provider type: {dataProviderType}");
        }
    }

    
    private IDataStore CreateDataStore(string dataProviderType, string connectionString, string schema)
    {
        switch (dataProviderType)
        {
            case "SQL_SERVER":
                return new DataStore.SQLServerDataStore.SQLServerGeomDataStore(connStr: connectionString, schema: schema);
            // Add more cases for other data provider types as needed
            default:
                throw new ArgumentException($"Unsupported data provider type: {dataProviderType}");
        }
    }
    */
}