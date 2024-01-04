using System.Windows.Media;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace DS_Generator.UI;

public class MainWindowController {
    public List<string> SelectedTables = new List<string>();
    private DataBaseManager DataBaseManager;

    // DONE
    private List<string> GetAvaliableDatabases => DataBaseManager.AvailableDatabases;
    
    
    public List<string> GetAvaliableDataProviders() {
        return DataBaseManager.AvailableDataProvider;
    }

    public List<string> GetAvaliableDataTables() {
        return DataBaseManager.AvailableTables.ToList();
    }

   

    public void SetDatabase(string database) {
        DataBaseManager.Database = database;
    }

    public void SetTables() {
        DataBaseManager.Tables = SelectedTables.ToArray();
    }

    public void SetConfigurationFile(string file) {
        DataBaseManager = new DataBaseManager($"{file}");
    }

    public void SetOutputDirectory(string directory) {
        DataBaseManager.OutputConfigFilePath = $"{directory}";
    }

    public void AddToSelectedTables(string table) {
        SelectedTables.Add(table);
    }

    public void RemoveFromSelectedTables(string table) {
        SelectedTables.Remove(table);
    }

    //DONE
    public List<string> SetDataStoreType(string dataStoreType) {
        DataBaseManager.DataStoreType = dataStoreType;
        return GetAvaliableDatabases;
    }

    //DONE
    public void OnDialogBrowse(string type) {
        var dialog = DialogCreator(type);
        if (dialog!.ShowDialog() != CommonFileDialogResult.Ok) return;

        var selectedPath = dialog.FileName;
        switch (type) {
            case "XML":
                SetConfigurationFile(selectedPath);
                break;
            case "DIRECTORY":
                SetOutputDirectory(selectedPath);
                break;
        }

        ChangeConsoleText($"{type} set correctly.", Brushes.Green);
    }

    private static void ChangeConsoleText(string text, Brush color) {
        throw new NotImplementedException();
    }

    //DONE
    private static CommonOpenFileDialog? DialogCreator(string type) {
        switch (type) {
            case "XML":
                var dialog = new CommonOpenFileDialog {
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
            case "DIRECTORY":
                var dialog2 = new CommonOpenFileDialog {
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