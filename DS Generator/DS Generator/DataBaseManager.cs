using System.Data;
using System.Windows.Documents;
using DataStore.Factory;
using DataStore.Interface;

namespace DS_Generator;

public class DataBaseManager {
    private const string FolderPath = @"../../../DataBaseConfig";

    private string _currentDataStoreType;
    private string _currentId;
    private List<string> _availableTables;
    private List<string> _availableDatabases;
    private List<string> _availableDataProvider;
    private DataSet _configDataSet;
    private IDataStore _dsFactory;

    public List<string> AvailableDataProvider => _availableDataProvider;

    public List<string> AvailableDatabases => _availableDatabases;

    public List<string> AvailableTables => _availableTables;

    public DataBaseManager() {
        SetDatasetConfig();
        SetAvailableDataType();
    }

    public void ChooseDataStoreType(string dataStoreType) {
        _currentDataStoreType = dataStoreType;
        SetAvailableDatabase();
    }

    public void ChooseDatabase(string id) {
        _currentId = id;
        SetAvailableTables();
    }

    public void ChooseTable(string[] tables) {
        var sqlgen = new SqlGenerator(_dsFactory);
        sqlgen.Generate(@"../../../DataBaseConfig", tables);
    }


    private void SetAvailableTables() {
        var cnnStr = (
            from DataRow dataProvider in _configDataSet.Tables[0].Rows
            where TagPickerXml(dataProvider, "ID") == _currentId
            select TagPickerXml(dataProvider, "CONN_STR")
        ).ToList();

        var schema = (
            from DataRow dataProvider in _configDataSet.Tables[0].Rows
            where TagPickerXml(dataProvider, "ID") == _currentId
            select TagPickerXml(dataProvider, "SCHEMA")
        ).ToList();

        _dsFactory = DataStoreFactory.GetDataStore(_currentDataStoreType, connStr: cnnStr[0], schema: schema[0]);
        _availableTables = _dsFactory.GetExistingTables(owner: schema[0]).ToList();
        _dsFactory.DataProviderType = _currentDataStoreType;
    }

    private void SetAvailableDataType() {
        _availableDataProvider = (
            from DataRow dataProvider in _configDataSet.Tables[0].Rows
            select TagPickerXml(dataProvider, "DATA_STORE_TYPE")
        ).ToList();
    }

    private void SetAvailableDatabase() {
        _availableDatabases = (
            from DataRow dataProvider in _configDataSet.Tables[0].Rows
            where TagPickerXml(dataProvider, "DATA_STORE_TYPE") == _currentDataStoreType
            select TagPickerXml(dataProvider, "ID")
        ).ToList();
    }

    private void SetDatasetConfig() {
        try {
            var dataset = new DataSet();
            dataset.ReadXml(FolderPath + "/dataProviderConfig.xml");
            _configDataSet = dataset;
        }
        catch (Exception e) {
            Console.WriteLine(e);
        }
    }

    private static string TagPickerXml(DataRow row, string tag) {
        try {
            var value = row[tag].ToString();

            if (string.IsNullOrEmpty(value)) throw new Exception("Type is null or empty");

            return value;
        }
        catch (Exception e) {
            Console.WriteLine(e);
            throw;
        }
    }
}