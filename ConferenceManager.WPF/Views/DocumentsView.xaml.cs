using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ConferenceManager.WPF.Models;
using ConferenceManager.WPF.Services;
using ConferenceManager.WPF.ViewModels;
using ConferenceManager.WPF.Views;

namespace ConferenceManager.WPF.Views
{
    public partial class DocumentsView : UserControl
    {
        public DocumentsView()
        {
            InitializeComponent();
        }

        private async void DocumentsView_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is ConferenceManager.WPF.ViewModels.DocumentsViewModel vm)
            {
                await vm.LoadDataAsync();
            }
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox && DataContext is DocumentsViewModel vm)
            {
                vm.SearchText = textBox.Text;
            }
        }

        private void ConferenceFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedItem is Conference selectedConference && DataContext is DocumentsViewModel vm)
            {
                vm.SelectedConferenceId = selectedConference.Id;
            }
        }

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is DocumentsViewModel vm)
            {
                await vm.AddDocumentAsync();
            }
        }

        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is DocumentsViewModel vm && vm.SelectedDocument != null)
            {
                await vm.UpdateDocumentAsync();
            }
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is DocumentsViewModel vm && vm.SelectedDocument != null)
            {
                await vm.DeleteDocumentAsync();
            }
        }

        private void TypeFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedItem is ComboBoxItem selectedItem && DataContext is DocumentsViewModel vm)
            {
                vm.SelectedDocumentType = selectedItem.Content.ToString();
            }
        }

        private void DocumentsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is DataGrid dataGrid && DataContext is DocumentsViewModel vm)
            {
                vm.SelectedDocument = dataGrid.SelectedItem as Document;
            }
        }

        private async void ViewButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is DocumentsViewModel vm && vm.SelectedDocument != null)
            {
                await vm.ViewDocumentAsync();
            }
        }

        private async void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is DocumentsViewModel vm && vm.SelectedDocument != null)
            {
                await vm.DownloadDocumentAsync(vm.SelectedDocument);
            }
        }

        private async void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is DocumentsViewModel vm)
            {
                await vm.ExportDocumentsAsync();
            }
        }

        private void ConferenceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedItem is Conference selectedConference && DataContext is DocumentsViewModel vm)
            {
                vm.SelectedConference = selectedConference;
            }
        }
    }
} 