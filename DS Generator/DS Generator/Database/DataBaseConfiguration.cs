using System.Data;
using System.IO;
using DataStore.Factory;
using DataStore.Interface;

namespace DS_Generator.Database;

public class DataBaseConfiguration
{
    public List<string> dataBaseTypes = new();
    public List<string> dataProviderIds = new();
    public List<string> dataTables = new();

    private Dictionary<int, Tuple<string, string, string>> _databaseList = new();

    public void PopulateData()
    {
        dataBaseTypes.Add("ORACLE");
        dataBaseTypes.Add("SQLSERVER");

        dataProviderIds.Add("DEFAULT_DATA");
        dataProviderIds.Add("DEFAULT_SYSTEM");
        dataProviderIds.Add("METADATA_TLC");

        dataTables.Add("WATER");
        dataTables.Add("GAS");
        dataTables.Add("ELECTRICITY");
        dataTables.Add("THERMAL");
        dataTables.Add("DISTRICT_HEATING");
        dataTables.Add("DISTRICT_COOLING");
        dataTables.Add("DISTRICT_WATER");
        dataTables.Add("DISTRICT_GAS");
        dataTables.Add("DISTRICT_ELECTRICITY");
        dataTables.Add("DISTRICT_THERMAL");
    }

    public string GetDatabaseType(int index)
    {
        return dataBaseTypes[index];
    }

    public string GetDataProviderId(int index)
    {
        return dataProviderIds[index];
    }

    public List<string> SearchDataTables(string nameStream)
    {
        foreach (var dataTable in dataTables)
            if (dataTable.Contains(nameStream))
            {
                dataTables.Clear();
                dataTables.Add(dataTable);
            }

        return dataTables;
    }
}