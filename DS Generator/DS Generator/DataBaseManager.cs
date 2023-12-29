using System.Data;
using System.IO;

namespace DS_Generator;

public class DatabaseManager {
    private const string FolderPath = @"../../../DataBaseConfig";

    private List<string> _availableDataProvider;
    private List<string> _availableDatabase;

    public DatabaseManager() {
        _availableDatabase = new List<string>();
        _availableDataProvider = new List<string>();
        Initializer();
    }

    public List<string> AvailableDatabase {
        get {
            return _availableDatabase.Select(db => db.Split("\\")[1].Split(".")[0])
                .ToList(); // Trims the filepath returning only the file name (es: oracle instead of ../../../DataBaseConfig/oracle.xml)
        }
    }

    public List<string> AvailableDataProvider {
        get => _availableDataProvider;
    }

    public void SetDataProvider(int databaseTypeIndex) {
        var dataProviderList = new List<string>();
        var file = _availableDatabase[databaseTypeIndex];
        var dataSet = new DataSet();
        dataSet.ReadXml(file);
        _availableDataProvider = dataProviderList;
    }


    private void Initializer() {
        try {
            var files = Directory.GetFiles(FolderPath, "*.xml");
            foreach (var file in files) _availableDatabase.Add(file);
        }
        catch (Exception e) {
            Console.WriteLine(e);
        }
    }
}