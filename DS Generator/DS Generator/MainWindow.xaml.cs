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
    private readonly ModelViewController _mvc = new();

    private double GridWidth;

    public MainWindow()
    {
        DataContext = this;
        InitializeComponent();

        SetVisibilities();
        SetMargins();
    }

    private void SetMargins()
    {
        MyGrid.Width = Width / 2;
    }
    
    private void OnwindowSizeChanged(object sender, SizeChangedEventArgs e)
    {
        SetMargins();
    }

    private void SetVisibilities()
    {
        SetVisibility(ProviderPanel, false);
        SetVisibility(DatabasePanel, false);
        SetVisibility(TablesPanel, false);
        SetVisibility(SelectedTablesPanel, false);
        SetVisibility(GeneratePanel, false);
    }

    private void PopulateDatabaseComboBox()
    {
        SetVisibility(DatabasePanel, true);
        CbDatabaseType.ItemsSource = _mvc.GetAvaliableDatabases();
    }

    private void PopulateDataProviderComboBox()
    {
        SetVisibility(ProviderPanel, true);
        CbDataProviderType.ItemsSource = _mvc.GetAvaliableDataProviders();
    }

    private void PopulateDataTableComboBox()
    {
        SetVisibility(TablesPanel, true);
        CbDataTableType.ItemsSource = _mvc.GetAvaliableDataTables();
    }

    private void OnDataProviderSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        _mvc.SetDataStoreType(CbDataProviderType.SelectedItem.ToString());
        
        ChangeConsoleText($"Data provider set {CbDataProviderType.SelectedItem}", Brushes.Green);
        
        PopulateDatabaseComboBox();
    }

    private void OnDatabaseSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        _mvc.SetDatabase(CbDatabaseType.SelectedItem.ToString());
        
        ChangeConsoleText($"Database set {CbDatabaseType.SelectedItem}", Brushes.Green);
        
        PopulateDataTableComboBox();
    }

    private void OnDataTableSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedItem = CbDataTableType.SelectedItem.ToString();
        _mvc.AddToSelectedTables(selectedItem);
        
        SetVisibility(GeneratePanel, true);
        PopulateTablesComboBox(selectedItem);
    }

    private void PopulateTablesComboBox(string selectedItem)
    {
        SetVisibility(SelectedTablesPanel, true);
        MyListBox.Items.Add(selectedItem);
    }

    private void OnTableRemoveButtonSelect(object sender, RoutedEventArgs e)
    {
        if (MyListBox.SelectedItem == null) return;
        _mvc.RemoveFromSelectedTables(MyListBox.SelectedItem.ToString());
        MyListBox.Items.Remove(MyListBox.SelectedItem);
    }

    private void OnBrowseButtonClick(object sender, RoutedEventArgs e)
    {
        var dialog = _mvc.DialogCreator("xml");

        if (dialog.ShowDialog() != CommonFileDialogResult.Ok) return;
        var selectedPath = dialog.FileName;

        if (selectedPath == null)
        {
            ChangeConsoleText("Invalid file path.", Brushes.Red);
            return;
        }
        
        ChangeConsoleText("File path set correctly.", Brushes.Green);
        
        _mvc.SetConfigurationFile(selectedPath);
        PopulateDataProviderComboBox();
    }
    
    private void ChangeConsoleText(string text, Brush color)
    {
        ConsoleText.Content += "\n" + text;
        ConsoleText.Foreground = color;
    }

    private void OnFinalBrowseButtonClick(object sender, RoutedEventArgs e)
    {
        var dialog = _mvc.DialogCreator("directory");
        if (dialog.ShowDialog() != CommonFileDialogResult.Ok) return;
        
        var selectedPath = dialog.FileName;
        _mvc.SetOutputDirectory(selectedPath);
        
        ChangeConsoleText($"Output directory set in {selectedPath}", Brushes.Green);
        
        SetVisibility(DatabasePanel, true);
    }

    private void OnGenerateButtonSelected(object sender, RoutedEventArgs e)
    {
        _mvc.SetTables();
    }
    
    private void SetVisibility(object item, bool isVisible)
    {
        if (item is ComboBox comboBox)
        {
            comboBox.Visibility = isVisible ? Visibility.Visible : Visibility.Hidden;
        }
        if (item is Button button)
        {
            button.Visibility = isVisible ? Visibility.Visible : Visibility.Hidden;
        }
        if (item is StackPanel stackPanel)
        {
            stackPanel.Visibility = isVisible ? Visibility.Visible : Visibility.Hidden;
        }
        
        if (item is TextBox textBox)
        {
            textBox.Visibility = isVisible ? Visibility.Visible : Visibility.Hidden;
        }
    }
    
}