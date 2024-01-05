using System.Windows;
using System.Windows.Controls;
using DS_Generator.UI;

namespace DS_Generator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowController mMainWindowController;
        private List<string> mAvailableDataProviders;
        private List<string> mAvailableDataTables;

        private const string SelectConfigFileButton = "mSelectConfigFileButton";
        private const string DirectoryButton = "mDirectoryButton";

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
            InitializeControllerAndLists();
        }

        private void InitializeControllerAndLists()
        {
            mMainWindowController = new MainWindowController();
            mAvailableDataProviders = new List<string>();
            mAvailableDataTables = new List<string>();
        }

        private void OnDataTableSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = GetSelectedItem(mTablesListView);
            UpdateSelectedTables(selectedItem, "ADD");
            AddToListBoxIfNotPresent(mSelectedTablesListBox, selectedItem);
        }

        private void OnRemoveButtonClick(object sender, RoutedEventArgs e)
        {
            var selectedItem = GetSelectedItem(mSelectedTablesListBox);
            if (selectedItem == null) return;
            UpdateSelectedTables(selectedItem, "REMOVE");
            mSelectedTablesListBox.Items.Remove(selectedItem);
        }

        private void OnGenerateButtonSelected(object sender, RoutedEventArgs e)
        {
            mMainWindowController.SetTables();
            mConsoleLabel.Content = "Generated files successfully";
        }

        private void OnDataProviderSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try {
                mMainWindowController.SetDatabase(mCbDataProviderType.SelectedItem?.ToString());
                UpdateDataTables();
            }
            catch (Exception exception) {
                Console.WriteLine(exception);
            }
        }

        private void OnOpenDialogButtonClick(object sender, RoutedEventArgs e)
        {
            var buttonName = (sender as Button)?.Name;
            HandleDialogButtonClick(buttonName);
            UpdateDataProviders();
        }

        private void OnClearButtonClick(object sender, RoutedEventArgs e)
        {
            mSelectedTablesListBox.Items.Clear();
        }

        private static string GetSelectedItem(ListBox listBox)
        {
            return listBox.SelectedItem?.ToString();
        }

        private void UpdateSelectedTables(string item, string action)
        {
            mMainWindowController.ModifySelectedTables(item, action);
        }

        private static void AddToListBoxIfNotPresent(ListBox listBox, string item)
        {
            if (!listBox.Items.Contains(item))
            {
                listBox.Items.Add(item);
            }
        }

        private void UpdateDataTables()
        {
            mAvailableDataTables = mMainWindowController.GetTablesList;
            mTablesListView.ItemsSource = mAvailableDataTables;
        }

        private void UpdateDataProviders()
        {
            mAvailableDataProviders = mMainWindowController.GetAvaliableDatastores;
            mCbDataProviderType.ItemsSource = mAvailableDataProviders;
        }

        private void HandleDialogButtonClick(string buttonName)
        {
            switch (buttonName)
            {
                case SelectConfigFileButton:
                    mMainWindowController.DialogCreator("XML");
                    break;
                case DirectoryButton:
                    mMainWindowController.DialogCreator("DIRECTORY");
                    UpdateDataProviders();
                    break;
            }
        }
        
    }
}
