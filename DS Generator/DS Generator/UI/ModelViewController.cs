using Microsoft.WindowsAPICodePack.Dialogs;

namespace DS_Generator.UI;

public class ModelViewController
{
    public List<string> SelectedTables = new List<string>();

    private DataBaseManager DataBaseManager;
    
    public List<string> GetAvaliableDatabases()
    {
        return DataBaseManager.AvailableDatabases.ToList();
    }

    public List<string> GetAvaliableDataProviders()
    {
        return DataBaseManager.AvailableDataProvider.ToList();
    }

    public List<string> GetAvaliableDataTables()
    {
        return DataBaseManager.AvailableTables.ToList();
    }

    public void SetDataStoreType(string dataStoreType)
    {
        DataBaseManager.DataStoreType = dataStoreType;
    }

    public void SetDatabase(string database)
    {
        DataBaseManager.Database = database;
    }

    public void SetTables()
    {
        DataBaseManager.Tables = SelectedTables.ToArray();
    }

    public void SetConfigurationFile(string file)
    {
        DataBaseManager = new DataBaseManager($"{file}");
    }

    public void SetOutputDirectory(string directory)
    {
        DataBaseManager.OutputConfigFilePath = $"{directory}";
    }

    public void AddToSelectedTables(string table)
    {
        SelectedTables.Add(table);
    }

    public void RemoveFromSelectedTables(string table)
    {
        SelectedTables.Remove(table);
    }

    public CommonOpenFileDialog? DialogCreator(string type)
    {
        switch (type)
        {
            case "xml":
                var dialog = new CommonOpenFileDialog
                {
                    InitialDirectory =
                        "C:\\Users\\K.Rrapaj\\DataStoreGenerator\\DS Generator\\DS Generator\\DataBaseConfig",
                    AddToMostRecentlyUsedList = false,
                    AllowNonFileSystemItems = false,
                    DefaultDirectory = "C:\\Users\\",
                    EnsureFileExists = true,
                    EnsurePathExists = true,
                    EnsureReadOnly = false,
                    EnsureValidNames = true,
                    Multiselect = false,
                    ShowPlacesList = true,
                    DefaultExtension = "xml"
                };
                return dialog;
            case "directory":
                var dialog2 = new CommonOpenFileDialog
                {
                    IsFolderPicker = true,
                    InitialDirectory =
                        "C:\\Users\\K.Rrapaj\\DataStoreGenerator\\DS Generator\\DS Generator\\DataBaseConfig",
                    AddToMostRecentlyUsedList = false,
                    AllowNonFileSystemItems = false,
                    DefaultDirectory = "C:\\Users\\",
                    EnsureFileExists = true,
                    EnsurePathExists = true,
                    EnsureReadOnly = false,
                    EnsureValidNames = true,
                    Multiselect = false,
                    ShowPlacesList = true,
                };
                return dialog2;
            default:
                Console.WriteLine("Invalid type");
                return null;
        }
    }
}