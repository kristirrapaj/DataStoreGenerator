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

    private string[] tables;
    public void SetPaths(string? configPath, string finalPath)
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
    
    public void SetTables(string[] tables)
    {
        this.tables = tables;
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

    public void DbSelector(int selectedIndex)
    {
        if (selectedIndex == 0) return;
        
        var dataBaseType = DatabaseList[selectedIndex].Item1;
        var connectionString = DatabaseList[selectedIndex].Item2;
        var schema = DatabaseList[selectedIndex].Item3;
        
        switch (dataBaseType)
        {
            case "SQL_SERVER":
                //todo: chiedere perchè chiediamo dataProviderType
                IDataStore dataStore =
                    new SQLServerGeomDataStore(connectionString, schema);
                //??
                dataStore.DataProviderType = "SQL_SERVER";
                
                var generator = new SqlGenerator(dataStore);
                generator.Generate(FinalPath, tables);
                break;
            case "ORACLE":
                //todo: implementare
                break;
            default:
                throw new Exception("DataStoreType not supported");
        }
    }
    
    //datastore createdatastore {datastoretype} {connectionstring} {schema}
    
    
    
}