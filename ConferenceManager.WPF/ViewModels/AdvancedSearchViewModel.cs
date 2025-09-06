using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ConferenceManager.WPF.Models;
using ConferenceManager.WPF.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ConferenceManager.WPF.ViewModels
{
    public partial class AdvancedSearchViewModel : ObservableObject
    {
        private readonly IConferenceService _conferenceService;
        private readonly ISpeakerService _speakerService;
        private readonly IDocumentService _documentService;

        [ObservableProperty]
        private string _searchText = string.Empty;

        [ObservableProperty]
        private string _location = string.Empty;

        [ObservableProperty]
        private string _speakerName = string.Empty;

        [ObservableProperty]
        private string _documentType = string.Empty;

        [ObservableProperty]
        private string _searchType = "All";

        [ObservableProperty]
        private object? _selectedItem;

        [ObservableProperty]
        private ObservableCollection<object> _searchResults = new();

        [ObservableProperty]
        private string _errorMessage = string.Empty;

        [ObservableProperty]
        private DateTime? _startDate;

        [ObservableProperty]
        private DateTime? _endDate;

        public AdvancedSearchViewModel(
            IConferenceService conferenceService,
            ISpeakerService speakerService,
            IDocumentService documentService)
        {
            _conferenceService = conferenceService;
            _speakerService = speakerService;
            _documentService = documentService;
        }

        [RelayCommand]
        private async Task SearchAsync()
        {
            try
            {
                SearchResults.Clear();
                ErrorMessage = string.Empty;

                if (string.IsNullOrWhiteSpace(SearchText))
                {
                    ErrorMessage = "Please enter a search term";
                    return;
                }

                var searchType = SearchType.ToLower();
                if (searchType == "all" || searchType == "conferences")
                {
                    var conferences = await _conferenceService.SearchConferencesAsync(
                        SearchText,
                        Location,
                        StartDate,
                        EndDate);

                    foreach (var conference in conferences)
                    {
                        SearchResults.Add(conference);
                    }
                }

                if (searchType == "all" || searchType == "speakers")
                {
                    var speakers = await _speakerService.SearchSpeakersAsync(SearchText);
                    foreach (var speaker in speakers)
                    {
                        SearchResults.Add(speaker);
                    }
                }

                if (searchType == "all" || searchType == "documents")
                {
                    var documents = await _documentService.SearchDocumentsAsync(
                        SearchText,
                        DocumentType,
                        string.Empty);

                    foreach (var document in documents)
                    {
                        SearchResults.Add(document);
                    }
                }

                if (!SearchResults.Any())
                {
                    ErrorMessage = "No results found";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error performing search: {ex.Message}";
            }
        }

        [RelayCommand]
        private void ClearSearch()
        {
            SearchText = string.Empty;
            Location = string.Empty;
            SpeakerName = string.Empty;
            DocumentType = string.Empty;
            StartDate = null;
            EndDate = null;
            SearchResults.Clear();
            ErrorMessage = string.Empty;
        }

        public async Task LoadDataAsync()
        {
            // À compléter si besoin
            await Task.CompletedTask;
        }
    }
} 