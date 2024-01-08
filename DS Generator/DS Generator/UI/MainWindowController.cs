using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace DS_Generator.UI;

/// <summary>
/// Controls the main operations and user interactions in the DS_Generator application.
/// Manages database configurations, table selections, and UI updates.
/// </summary>
public class MainWindowController {
    private List<string> mSelectedTables;
    private DataBaseManager mDataBaseManager;
    
    /// <summary>
    /// Initializes a new instance of the MainWindowController class.
    /// Sets up the necessary data structures and initializes the DataBaseManager.
    /// </summary>
    public MainWindowController() {
        mSelectedTables = [];
        mDataBaseManager = new DataBaseManager();
    }

    /// <summary>
    /// Gets the list of available datastores from the DataBaseManager.
    /// </summary>
    public List<string> GetAvaliableDatastores => mDataBaseManager.AvailableDatastores;
    
    /// <summary>
    /// Gets the list of available tables from the DataBaseManager.
    /// </summary>
    public List<string> GetTablesList => mDataBaseManager.AvailableTables;
    
    /// <summary>
    /// Sets the output directory in the DataBaseManager for generated data.
    /// </summary>
    /// <param name="directory">The directory to set as the output path.</param>
    private void SetOutputDirectory(string directory) {
        mDataBaseManager.OutputConfigFilePath = $"{directory}";
    }

    /// <summary>
    /// Sets the database configuration in the DataBaseManager.
    /// Called when a user selects a database.
    /// </summary>
    /// <param name="database">The database to set.</param>
    public void SetDatabase(string database) {
        mDataBaseManager.Database = database;
    }

    /// <summary>
    /// Sets the tables to be processed in the DataBaseManager.
    /// Called when a user has finished selecting tables and is ready to generate the XML files.
    /// </summary>
    public void SetTables() {
        mDataBaseManager.Tables = mSelectedTables.ToArray();
    }

    ///<summary>
    /// Modifies the list of selected tables based on user commands (add or remove).
    /// </summary>
    /// <param name="table">The table to be added or removed.</param>
    /// <param name="command">The command indicating whether to add or remove the table.</param>
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

    /// <summary>
    /// Updates the text in a RichTextBox with the specified message and color.
    /// Used for displaying status messages and errors in the console part of the UI.
    /// </summary>
    /// <param name="richTextBox">The RichTextBox to update.</param>
    /// <param name="text">The text message to display.</param>
    /// <param name="color">The color of the text message.</param>
    public void ChangeConsoleText(RichTextBox richTextBox, string text, Brush color) {
        Paragraph paragraph = new Paragraph();
        paragraph.Inlines.Add(new Run(text));
        
        paragraph.Foreground = color;
        richTextBox.Document.Blocks.Add(paragraph);
    }

    /// <summary>
    /// Sets the configuration file path in the DataBaseManager.
    /// Fetches database configurations based on the provided configuration file.
    /// </summary>
    /// <param name="path">The path to the configuration file.</param>
    private void SetConfigFilePath(string path) {
        mDataBaseManager.ConfigFilePath = path;
        mDataBaseManager.FetchDatabasesFromConfigurationFile();
    }
    
    /// <summary>
    /// Creates and shows a dialog to the user for selecting files or directories.
    /// </summary>
    /// <param name="type">The type of dialog to show ("XML" for file selection, "DIRECTORY" for folder selection).</param>
    /// <returns>The path selected by the user.</returns>
    public string DialogCreator(string type) {
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
                    throw new Exception();
                }
                
                SetConfigFilePath(dialog.FileName);
                return dialog.FileName;
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
                    throw new Exception();
                }

                SetOutputDirectory(dialog.FileName);
                return dialog.FileName;
                break;
            default:
                throw new Exception();
        }
    }
}