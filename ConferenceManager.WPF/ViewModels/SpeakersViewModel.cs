using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ConferenceManager.WPF.Models;
using ConferenceManager.WPF.Services;
using ConferenceManager.WPF.Views;
using Microsoft.Win32;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ConferenceManager.WPF.ViewModels
{
    public partial class SpeakersViewModel : ObservableObject
    {
        private readonly ISpeakerService _speakerService;
        private readonly IConferenceService _conferenceService;
        private ObservableCollection<Speaker> _speakers = new ObservableCollection<Speaker>();
        private Speaker _selectedSpeaker = new Speaker();
        private string _searchText = string.Empty;
        private bool _isLoading;
        private string _errorMessage = string.Empty;
        private string _statusFilter = "All";

        [ObservableProperty]
        private ObservableCollection<Speaker> speakers = new();

        [ObservableProperty]
        private Speaker? selectedSpeaker;

        [ObservableProperty]
        private string searchText = string.Empty;

        public SpeakersViewModel(ISpeakerService speakerService, IConferenceService conferenceService)
        {
            _speakerService = speakerService;
            _conferenceService = conferenceService;
            _ = LoadSpeakersAsync();
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        public string StatusFilter
        {
            get => _statusFilter;
            set
            {
                _statusFilter = value;
                OnPropertyChanged();
            }
        }

        public async Task LoadSpeakersAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = null;
                var speakers = await _speakerService.GetAllSpeakersAsync();
                Speakers.Clear();
                foreach (var speaker in speakers)
                {
                    Speakers.Add(speaker);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement des intervenants : {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        public async Task AddSpeakerAsync()
        {
            var viewModel = new SpeakerDialogViewModel(_speakerService);
            var dialog = new SpeakerDialog(viewModel);

            if (dialog.ShowDialog() == true)
            {
                await LoadSpeakersAsync();
            }
        }

        [RelayCommand]
        public async Task EditSpeakerAsync()
        {
            if (SelectedSpeaker == null)
            {
                MessageBox.Show("Veuillez sélectionner un intervenant à modifier.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var viewModel = new SpeakerDialogViewModel(_speakerService, SelectedSpeaker);
            var dialog = new SpeakerDialog(viewModel);

            if (dialog.ShowDialog() == true)
            {
                await LoadSpeakersAsync();
            }
        }

        [RelayCommand]
        public async Task DeleteSpeakerAsync()
        {
            if (SelectedSpeaker == null)
            {
                MessageBox.Show("Veuillez sélectionner un intervenant à supprimer.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = MessageBox.Show(
                "Êtes-vous sûr de vouloir supprimer cet intervenant ?",
                "Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    await _speakerService.DeleteSpeakerAsync(SelectedSpeaker.Id);
                    await LoadSpeakersAsync();
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Erreur lors de la suppression : {ex.Message}";
                }
            }
        }

        [RelayCommand]
        public async Task SearchAsync()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                await LoadSpeakersAsync();
                return;
            }

            try
            {
                var speakers = await _speakerService.GetAllSpeakersAsync();
                var filteredSpeakers = speakers.Where(s =>
                    s.FirstName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    s.LastName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    s.Email.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    s.Expertise.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

                Speakers.Clear();
                foreach (var speaker in filteredSpeakers)
                {
                    Speakers.Add(speaker);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de la recherche : {ex.Message}";
            }
        }

        public async Task ExportAsync()
        {
            try
            {
                var dialog = new SaveFileDialog
                {
                    Filter = "Excel Files|*.xlsx|CSV Files|*.csv",
                    DefaultExt = "xlsx"
                };

                if (dialog.ShowDialog() == true)
                {
                    await _speakerService.ExportSpeakersAsync(dialog.FileName);
                    MessageBox.Show("Export completed successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error exporting speakers: {ex.Message}";
            }
        }

        public async Task ImportAsync()
        {
            try
            {
                var dialog = new OpenFileDialog
                {
                    Filter = "Excel Files|*.xlsx|CSV Files|*.csv",
                    DefaultExt = "xlsx"
                };

                if (dialog.ShowDialog() == true)
                {
                    await _speakerService.ImportSpeakersAsync(dialog.FileName);
                    await LoadSpeakersAsync();
                    MessageBox.Show("Import completed successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error importing speakers: {ex.Message}";
            }
        }
    }
} 