using System.Windows;
using System.Windows.Controls;

namespace DS_Generator;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public List<string> DataBaseNames { get; set; } = new List<string>();
    DatabaseManager databaseManager = new DatabaseManager();
    
    public int SelectedIndex { get; set; }

    public MainWindow()
    {
        InitDatabases();
        InitializeComponent();
        DataContext = this;
        
    }
    private void InitDatabases()
    {

        foreach (var item in databaseManager.DataBaseNames)
        {
            DataBaseNames.Add(item.Value);
        }
    }
    
    private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        SelectedIndex = ((ComboBox)sender).SelectedIndex;
        databaseManager.tuma(SelectedIndex);
    }
}

