using System.Data;
using System.IO;

namespace DS_Generator.Database;

public class DataBaseConfiguration
{
    public List<string> dataBaseTypes = new List<string>();
    public List<string> dataProviderIds = new List<string>();
    public string[] dataTables;
    
    private Dictionary<int, Tuple<string, string, string>> _databaseList =
        new Dictionary<int, Tuple<string, string, string>>();
    public void PopulateData()
    {
        dataBaseTypes.Add("ORACLE");
        dataBaseTypes.Add("SQLSERVER");
        
        dataProviderIds.Add("DEFAULT_DATA");
        dataProviderIds.Add("DEFAULT_SYSTEM");
        dataProviderIds.Add("METADATA_TLC");
        
        dataTables = new string[] { "ACQ_PERDITA", "ACQ_ZONA_ISPEZ" };
    }
    
    public string GetDatabaseType(int index)
    {
        return dataBaseTypes[index];
    }
    
    public string GetDataProviderId(int index)
    {
        return dataProviderIds[index];
    }
}

