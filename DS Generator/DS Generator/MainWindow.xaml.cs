using System.Windows;
using System.Windows.Controls;

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

    private int SelectedIndex { get; set; }

    public MainWindow()
    {
        InitDatabases();
        InitializeComponent();
        DataContext = this;
    }

    private void InitDatabases()
    {
        foreach (var item in _databaseManager.DataBaseNames)
        {
            DataBaseNames.Add(item.Value);
        }
    }

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        SelectedIndex = ((ComboBox)sender).SelectedIndex;
        _databaseManager.DbSelector(SelectedIndex);
    }
}