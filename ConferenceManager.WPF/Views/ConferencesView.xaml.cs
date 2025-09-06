using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using ConferenceManager.WPF.Models;
using ConferenceManager.WPF.Services;
using System.Threading.Tasks;

namespace ConferenceManager.WPF.Views
{
    public partial class ConferencesView : UserControl
    {
        private readonly IConferenceService _conferenceService;
        private ObservableCollection<Conference> _conferences;
        private string _searchText;
        private string _statusFilter;

        public ConferencesView(IConferenceService conferenceService)
        {
            InitializeComponent();
            _conferenceService = conferenceService;
            _conferences = new ObservableCollection<Conference>();
            ConferencesList.ItemsSource = _conferences;
            LoadConferences();
        }

        private async void LoadConferences()
        {
            var conferences = await _conferenceService.GetConferencesAsync();
            _conferences.Clear();
            foreach (var conference in conferences)
            {
                _conferences.Add(conference);
            }
        }

        private async void AddConference_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ConferenceDialog();
            if (dialog.ShowDialog() == true)
            {
                var conference = dialog.Conference;
                await _conferenceService.AddConferenceAsync(conference);
                _conferences.Add(conference);
            }
        }

        private async void EditConference_Click(object sender, RoutedEventArgs e)
        {
            var conference = ConferencesList.SelectedItem as Conference;
            if (conference != null)
            {
                var dialog = new ConferenceDialog(conference);
                if (dialog.ShowDialog() == true)
                {
                    await _conferenceService.UpdateConferenceAsync(conference);
                    var index = _conferences.IndexOf(conference);
                    _conferences[index] = conference;
                }
            }
        }

        private async void DeleteConference_Click(object sender, RoutedEventArgs e)
        {
            var conference = ConferencesList.SelectedItem as Conference;
            if (conference != null)
            {
                var result = MessageBox.Show(
                    "Êtes-vous sûr de vouloir supprimer cette conférence ?",
                    "Confirmation",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    await _conferenceService.DeleteConferenceAsync(conference.Id);
                    _conferences.Remove(conference);
                }
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _searchText = SearchBox.Text;
            ApplyFilters();
        }

        private void StatusFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _statusFilter = (StatusFilter.SelectedItem as ComboBoxItem)?.Content.ToString();
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            var filteredConferences = _conferenceService.GetConferencesAsync().Result;

            if (!string.IsNullOrWhiteSpace(_searchText))
            {
                filteredConferences = filteredConferences.Where(c =>
                    c.Title.Contains(_searchText, StringComparison.OrdinalIgnoreCase) ||
                    c.Description.Contains(_searchText, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(_statusFilter))
            {
                filteredConferences = filteredConferences.Where(c => c.Status == _statusFilter);
            }

            _conferences.Clear();
            foreach (var conference in filteredConferences)
            {
                _conferences.Add(conference);
            }
        }
    }
} 