﻿using System.Windows;
using System.Windows.Controls;
using DS_Generator.UI;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Collections.Generic;

namespace DS_Generator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowController mMainWindowController;
        private List<string> mAvailableDatastore;
        private List<string> mAvailableDataProviders;
        private List<string> mAvailableDataTables;
        private List<string> mTablesSelected;

        private const string SelectConfigFileButton = "mSelectConfigFileButton";
        private const string DirectoryButton = "mDirectoryButton";
        private const string TablesListView = "mTablesListView";
        private const string RemoveButton = "mRemoveButton";

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
            InitializeControllerAndLists();
        }

        private void InitializeControllerAndLists()
        {
            mMainWindowController = new MainWindowController();
            mTablesSelected = new List<string>();
            mAvailableDatastore = new List<string>();
            mAvailableDataProviders = new List<string>();
            mAvailableDataTables = new List<string>();
        }

        private void OnDataTableSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedItem = GetSelectedItem(mTablesListView);
            UpdateSelectedTables(selectedItem, "ADD");
            AddToListBoxIfNotPresent(mSelectedTablesListBox, selectedItem);
        }

        private void OnRemoveButtonClick(object sender, RoutedEventArgs e)
        {
            string selectedItem = GetSelectedItem(mSelectedTablesListBox);
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
            UpdateDataTables();
        }

        private void OnDatastoreSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateDataProviders();
        }

        private void OnOpenDialogButtonClick(object sender, RoutedEventArgs e)
        {
            string buttonName = (sender as Button)?.Name;
            HandleDialogButtonClick(buttonName);
        }

        private void OnClearButtonClick(object sender, RoutedEventArgs e)
        {
            mSelectedTablesListBox.Items.Clear();
        }

        private string GetSelectedItem(ListBox listBox)
        {
            return listBox.SelectedItem?.ToString();
        }

        private void UpdateSelectedTables(string item, string action)
        {
            mMainWindowController.ModifySelectedTables(item, action);
        }

        private void AddToListBoxIfNotPresent(ListBox listBox, string item)
        {
            if (!listBox.Items.Contains(item))
            {
                listBox.Items.Add(item);
            }
        }

        private void UpdateDataTables()
        {
            mAvailableDataTables = mMainWindowController.SetDataProvider(mCbDataProviderType.SelectedItem?.ToString());
            mTablesListView.ItemsSource = mAvailableDataTables;
        }

        private void UpdateDataProviders()
        {
            mAvailableDataProviders = mMainWindowController.SetDataStoreType(mCbDatastoreType.SelectedItem?.ToString());
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
                    UpdateDatastoreTypeSource();
                    break;
            }
        }

        private void UpdateDatastoreTypeSource()
        {
            mAvailableDatastore = mMainWindowController.GetAvaliableDatastores;
            mCbDatastoreType.ItemsSource = mAvailableDatastore;
        }
    }
}
