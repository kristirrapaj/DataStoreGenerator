using System.Data;
using System.IO;

namespace DS_Generator;

public class DataBaseManager {
    private const string FolderPath = @"../../../DataBaseConfig";

    private Tuple<int, string> _currentDataProvider;
    private Tuple<int, string> _currentDatabase;
    private List<string> _availableDatabase;
    private List<string> _availableDataProvider;

    public List<string> AvailableDataProvider {
        get => _availableDataProvider;
        set => _availableDataProvider = value;
    }

    public List<string> AvailableDatabase {
        get {
            return _availableDatabase.Select(db => db.Split("\\")[1].Split(".")[0])
                .ToList(); // Trims the filepath returning only the file name (es: oracle instead of ../../../DataBaseConfig/oracle.xml)
        }
    }

    public DataBaseManager() {
        _currentDataProvider = new Tuple<int, string>(-1, string.Empty);
        _currentDatabase = new Tuple<int, string>(-1, string.Empty);
        _availableDatabase = new List<string>();
        _availableDataProvider = new List<string>();
        AvailableDataProvider = [];
        SetDatabasesConfig();
    }

    public void ChooseDatabase(int databaseIndex) {
        if (databaseIndex < 0 || databaseIndex >= _availableDatabase.Count)
            throw new InvalidDataException("index passed is: " + databaseIndex + ", which is invalid or out of range");
        _currentDatabase = new Tuple<int, string>(databaseIndex, _availableDatabase[databaseIndex]);
        SetAvailableDataProvider();
    }

    public void ChooseDataProvider(int dataProviderIndex) {
        if (dataProviderIndex < 0 || dataProviderIndex >= _availableDataProvider.Count)
            throw new InvalidDataException("index passed is: " + dataProviderIndex +
                                           ", which is invalid or out of range");
        _currentDataProvider = new Tuple<int, string>(dataProviderIndex, _availableDatabase[dataProviderIndex]);
    }

    private void SetAvailableDataProvider() {
        var dataProviderList = new List<string>();
        var file = _currentDatabase.Item2;
        var dataSet = new DataSet();
        dataSet.ReadXml(file);
        foreach (DataRow dataProvider in dataSet.Tables[0].Rows) {
            dataProviderList.Add(TagPickerXml(dataProvider, "ID"));
        }

        foreach (var sesso in dataProviderList) {
            Console.WriteLine("3) Dani: " + AvailableDatabase[_currentDatabase.Item1] + ", " + sesso);
        }
        AvailableDataProvider = dataProviderList;
    }

    private void SetDatabasesConfig() {
        try {
            var files = Directory.GetFiles(FolderPath, "*.xml");
            foreach (var file in files) {
                _availableDatabase.Add(file);
                Console.WriteLine("1) Dani: " + file);
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