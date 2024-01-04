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
    
    private List<string> mAvailableDatastore; // todo: INIZIALIZZARE
    private List<string> mAvailableDataProviders; // todo: INIZIALIZZARE
    
    private const string SelectConfigFileButton = "mSelectConfigFileButton";
    private const string DirectoryButton = "mDirectoryButton";
    
    public MainWindow() {
        // REQUIRED
        DataContext = this;
        InitializeComponent();
        mMainWindowController = new MainWindowController();
    }

    private void OnWindowSizeChanged(object sender, SizeChangedEventArgs e) {
        throw new NotImplementedException();
    }
    
    private void PopulateDataTableComboBox() {
        TablesListView.ItemsSource = mMainWindowController.GetAvaliableDataTables();
    }
    
    

    private void OnDataProviderSelectionChanged(object sender, SelectionChangedEventArgs e) {
        mMainWindowController.SetDatabase(CbDatabaseType.SelectedItem.ToString());

        ChangeConsoleText($"Database set {CbDatabaseType.SelectedItem}", Brushes.Green);

        PopulateDataTableComboBox();
    }

    private void OnDataTableSelectionChanged(object sender, SelectionChangedEventArgs e) {
        var selectedItem = TablesListView.SelectedItem.ToString();
        mMainWindowController.AddToSelectedTables(selectedItem);

        SetVisibility(GeneratePanel, true);
        PopulateTablesComboBox(selectedItem);
    }

    private void PopulateTablesComboBox(string selectedItem) {
        SetVisibility(SelectedTablesPanel, true);
        SelectedTablesListBox.Items.Add(selectedItem);
    }

    private void OnTableRemoveButtonSelect(object sender, RoutedEventArgs e) {
        if (SelectedTablesListBox.SelectedItem == null) return;
        mMainWindowController.RemoveFromSelectedTables(SelectedTablesListBox.SelectedItem.ToString());
        SelectedTablesListBox.Items.Remove(SelectedTablesListBox.SelectedItem);
    }
    
    private void ChangeConsoleText(string text, Brush color) {
        ConsoleLabel.Content += "\n" + text;
        ConsoleLabel.Foreground = color;
    }
    
    private void OnGenerateButtonSelected(object sender, RoutedEventArgs e) {
        mMainWindowController.SetTables();
    }

    private void SetVisibility(object item, bool isVisible) {
        if (item is ComboBox comboBox) {
            comboBox.Visibility = isVisible ? Visibility.Visible : Visibility.Hidden;
        }

        if (item is Button button) {
            button.Visibility = isVisible ? Visibility.Visible : Visibility.Hidden;
        }

        if (item is StackPanel stackPanel) {
            stackPanel.Visibility = isVisible ? Visibility.Visible : Visibility.Hidden;
        }

        if (item is TextBox textBox) {
            textBox.Visibility = isVisible ? Visibility.Visible : Visibility.Hidden;
        }
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
                mCbDatastoreType.ItemsSource = mMainWindowController.GetAvaliableDataProviders();
                break;
        }
    }
}