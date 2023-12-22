using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DS_Generator;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private static string folderPath = @"C:\Users\K.Rrapaj\Desktop\DataBaseConfig";
    public List<string> dataBaseNames = new List<string>();
    public MainWindow()
    {
        InitializeComponent();
        
    }
    private void TestButton(object sender, RoutedEventArgs e)
    {
        DatabaseManager databaseManager = new DatabaseManager(folderPath);
        dataBaseNames = databaseManager.DataBaseNames;
    }
}