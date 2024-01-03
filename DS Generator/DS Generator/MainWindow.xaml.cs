using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using DS_Generator.Database;
using DS_Generator.UI;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace DS_Generator;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private DataBaseManager _dbManager = new();
    private List<string> AvailableDatabases { get; set; }
    private List<string> AvailableDataProviders { get; set; }

    private List<string> AvailableDataTables { get; set; }
    
    public List<string> _tablesSelected = new();

    public MainWindow()
    {
        DataContext = this;
        InitializeComponent();
        PopulateDataProviderComboBox();
    }

    private void PopulateDatabaseComboBox()
    {
        CbDatabaseType.Items.Clear();
        AvailableDatabases = _dbManager.AvailableDatabases.ToList();
        var placeholderItem = "Select an item...";
        AvailableDatabases.Insert(0, placeholderItem);
        AvailableDatabases.ForEach(x => CbDatabaseType.Items.Add(x));
    }

    private void PopulateDataProviderComboBox()
    {
        CbDataProviderType.Items.Clear();
        AvailableDataProviders = _dbManager.AvailableDataProvider.ToList();
        var placeholderItem = "Select an item...";
        AvailableDataProviders.Insert(0, placeholderItem);
        AvailableDataProviders.ForEach(x => CbDataProviderType.Items.Add(x));
    }

    private void PopulateDataTableComboBox()
    {
        CbDataTableType.Items.Clear();
        CbDataTableType.Items.Add("Select an item...");
        CbDataTableType.SelectedIndex = 0;
        AvailableDataTables = _dbManager.AvailableTables.ToList();
        AvailableDataTables.ForEach(x => CbDataTableType.Items.Add(x));
    }

    private void OnDataProviderSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (CbDataProviderType.SelectedIndex <= 0) return;
        var selectedItem = AvailableDataProviders[CbDataProviderType.SelectedIndex];
        _dbManager.ChooseDataStoreType(selectedItem);
        PopulateDatabaseComboBox();
        Console.WriteLine(selectedItem);
    }

    private void OnDatabaseSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (CbDatabaseType.SelectedIndex <= 0) return;
        var selectedItem = AvailableDatabases[CbDatabaseType.SelectedIndex];
        Console.WriteLine(selectedItem);
        _dbManager.ChooseDatabase(selectedItem);
        PopulateDataTableComboBox();
    }

    private void OnDataTableSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        Console.WriteLine("Selected table changed");
        if (CbDataTableType.SelectedIndex <= 0) return;
        var selectedItem = AvailableDataTables[CbDataTableType.SelectedIndex -1];
        Console.WriteLine(selectedItem);
        _tablesSelected.Add(selectedItem);
        //_dbManager.ChooseTable(_tablesSelected);
        PopulateTablesComboBox(selectedItem);
    }

    private void PopulateTablesComboBox(string selectedItem)
    {
        MyListBox.Items.Add(selectedItem);
    }

    private void OnTableRemoveButtonSelect(object sender, RoutedEventArgs e)
    {
        var selectedItem = MyListBox.SelectedItem;
        MyListBox.Items.Remove(selectedItem);
    }

    private void OnBrowseButtonClick(object sender, RoutedEventArgs e)
    {
        var dialog = new CommonOpenFileDialog
        {
            IsFolderPicker = true,
            InitialDirectory = "C:\\Users\\",
            AddToMostRecentlyUsedList = false,
            AllowNonFileSystemItems = false,
            DefaultDirectory = "C:\\Users\\",
            EnsureFileExists = true,
            EnsurePathExists = true,
            EnsureReadOnly = false,
            EnsureValidNames = true,
            Multiselect = false,
            ShowPlacesList = true
        };

        if (dialog.ShowDialog() != CommonFileDialogResult.Ok) return;
        var selectedPath = dialog.FileName;
        //dai il path a daniele
    }

    private void OnFinalBrowseButtonClick(object sender, RoutedEventArgs e)
    {
        var dialog = new CommonOpenFileDialog
        {
            IsFolderPicker = true,
            InitialDirectory = "C:\\Users\\",
            AddToMostRecentlyUsedList = false,
            AllowNonFileSystemItems = false,
            DefaultDirectory = "C:\\Users\\",
            EnsureFileExists = true,
            EnsurePathExists = true,
            EnsureReadOnly = false,
            EnsureValidNames = true,
            Multiselect = false,
            ShowPlacesList = true
        };

        if (dialog.ShowDialog() != CommonFileDialogResult.Ok) return;
        var selectedPath = dialog.FileName;
        //dai il path a daniele
    }

    private void OnGenerateButtonSelected(object sender, RoutedEventArgs e)
    {
        //daniele chiama il metodo di daniele
        Console.WriteLine("Generate button selected");
    }
}