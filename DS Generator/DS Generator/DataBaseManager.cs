using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows.Shapes;
using DataStore.Interface;
using DS_Generator.Database;

namespace DS_Generator;

public class DatabaseManager
{
    public Dictionary<int, string> DataBaseNames { get; set; } = new Dictionary<int, string>();
    private string cnnStr;
    private string schema;
    
    //todo: Prendere questo path da UI
    private static string FolderPath = @"C:\\Users\\WOWMA\\Desktop\\Projects\\DataStoreGenerator\\DS Generator\\DS Generator\\DataBaseConfig";
    public DatabaseManager()
    {
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

    public void tuma(int selectedIndex)
    {
        if (selectedIndex != 0)
        {
            //todo: Prendere questi due var da UI
            var destPath = @"C:\Users\WOWMA\Desktop\Projects\DataStoreGenerator\DS Generator\DS Generator\output";
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
                    generator.Generate(destPath, tables);
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