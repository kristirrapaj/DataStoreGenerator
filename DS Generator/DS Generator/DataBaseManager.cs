using System.Data;
using System.IO;

namespace DS_Generator;

public class DataBaseManager {
    private const string FolderPath = @"../../../DataBaseConfig";

    private string _currentNameFilter;
    private Tuple<int, string> _currentDataProvider;
    private Tuple<int, string> _currentDatabase;
    private List<string> _availableTables;
    private List<string> _availableDatabase;
    private List<string> _availableDataProvider;

    public List<string> AvailableDataProvider {
        get => _availableDataProvider;
    }

    public List<string> AvailableDatabase {
        get {
            return _availableDatabase.Select(db => db.Split("\\")[1].Split(".")[0])
                .ToList(); // Trims the filepath returning only the file name (es: oracle instead of ../../../DataBaseConfig/oracle.xml)
        }
    }

    public List<string> AvailableTables {
        get => _availableTables;
    }

    public DataBaseManager() {
        _currentDataProvider = new Tuple<int, string>(-1, string.Empty);
        _currentDatabase = new Tuple<int, string>(-1, string.Empty);
        _availableDatabase = new List<string>();
        _availableDataProvider = new List<string>();
        _availableTables = new List<string>();
        _currentNameFilter = "";
        SetDatabasesConfig();
    }

    public void ChooseDatabase(int databaseIndex) {
        if (databaseIndex < 0 || databaseIndex >= _availableDatabase.Count)
            throw new InvalidDataException("index passed is: " + databaseIndex + ", which is invalid or out of range");
        _currentDatabase = new Tuple<int, string>(databaseIndex, _availableDatabase[databaseIndex]);
        SetAvailableDataProvider();
    }

    public void ChooseDataProvider(int dataProviderIndex) {
        _currentDataProvider = new Tuple<int, string>(dataProviderIndex, _availableDataProvider[dataProviderIndex]);
        SetAvailableTables();
    }

    private void SetAvailableTables() {
        var factory = DataStoreFactory.GetDataStoreByDataProviderID(_currentDataProvider.Item2);
        _availableTables = factory.GetExistingTables(_currentNameFilter).ToList();
    }


    private void SetAvailableDataProvider() {
        var file = _currentDatabase.Item2;
        var dataSet = new DataSet();
        dataSet.ReadXml(file);
        var dataProviderList =
            (from DataRow dataProvider in dataSet.Tables[0].Rows select TagPickerXml(dataProvider, "ID")).ToList();

        foreach (var id in dataProviderList) {
            Console.WriteLine(AvailableDatabase[_currentDatabase.Item1] + ", " + id);
        }

        _availableDataProvider = dataProviderList;
    }

    private void SetDatabasesConfig() {
        try {
            var files = Directory.GetFiles(FolderPath, "*.xml");
            foreach (var file in files) {
                _availableDatabase.Add(file);
                Console.WriteLine(file);
            }
        }
        catch (Exception e) {
            Console.WriteLine(e);
        }
    }

    private static string TagPickerXml(DataRow row, string tag) {
        try {
            var value = row[tag].ToString();

            if (string.IsNullOrEmpty(value)) {
                throw new Exception("Type is null or empty");
            }

            return value;
        }
        catch (Exception e) {
            Console.WriteLine(e);
            throw;
        }
    }
}