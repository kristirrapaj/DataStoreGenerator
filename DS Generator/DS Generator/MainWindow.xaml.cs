using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DS_Generator.Database;
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
        foreach (var avaliableDatabase in AvailableDataTables) Console.WriteLine(avaliableDatabase);
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
        //_dbManager.ChooseTable(selectedItem);
    }
}