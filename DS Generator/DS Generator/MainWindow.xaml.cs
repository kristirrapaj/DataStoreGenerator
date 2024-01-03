using System.ComponentModel;
using System.IO;
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
    private readonly ModelViewController _mvc = new();

    public MainWindow()
    {
        DataContext = this;
        InitializeComponent();
    }

    private void PopulateDatabaseComboBox()
    {
        CbDatabaseType.ItemsSource = _mvc.GetAvaliableDatabases();
    }

    private void PopulateDataProviderComboBox()
    {
        CbDataProviderType.ItemsSource = _mvc.GetAvaliableDataProviders();
    }

    private void PopulateDataTableComboBox()
    {
        CbDataTableType.ItemsSource = _mvc.GetAvaliableDataTables();
    }

    private void OnDataProviderSelectionChanged(object sender, SelectionChangedEventArgs e)
    {

        var selectedItem = CbDataProviderType.SelectedItem.ToString();
        _mvc.SetDataStoreType(selectedItem);
        
        PopulateDatabaseComboBox();
    }

    private void OnDatabaseSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedItem = CbDatabaseType.SelectedItem.ToString();
        Console.WriteLine(selectedItem);
        _mvc.SetDatabase(selectedItem);
        
        PopulateDataTableComboBox();
    }

    private void OnDataTableSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedItem = CbDataTableType.SelectedItem.ToString();
        _mvc.AddToSelectedTables(selectedItem);
        
        PopulateTablesComboBox(selectedItem);
    }

    private void PopulateTablesComboBox(string selectedItem)
    {
        MyListBox.Items.Add(selectedItem);
    }

    private void OnTableRemoveButtonSelect(object sender, RoutedEventArgs e)
    {
        var selectedItem = MyListBox.SelectedItem;
        _mvc.RemoveFromSelectedTables(selectedItem.ToString());
    }

    private void OnBrowseButtonClick(object sender, RoutedEventArgs e)
    {
        var dialog = _mvc.DialogCreator("xml");

        if (dialog.ShowDialog() != CommonFileDialogResult.Ok) return;
        var selectedPath = dialog.FileName;
        
        _mvc.SetConfigurationFile(selectedPath);
        PopulateDataProviderComboBox();
    }

    private void OnFinalBrowseButtonClick(object sender, RoutedEventArgs e)
    {
        var dialog = _mvc.DialogCreator("directory");
        if (dialog.ShowDialog() != CommonFileDialogResult.Ok) return;
        
        var selectedPath = dialog.FileName;
        _mvc.SetOutputDirectory(selectedPath);
    }

    private void OnGenerateButtonSelected(object sender, RoutedEventArgs e)
    {
        _mvc.SetTables();
    }
    
}