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
    private DataBaseManager _dbManager = new DataBaseManager();
    private List<string> AvaliableDatabases { get; set; }
    private List<string> AvaliableDataProviders { get; set; }

    private List<string> AvaliableDataTables { get; set; }

    DataBaseConfiguration dataBaseConfiguration = new DataBaseConfiguration();

    public MainWindow()
    {
        DataContext = this;
        InitializeComponent();
        dataBaseConfiguration.PopulateData();
        PopulateDatabaseComboBox();
        PopulateDataTableComboBox();
    }

    private void PopulateDatabaseComboBox()
    {
        AvaliableDatabases = _dbManager.AvailableDatabase.ToList();
        var placeholderItem = "Select an item...";
        AvaliableDatabases.Insert(0, placeholderItem);
        AvaliableDatabases.ForEach(x => CbDatabaseType.Items.Add(x));
    }

    private void PopulateDataProviderComboBox()
    {
        CbDataProviderType.Items.Clear();
        AvaliableDataProviders = _dbManager.AvailableDataProvider.ToList();
        var placeholderItem = "Select an item...";
        AvaliableDataProviders.Insert(0, placeholderItem);
        AvaliableDataProviders.ForEach(x => CbDataProviderType.Items.Add(x));
    }

    private void PopulateDataTableComboBox()
    {
        AvaliableDataTables = dataBaseConfiguration.dataTables;
        AvaliableDataTables.ForEach(x => CbDataTableType.Items.Add(x));
    }

    private void OnDatabaseSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedIndex = CbDatabaseType.SelectedIndex - 1;
        if (selectedIndex < 0) return;
        _dbManager.ChooseDatabase(selectedIndex);

        PopulateDataProviderComboBox();
    }

    private void OnDataProviderSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedIndex = CbDataProviderType.SelectedIndex - 1;
        if (selectedIndex < 0) return;
        _dbManager.ChooseDataProvider(selectedIndex);
    }

    private void OnDataTableSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        Console.WriteLine("Autismo");
    }
}
    