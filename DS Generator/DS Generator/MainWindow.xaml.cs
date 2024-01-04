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

    /*private void OnWindowSizeChanged(object sender, SizeChangedEventArgs e) {
        throw new NotImplementedException();
    }*/

    private void OnDataTableSelectionChanged(object sender, SelectionChangedEventArgs e) {
        var selectedItem = mTablesListView.SelectedItem.ToString();
        if (selectedItem == null) return;
        mMainWindowController.ModifySelectedTables(selectedItem, "ADD");
        mSelectedTablesListBox.Items.Add(selectedItem);
    }

    private void OnRemoveButtonClick(object sender, RoutedEventArgs e) {
        var selectedItem = mSelectedTablesListBox.SelectedItem.ToString();
        if (selectedItem == null) return;
        mMainWindowController.ModifySelectedTables(selectedItem, "REMOVE");
        mSelectedTablesListBox.Items.Remove(selectedItem);
    }

    private void OnGenerateButtonSelected(object sender, RoutedEventArgs e) {
        mMainWindowController.SetTables();
    }

    private void OnDataProviderSelectionChanged(object sender, SelectionChangedEventArgs e) {
        mAvailableDataTables = mMainWindowController.SetDataProvider(mCbDataProviderType.SelectedItem.ToString());
    }

    private void OnDatastoreSelectionChanged(object sender, SelectionChangedEventArgs e) {
        mAvailableDataProviders = mMainWindowController.SetDataStoreType(mCbDatastoreType.SelectedItem.ToString()!);
    }

    private void OnOpenDialogButtonClick(object sender, RoutedEventArgs e) {
        switch ((sender as Button)?.Name) {
            case SelectConfigFileButton:
                mMainWindowController.DialogCreator("XML");
                break;
            case DirectoryButton:
                mMainWindowController.DialogCreator("DIRECTORY");
                mCbDatastoreType.ItemsSource = mMainWindowController.GetAvaliableDatastores;
                break;
        }
    }
}