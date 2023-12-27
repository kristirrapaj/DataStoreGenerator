using System.Windows;
using System.Windows.Controls;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace DS_Generator;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public List<string> DataBaseNames
    {
        get => _dataBaseNames;
        set => _dataBaseNames = value;
    }

    readonly DatabaseManager _databaseManager = new DatabaseManager();
    private List<string> _dataBaseNames = new List<string>();
    private static string? ConfigPath { get; set; }
    private static string FinalPath { get; set; }
    
    private static int _selectedCount= 0;

    public static string fefe = "Seleziona un database";

    private int SelectedIndex { get; set; } = 1;

    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;
        DataBasePanel.Visibility = Visibility.Hidden;
        TablePanel.Visibility = Visibility.Hidden;
    }

    private void InitDatabases()
    {
        if (_selectedCount != 2) return;
        
        _databaseManager.SetPaths(ConfigPath, FinalPath);
        _selectedCount = 0;

        foreach (string databaseName in _databaseManager.DatabaseList.Values.Select(tuple => tuple.Item1))
        {
            DataBaseNames.Add(databaseName);
        }
        TablePanel.Visibility = Visibility.Visible;
    }

    private void OnComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        SelectedIndex = ((ComboBox)sender).SelectedIndex;
        _databaseManager.DbSelector(SelectedIndex);
    }

    private void OnConfigurationPathSelected(object sender, RoutedEventArgs e)
    {
        using var dialog = new CommonOpenFileDialog();
        dialog.IsFolderPicker = true;
        if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
        {
            ConfigPath = dialog.FileName;
            ConfigPathTextBox.Text = ConfigPath;
        }
        _selectedCount++;
    }

    private void OnFinalPathSelected(object sender, RoutedEventArgs e)
    {
        using var dialog = new CommonOpenFileDialog();
        dialog.IsFolderPicker = true;
        if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
        {
            FinalPath = dialog.FileName;
            FinalPathTextBox.Text = ConfigPath;
        }
        _selectedCount++;
    }

    private void OnStartSelected(object sender, RoutedEventArgs e)
    {
        InitDatabases();
        DataBasePanel.Visibility = Visibility.Visible;
    }
}