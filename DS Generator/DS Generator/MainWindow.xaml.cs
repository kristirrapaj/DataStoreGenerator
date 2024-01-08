using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DS_Generator.UI;

namespace DS_Generator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// serves as the user interface for a data generation application.
    /// It includes methods for handling user interactions such as selecting data tables,
    /// configuring data sources,
    /// and ultimately generating the XML files.
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowController mMainWindowController;
        private List<string> mAvailableDataProviders;
        private List<string> mAvailableDataTables;

        private const string SelectConfigFileButton = "mSelectConfigFileButton";
        private const string DirectoryButton = "mDirectoryButton";
        
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
            InitializeControllerAndLists();
        }

        /// <summary>
        /// Initializes the main window controller and lists for data providers and tables.
        /// </summary>
        private void InitializeControllerAndLists()
        {
            mMainWindowController = new MainWindowController();
            mAvailableDataProviders = new List<string>();
            mAvailableDataTables = new List<string>();
        }
        
        /// <summary>
        /// Handles changes in the data table selection.
        /// </summary>
        private void OnDataTableSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = GetSelectedItem(mTablesListView);
            UpdateSelectedTables(selectedItem, "ADD");
            AddToListBoxIfNotPresent(mSelectedTablesListBox, selectedItem);
        }
        
        /// <summary>
        /// Handles the click event of the remove button, removing selected items from the list.
        /// </summary>
        private void OnRemoveButtonClick(object sender, RoutedEventArgs e)
        {
            var selectedItem = GetSelectedItem(mSelectedTablesListBox);
            if (selectedItem == null) return;
            UpdateSelectedTables(selectedItem, "REMOVE");
            mSelectedTablesListBox.Items.Remove(selectedItem);
        }
        
        /// <summary>
        /// Handles the click event for generating the XML files based on selected options.
        /// </summary>
        private void OnGenerateButtonSelected(object sender, RoutedEventArgs e)
        {
            mMainWindowController.SetTables();
            mMainWindowController.ChangeConsoleText(mConsoleTextBox, "Successfully gerenated tables.", Brushes.Green);
        }
        
        /// <summary>
        /// Handles changes in the data provider selection.
        /// The 'try' block is used to catch exceptions thrown by the controller.
        /// These are then displayed in the console.
        /// </summary>
        private void OnDataProviderSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                mMainWindowController.SetDatabase(mCbDataProviderType.SelectedItem?.ToString());
                UpdateDataTables();
            }
            catch (Exception exception)
            {
                mMainWindowController.ChangeConsoleText(mConsoleTextBox, exception.ToString(), Brushes.Red);
            }
        }
        
        /// <summary>
        /// Handles the click event of open dialog buttons, opening file or directory selectors.
        /// </summary>
        private void OnOpenDialogButtonClick(object sender, RoutedEventArgs e)
        {
            var buttonName = (sender as Button)?.Name;
            HandleDialogButtonClick(buttonName);
            UpdateDataProviders();
        }
        
        /// <summary>
        /// Clears the selected tables.
        /// </summary>
        private void OnClearButtonClick(object sender, RoutedEventArgs e)
        {
            mSelectedTablesListBox.Items.Clear();
            mMainWindowController.ChangeConsoleText(mConsoleTextBox, "Cleared selected tables.", Brushes.Green);
        }
        
        /// <summary>
        /// Retrieves the selected item from a ListBox.
        /// </summary>
        /// <param name="listBox">The ListBox from which to retrieve the item.</param>
        /// <returns>The selected item as a string.</returns>
        private static string GetSelectedItem(ListBox listBox)
        {
            return listBox.SelectedItem?.ToString();
        }
        
        /// <summary>
        /// Updates the list of selected tables based on user actions.
        /// </summary>
        /// <param name="item">The item to add or remove.</param>
        /// <param name="action">The action to perform ("ADD" or "REMOVE").</param>
        private void UpdateSelectedTables(string item, string action)
        {
            mMainWindowController.ModifySelectedTables(item, action);
            mMainWindowController.ChangeConsoleText(mConsoleTextBox, "Tables modified.", Brushes.Green);
        }
        
        /// <summary>
        /// Adds an item to a ListBox if it is not already present.
        /// </summary>
        /// <param name="listBox">The ListBox to update.</param>
        /// <param name="item">The item to add.</param>
        private static void AddToListBoxIfNotPresent(ListBox listBox, string item)
        {
            if (!listBox.Items.Contains(item))
            {
                listBox.Items.Add(item);
            }
        }
        
        /// <summary>
        /// Updates the list of available data tables.
        /// </summary>
        private void UpdateDataTables()
        {
            mAvailableDataTables = mMainWindowController.GetTablesList;
            mTablesListView.ItemsSource = mAvailableDataTables;
        }
        
        /// <summary>
        /// Updates the list of available data providers.
        /// </summary>
        private void UpdateDataProviders()
        {
            mAvailableDataProviders = mMainWindowController.GetAvaliableDatastores;
            mCbDataProviderType.ItemsSource = mAvailableDataProviders;
        }
        
        /// <summary>
        /// Handles the click event of the open dialog buttons, opening file or directory selectors.
        /// <param name="buttonName">The button name to be considered.</param>
        /// </summary>
        private void HandleDialogButtonClick(string buttonName)
        {
            switch (buttonName)
            {
                case SelectConfigFileButton:
                    var cfg = mMainWindowController.DialogCreator("XML");
                    mSelectedConfigFilePathTextBox.Text = $"{cfg}";
                    mSelectedConfigFilePathTextBox.Foreground = Brushes.Green;
                    break;
                case DirectoryButton:
                    var dry = mMainWindowController.DialogCreator("DIRECTORY");
                    mSelectedDirectoryPathTextBox.Text = $"{dry}";
                    mSelectedDirectoryPathTextBox.Foreground = Brushes.Green;
                    UpdateDataProviders();
                    break;
            }
        }
    }
}
