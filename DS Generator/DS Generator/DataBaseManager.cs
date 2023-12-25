using System.Data;
using System.IO;
using DataStore.Interface;

namespace DS_Generator;

public class DatabaseManager
{
    public Dictionary<int, string> DataBaseNames { get; set; } = new Dictionary<int, string>();
    private string cnnStr;
    private string schema;
    
    //todo: Prendere questo path da UI
    private static string FolderPath;
    private static string FinalPath;
    public DatabaseManager()
    {
        
    }
    
    public void SetPaths(string configPath, string finalPath)
    {
        FolderPath = configPath;
        FinalPath = finalPath;
        
        string[] files = Directory.GetFiles(FolderPath, "*.xml");
        int index = 1;
        DataBaseNames.Add(0, "Seleziona un Database");
        foreach (string file in files)
        {
            GetDataStoreType(file, index);
            index++;
        }
    }

    private void GetDataStoreType(string file, int index)
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
    }

    public void DbSelector(int selectedIndex)
    {
        if (selectedIndex != 0)
        {
            string[] tables = new string[] { "EXT_MAP_NOTE_POINT", "EXT_MAP_NOTE_LINE", "EXT_MAP_NOTE_POLY" };
            ////
            IDataStore dataStore;
            switch (DataBaseNames[selectedIndex])
            {
                case "SQL_SERVER":
                    getCnnStr();
                    getSchema();
                    dataStore = new DataStore.SQLServerDataStore.SQLServerGeomDataStore(connStr: cnnStr,
                        schema: schema);
                    dataStore.DataProviderType = "SQL_SERVER";
                    var generator = new SqlGenerator(dataStore);
                    generator.Generate(FinalPath, tables);
                    break;
            }
        }
    }

    private void getSchema()
    {
        //todo: leggere da xml config (oracle.xml/sql_server.xml) lo schema, quale xml? -> selected index
    }

    private void getCnnStr()
    {
        //todo: leggere da xml config (oracle.xml/sql_server.xml) la connection string, quale xml? -> selected index
    }
}