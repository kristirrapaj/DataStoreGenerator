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

    private const string SelectConfigFileButton = "mSelectConfigFileButton";
    private const string DirectoryButton = "mDirectoryButton";

    public MainWindow() {
        mMainWindowController = new MainWindowController();
        mAvailableDatastore = new List<string>();
        mAvailableDataProviders = new List<string>();
        mAvailableDataTables = new List<string>();
        DataContext = this;
        InitializeComponent();
    }

    //TODO: Implement TABLE SYSTEM
    ////////////////////////////////////////////////////////////////////////////////
    private void OnDataTableSelectionChanged(object sender, SelectionChangedEventArgs e) {
        Console.WriteLine("autismo");
        /*var selectedItem = TablesListView.SelectedItem.ToString();
        mMainWindowController.AddToSelectedTables(selectedItem);
        PopulateTablesComboBox(selectedItem);*/
    }

    private void PopulateTablesComboBox(string selectedItem) {
        mSelectedTablesListBox.Items.Add(selectedItem);
    }

    private void OnTableRemoveButtonSelect(object sender, RoutedEventArgs e) {
        if (mSelectedTablesListBox.SelectedItem == null) return;
        mMainWindowController.RemoveFromSelectedTables(mSelectedTablesListBox.SelectedItem.ToString());
        mSelectedTablesListBox.Items.Remove(mSelectedTablesListBox.SelectedItem);
    }

    private void OnWindowSizeChanged(object sender, SizeChangedEventArgs e) {
        throw new NotImplementedException();
    }

    private void OnGenerateButtonSelected(object sender, RoutedEventArgs e) {
        mMainWindowController.SetTables();
    }
    ////////////////////////////////////////////////////////////////////////////////

    // DONE
    private void OnDataProviderSelectionChanged(object sender, SelectionChangedEventArgs e) {
        mAvailableDataTables = mMainWindowController.SetDataProvider(mCbDataProviderType.SelectedItem.ToString());
    }

    // DONE
    private void OnDatastoreSelectionChanged(object sender, SelectionChangedEventArgs e) {
        mAvailableDataProviders = mMainWindowController.SetDataStoreType(mCbDatastoreType.SelectedItem.ToString()!);
    }

    // DONE
    private void OnOpenDialogButtonClick(object sender, RoutedEventArgs e) {
        switch ((sender as Button)?.Name) {
            case SelectConfigFileButton:
                mMainWindowController.OnDialogBrowse("XML");
                break;
            case DirectoryButton:
                mMainWindowController.OnDialogBrowse("DIRECTORY");
                mCbDatastoreType.ItemsSource = mMainWindowController.GetAvaliableDatastores;
                break;
        }
    }
}