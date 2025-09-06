using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ConferenceManager.WPF.Models;
using ConferenceManager.WPF.Services;
using ConferenceManager.WPF.ViewModels;

namespace ConferenceManager.WPF.Views
{
    public partial class SpeakersView : UserControl
    {
        public SpeakersView()
        {
            InitializeComponent();
        }

        private async void SpeakersView_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is ConferenceManager.WPF.ViewModels.SpeakersViewModel vm)
            {
                await vm.LoadSpeakersAsync();
            }
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (DataContext is ConferenceManager.WPF.ViewModels.SpeakersViewModel vm)
                {
                    vm.SearchText = textBox.Text;
                }
            }
        }

        private void StatusFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                if (DataContext is ConferenceManager.WPF.ViewModels.SpeakersViewModel vm)
                {
                    vm.StatusFilter = selectedItem.Content.ToString();
                }
            }
        }

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ConferenceManager.WPF.ViewModels.SpeakersViewModel vm)
            {
                await vm.AddSpeakerAsync();
            }
        }

        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ConferenceManager.WPF.ViewModels.SpeakersViewModel vm)
            {
                await vm.EditSpeakerAsync();
            }
        }

        private async void DeleteSpeaker_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ConferenceManager.WPF.ViewModels.SpeakersViewModel vm)
            {
                await vm.DeleteSpeakerAsync();
            }
        }

        private void SpeakersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is DataGrid dataGrid)
            {
                if (DataContext is ConferenceManager.WPF.ViewModels.SpeakersViewModel vm)
                {
                    vm.SelectedSpeaker = dataGrid.SelectedItem as Speaker;
                }
            }
        }
    }
} 