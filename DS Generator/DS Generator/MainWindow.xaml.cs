using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DS_Generator.UI;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace DS_Generator;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private Controller mController;
    
    public MainWindow()
    {
        DataContext = this;
        InitializeComponent();
        mController = new Controller();
    }
    
    private void OnWindowSizeChanged(object sender, SizeChangedEventArgs e)
    {
        throw new NotImplementedException();
    }
    
    private void PopulateDatabaseComboBox()
    {
        CbDatabaseType.ItemsSource = mController.GetAvaliableDatabases();
    }

    private void PopulateDataProviderComboBox()
    {
        CbDataProviderType.ItemsSource = mController.GetAvaliableDataProviders();
    }

    private void PopulateDataTableComboBox()
    {
        CbDataTableType.ItemsSource = mController.GetAvaliableDataTables();
    }

    private void OnDataProviderSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        mController.SetDataStoreType(CbDataProviderType.SelectedItem.ToString());
        
        PopulateDatabaseComboBox();
    }

    private void OnDatabaseSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        mController.SetDatabase(CbDatabaseType.SelectedItem.ToString());
        PopulateDataTableComboBox();
    }

    private void OnDataTableSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedItem = CbDataTableType.SelectedItem.ToString();
        mController.AddToSelectedTables(selectedItem);
        
        PopulateTablesComboBox(selectedItem);
    }

    private void PopulateTablesComboBox(string selectedItem)
    {
        MyListBox.Items.Add(selectedItem);
    }

    private void OnTableRemoveButtonSelect(object sender, RoutedEventArgs e)
    {
        if (MyListBox.SelectedItem == null) return;
        mController.RemoveFromSelectedTables(MyListBox.SelectedItem.ToString());
        MyListBox.Items.Remove(MyListBox.SelectedItem);
    }

    private void OnBrowseButtonClick(object sender, RoutedEventArgs e)
    {
        var dialog = mController.DialogCreator("xml");

        if (dialog.ShowDialog() != CommonFileDialogResult.Ok) return;
        var selectedPath = dialog.FileName;

        if (selectedPath == null)
        {
            return;
        }
        
        mController.SetConfigurationFile(selectedPath);
        PopulateDataProviderComboBox();
    }
    

    private void OnFinalBrowseButtonClick(object sender, RoutedEventArgs e)
    {
        var dialog = mController.DialogCreator("directory");
        if (dialog.ShowDialog() != CommonFileDialogResult.Ok) return;
        
        var selectedPath = dialog.FileName;
        mController.SetOutputDirectory(selectedPath);
    }

    private void OnGenerateButtonSelected(object sender, RoutedEventArgs e)
    {
        mController.SetTables();
    }
    
}