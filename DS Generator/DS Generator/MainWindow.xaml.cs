using System.Windows;
using System.Windows.Controls;
using DS_Generator.Database;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace DS_Generator;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private DataBaseManager _dbManager = new DataBaseManager();
    public List<string> Elements { get; set; }
    public MainWindow()
    {
        DataContext = this;
        InitializeComponent();
        PopulateComboBox();
    }

    private void PopulateComboBox()
    {
        Elements = _dbManager.AvailableDatabase.ToList();
        Elements.ForEach(x => CbDatabaseType.Items.Add(x));
    }


    private void OnComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedIndex = CbDatabaseType.SelectedIndex;
        _dbManager.SetDataProvider(selectedIndex);
    }
}