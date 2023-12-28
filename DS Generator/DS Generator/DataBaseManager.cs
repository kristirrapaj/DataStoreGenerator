using System.Data;
using System.IO;
using DataStore.Interface;
using DataStore.SQLServerDataStore;

namespace DS_Generator;

public class DatabaseManager
{
    private static string FolderPath;
    private static string FinalPath;
    
    //public Dictionary<int, Tuple<string, string, string>> DatabaseList = new Dictionary<int, Tuple<string, string, string>>();
    
    public Dictionary<int, Dictionary<string, DataSet>> DatabaseList = new Dictionary<int, Dictionary<string, DataSet>>();

    private string[] tables;

    private int selectedIndex;
    public void SetPaths(string configPath, string finalPath)
    {
        FolderPath = configPath;
        FinalPath = finalPath;
        
        string[] files = Directory.GetFiles(FolderPath, "*.xml");
        int index = 1;
        
        
        foreach (string file in files)
        {
            var dataStoreType = GetDataStoreProperties(file, "DATA_STORE_TYPE");
            
            var dictionary = new Dictionary<string, DataSet>();
            dictionary.Add(dataStoreType, new DataSet());
            
            DatabaseList.Add(index, dictionary);
            GetDatabaseProperties(index, dataStoreType, file);
            index++;
        }
    }
    
    private void GetDatabaseProperties(int index, string dataStoreType, string file)
    {
        var dataSet = new DataSet();
        dataSet.ReadXml(file);

        foreach (var key in DatabaseList.Keys)
        {
            if (key == index)
            {
                foreach (var value in DatabaseList[key].Keys)
                {
                    if (value == dataStoreType)
                    {
                        DatabaseList[key][value] = dataSet;
                    }
                }
            }
        }
    }
    
    public void SetTables(string[] tables)
    {
        this.tables = tables;
    }

    private string GetDataStoreProperties(string file, string tag)
    {
        var dataSet = new DataSet();
        dataSet.ReadXml(file);

        try
        {
            var value = dataSet.Tables[0].Rows[0][tag].ToString();

            if (string.IsNullOrEmpty(value))
            {
                throw new Exception("Type is null or empty");
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
        this.selectedIndex = selectedIndex;
        if (selectedIndex == 0) return;
        
        
        
        IDataStore dataStore = CreateDataStore(connectionString, schema);
        var generator = new SqlGenerator(dataStore);
        generator.Generate(FinalPath, tables);
    }

    private static IDataStore CreateDataStore(string connectionString, string schema)
    {
        return new SQLServerGeomDataStore(connectionString, schema);
    }
}