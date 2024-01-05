using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace DS_Generator.UI;

public class MainWindowController {
    private List<string> mSelectedTables;
    private DataBaseManager mDataBaseManager;

    public MainWindowController() {
        mSelectedTables = [];
        mDataBaseManager = new DataBaseManager();
    }

    public List<string> GetAvaliableDatastores => mDataBaseManager.AvailableDatastores;
    public List<string> GetTablesList => mDataBaseManager.AvailableTables;

    private List<string> _supportedDataProviders = new List<string>();

    public List<string> SupportedDataProviders {
        get => _supportedDataProviders;
        set => _supportedDataProviders = value;
    }

    private void SetOutputDirectory(string directory) {
        mDataBaseManager.OutputConfigFilePath = $"{directory}";
    }


    // CHIAMALO QUANDO L'USER SELEZIONA UNA DATABASE
    // IN AUTOMATICO SETTA LE TABELLE DISPONIBILI
    public void SetDatabase(string database) {
        mDataBaseManager.Database = database;
    }

    // CHIAMALO QUANDO L'USER HA SELEZIONATO LE TABELLE ED È PRONTO PER GENERARE
    public void SetTables() {
        mDataBaseManager.Tables = mSelectedTables.ToArray();
    }

    public void ModifySelectedTables(string table, string command) {
        switch (command) {
            case "ADD":
                mSelectedTables.Add(table);
                break;
            case "REMOVE":
                mSelectedTables.Remove(table);
                break;
        }
    }

    public void ChangeConsoleText(Label label, string text, Brush color) {
        label.Content = $"{label.Content}\n\r {text}";
        label.Foreground = color;
    }

    private void SetConfigFilePath(string path) {
        mDataBaseManager.ConfigFilePath = path;
        mDataBaseManager.FetchDatabasesFromConfigurationFile();
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
                if (dialog.ShowDialog() != CommonFileDialogResult.Ok) {
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
                if (dialog.ShowDialog() != CommonFileDialogResult.Ok) {
                    throw new InvalidCastException();
                }

                SetOutputDirectory(dialog.FileName);
                break;
            default:
                Console.WriteLine("Invalid dialog type");
                throw new InvalidCastException();
        }
    }
}