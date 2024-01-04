using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace DS_Generator.UI;

public class MainWindowController {
    private List<string> SelectedTables = [];
    private DataBaseManager DataBaseManager = new();
    
    public List<string> GetAvaliableDatastores => DataBaseManager.AvailableDatastores;

    private List<string> GetAvaliableProviders => DataBaseManager.AvailableDataProviders;

    public List<string> GetAvaliableDataTables => DataBaseManager.AvailableTables;
    
    //public string ConfigFilePath { set => mConfigFilePath = value; }


    public void SetTables() {
        DataBaseManager.Tables = SelectedTables.ToArray();
    }


    public void SetOutputDirectory(string directory) {
        DataBaseManager.OutputConfigFilePath = $"{directory}";
    }

    public void ModifySelectedTables(string table, string command) {
        switch (command) {
            case "ADD":
                SelectedTables.Add(table);
                break;
            case "REMOVE":
                SelectedTables.Remove(table);
                break;
        }
    }

    private static void ChangeConsoleText(string text, Brush color) {
        throw new NotImplementedException();
    }

    public List<string> SetDataProvider(string dataProvider) {
        DataBaseManager.DataProvider = dataProvider;
        return GetAvaliableDataTables;
    }

    public List<string> SetDataStoreType(string dataStoreType) {
        DataBaseManager.DataStoreType = dataStoreType;
        return GetAvaliableProviders;
    }
    
    private void SetConfigFilePath(string path) {
        DataBaseManager.ConfigFilePath = path;
        DataBaseManager.Initialize();
    }
    
    public void DialogCreator(string type) {
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
                if (dialog.ShowDialog() != CommonFileDialogResult.Ok)
                {
                    throw new InvalidCastException();
                } 
                SetConfigFilePath(dialog.FileName);
                break;
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
                if (dialog.ShowDialog() != CommonFileDialogResult.Ok)
                {
                    throw new InvalidCastException();
                } 
                SetOutputDirectory(dialog.FileName);
                break;
            default:
                ChangeConsoleText("Invalid dialog type", Brushes.Red);
                throw new InvalidCastException();
        }
        
    }
}