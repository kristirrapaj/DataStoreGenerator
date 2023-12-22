using System.Data;
using System.IO;
using System.Windows.Shapes;
using DS_Generator.Database;

namespace DS_Generator;

public class DatabaseManager
{
    public List<string> DataBaseNames { get; set; } = new List<string>();
    
    private string FolderPath { get; set; }
    public DatabaseManager(string path)
    {
        this.FolderPath = path;
        string[] files = Directory.GetFiles(FolderPath, "*.xml");
        
        foreach (string file in files)
        {
            GetDataStoreType(file);
        }
    }

    private void GetDataStoreType(string file)
    {
        DataSet dataSet = new DataSet();
        dataSet.ReadXml(file);
        
        string dataStoreTypeTag = "DATA_STORE_TYPE";
        string dataStoreType = "";

        try
        {
            dataStoreType = dataSet.Tables[0].Rows[0][dataStoreTypeTag].ToString();
            
            if (dataStoreType == null || dataStoreType == string.Empty)
            {
                throw new Exception("DataStoreType is null or empty");
            }
            DataBaseNames.Add(dataStoreType);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}