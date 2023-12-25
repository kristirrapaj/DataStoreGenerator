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
    private static string ConfigPath { get; set; }
    private static string FinalPath { get; set; }
    
    private static int _selectedCount= 0;

    private int SelectedIndex { get; set; }

    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;
    }

    private void InitDatabases()
    {
        if (_selectedCount != 2) return;
        
        _databaseManager.SetPaths(ConfigPath, FinalPath);
        _selectedCount = 0;

        foreach (string databaseName in _databaseManager.DataBaseNames.Values)
        {
            DataBaseNames.Add(databaseName);
        }
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
    }
}