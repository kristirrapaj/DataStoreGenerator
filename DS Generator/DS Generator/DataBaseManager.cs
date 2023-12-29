using System.Data;
using System.IO;
using DataStore.Interface;
using DataStore.SQLServerDataStore;

namespace DS_Generator;

public class DatabaseManager
{
    private static string _folderPath;
    private static string _finalPath;

    //public Dictionary<int, Tuple<string, string, string>> DatabaseList = new Dictionary<int, Tuple<string, string, string>>();

    public Dictionary<int, Dictionary<string, DataSet>> DatabaseList =
        new Dictionary<int, Dictionary<string, DataSet>>();

    private string[] _tables;

    private int _selectedIndex;

    private void GetDatabaseProperties(int index, string dataStoreType, string file)
    {
        var dataSet = new DataSet();
        dataSet.ReadXml(file);

        foreach (var key in DatabaseList.Keys)
        {
            if (key != index) continue;
            foreach (var value in DatabaseList[key].Keys.Where(value => value == dataStoreType))
            {
                DatabaseList[key][value] = dataSet;
            }
        }
    }

    public void Initializer(string configPath, string finalPath)
    {
        _folderPath = configPath;
        _finalPath = finalPath;

        var files = Directory.GetFiles(_folderPath, "*.xml");
        var index = 1;
        
        foreach (var file in files)
        {
            var dataStoreType = PickXmlProperty(file, "DATA_STORE_TYPE");

            var dictionary = new Dictionary<string, DataSet>();
            dictionary.Add(dataStoreType, new DataSet());

            DatabaseList.Add(index, dictionary);
            GetDatabaseProperties(index, dataStoreType, file);
            index++;
        }
    }


    public void IndexDbSelector(int selectedIndex)
    {
        if (selectedIndex == 0) return;
        _selectedIndex = selectedIndex;
        //todo: REFACTOR and IMPLEMENT
        //var factory = DataStoreFactory.GetDataStoreByDataProviderID()
    }


    public void SetTables(string[] tables)
    {
        _tables = tables;
    }


    //todo: IMPLEMENT
    private static IDataStore CreateDataStore(string connectionString, string schema)
    {
        return new SQLServerGeomDataStore(connectionString, schema);
    }

    private static string PickXmlProperty(string file, string tag)
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
}