using System.Windows;
using System.Windows.Controls;

namespace DS_Generator;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private static string folderPath = @"C:\Users\K.Rrapaj\Desktop\DataBaseConfig";
    public List<string> DataBaseNames { get; set; } = new List<string>();
    
    public int SelectedIndex { get; set; }

    public MainWindow()
    {
        InitDatabases();
        InitializeComponent();
        DataContext = this;
        
    }
    private void InitDatabases()
    {
        DataBaseNames.Add("Seleziona un database");
        DatabaseManager databaseManager = new DatabaseManager(folderPath);

        foreach (var item in databaseManager.DataBaseNames)
        {
            DataBaseNames.Add(item);
        }
    }
    
    private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        SelectedIndex = ((ComboBox)sender).SelectedIndex;
        Console.WriteLine(SelectedIndex);
    }
}

