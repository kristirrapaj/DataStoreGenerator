using System.Windows.Media;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace DS_Generator.UI;

public class MainWindowController {
    public List<string> SelectedTables = new List<string>();
    private DataBaseManager DataBaseManager;

    //DONE
    public List<string> GetAvaliableDatastores => DataBaseManager.AvailableDatastores;

    //DONE
    private List<string> GetAvaliableProviders => DataBaseManager.AvailableDataProviders;

    //DONE
    public List<string> GetAvaliableDataTables => DataBaseManager.AvailableTables;


    public void SetTables() {
        DataBaseManager.Tables = SelectedTables.ToArray();
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

    private static void ChangeConsoleText(string text, Brush color) {
        throw new NotImplementedException();
    }

    //DONE
    public void SetConfigurationFile(string file) {
        DataBaseManager = new DataBaseManager($"{file}");
    }

    public List<string> SetDataProvider(string dataProvider) {
        DataBaseManager.DataProvider = dataProvider;
        return GetAvaliableDataTables;
    }

    //DONE
    public List<string> SetDataStoreType(string dataStoreType) {
        DataBaseManager.DataStoreType = dataStoreType;
        return GetAvaliableProviders;
    }

    //DONE
    public string OnDialogBrowse(string type) {
        var dialog = DialogCreator(type);
        ChangeConsoleText($"{type} set correctly.", Brushes.Green);
        return dialog ?? "Error";
    }

    //DONE
    private static string? DialogCreator(string type) {
        CommonOpenFileDialog dialog;
        switch (type) {
            case "XML":
                dialog = new CommonOpenFileDialog {
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
                return dialog.ShowDialog() != CommonFileDialogResult.Ok ? null : dialog.FileName;
            case "DIRECTORY":
                dialog = new CommonOpenFileDialog {
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
                return dialog.ShowDialog() != CommonFileDialogResult.Ok ? null : dialog.FileName;
            default:
                Console.WriteLine("Invalid type");
                return null;
        }
    }
}