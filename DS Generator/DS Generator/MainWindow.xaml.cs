using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DS_Generator.UI;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace DS_Generator;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window {
    private MainWindowController mMainWindowController;
    private List<string> mAvailableDatastore;
    private List<string> mAvailableDataProviders;
    private List<string> mAvailableDataTables;
    private List<string> mTablesSelected;

    private const string SelectConfigFileButton = "mSelectConfigFileButton";
    private const string DirectoryButton = "mDirectoryButton";
    private const string TablesListView = "mTablesListView";
    private const string RemoveButton = "mRemoveButton";

    public MainWindow() {
        DataContext = this;
        InitializeComponent();

        mMainWindowController = new MainWindowController();
        mTablesSelected = new List<string>();
        mAvailableDatastore = new List<string>();
        mAvailableDataProviders = new List<string>();
        mAvailableDataTables = new List<string>();
    }
    
    
    private void OnDataTableSelectionChanged(object sender, SelectionChangedEventArgs e) {
        var selectedItem = mTablesListView.SelectedItem.ToString();
        mMainWindowController.ModifySelectedTables(selectedItem, "ADD");
        if (mSelectedTablesListBox.Items.Contains(selectedItem)) return;
        mSelectedTablesListBox.Items.Add(selectedItem);
    }

    private void OnRemoveButtonClick(object sender, RoutedEventArgs e) {
        if (mSelectedTablesListBox.SelectedItem == null) return;
        var selectedItem = mSelectedTablesListBox.SelectedItem.ToString();
        mMainWindowController.ModifySelectedTables(selectedItem, "REMOVE");
        mSelectedTablesListBox.Items.Remove(selectedItem);
    }

    private void OnGenerateButtonSelected(object sender, RoutedEventArgs e) {
        mMainWindowController.SetTables();
        mConsoleLabel.Content = "Generated files successfully";
    }

    private void OnDataProviderSelectionChanged(object sender, SelectionChangedEventArgs e) {
        mAvailableDataTables = mMainWindowController.SetDataProvider(mCbDataProviderType.SelectedItem.ToString());
        mTablesListView.ItemsSource = mAvailableDataTables;
    }

    private void OnDatastoreSelectionChanged(object sender, SelectionChangedEventArgs e) {
        mAvailableDataProviders = mMainWindowController.SetDataStoreType(mCbDatastoreType.SelectedItem.ToString());
        mCbDataProviderType.ItemsSource = mAvailableDataProviders;
    }

    private void OnOpenDialogButtonClick(object sender, RoutedEventArgs e) {
        switch ((sender as Button)?.Name) {
            case SelectConfigFileButton:
                Console.WriteLine("SelectConfigFileButton");
                mMainWindowController.DialogCreator("XML");
                break;
            case DirectoryButton:
                Console.WriteLine("DirectoryButton");
                mMainWindowController.DialogCreator("DIRECTORY");
                mCbDatastoreType.ItemsSource = mMainWindowController.GetAvaliableDatastores;
                mAvailableDatastore = mMainWindowController.GetAvaliableDatastores;
                break;
        }
    }

    private void OnClearButtonClick(object sender, RoutedEventArgs e)
    {
        mSelectedTablesListBox.Items.Clear();
    }
}