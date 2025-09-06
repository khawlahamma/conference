using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ConferenceManager.WPF.Models;
using ConferenceManager.WPF.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using ConferenceManager.WPF.Views;
using Microsoft.Extensions.DependencyInjection;

namespace ConferenceManager.WPF.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly IConferenceService _conferenceService;
        private readonly ISpeakerService _speakerService;
        private readonly IDocumentService _documentService;

        [ObservableProperty]
        private int _conferenceCount;

        [ObservableProperty]
        private int _speakerCount;

        [ObservableProperty]
        private ObservableCollection<ActivityItem> _recentActivities = new();

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private string _errorMessage = string.Empty;

        [ObservableProperty]
        private object _currentView;

        public MainViewModel(
            IConferenceService conferenceService,
            ISpeakerService speakerService,
            IDocumentService documentService)
        {
            _conferenceService = conferenceService ?? throw new ArgumentNullException(nameof(conferenceService));
            _speakerService = speakerService ?? throw new ArgumentNullException(nameof(speakerService));
            _documentService = documentService ?? throw new ArgumentNullException(nameof(documentService));

            CurrentView = new DashboardView(this);
            _ = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;
                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de l'initialisation : {ex.Message}";
                Debug.WriteLine($"Error in InitializeAsync: {ex}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task LoadDataAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;

                // Appels séquentiels pour éviter la réutilisation concurrente de la connexion
                var conferences = await _conferenceService.GetConferencesAsync();
                var speakers = await _speakerService.GetAllSpeakersAsync();
                var documents = await _documentService.GetDocumentsAsync();

                ConferenceCount = conferences.Count();
                SpeakerCount = speakers.Count();

                RecentActivities.Clear();
                foreach (var conference in conferences.OrderByDescending(c => c.Date).Take(5))
                {
                    RecentActivities.Add(new ActivityItem
                    {
                        Type = "Conférence",
                        Description = conference.Title,
                        Date = conference.Date
                    });
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement des données : {ex.Message}";
                Debug.WriteLine($"Error in LoadDataAsync: {ex}");
                throw;
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private void NavigateToHome()
        {
            CurrentView = new DashboardView(this);
        }

        [RelayCommand]
        private void NavigateToSearch()
        {
            var vm = new AdvancedSearchViewModel(_conferenceService, _speakerService, _documentService);
            CurrentView = vm;
        }

        [RelayCommand]
        private async void NavigateToDocuments()
        {
            var vm = new DocumentsViewModel(_documentService, _conferenceService, _speakerService);
            await vm.LoadDataAsync();
            CurrentView = vm;
        }

        [RelayCommand]
        private void NavigateToSpeakers()
        {
            var vm = new SpeakersViewModel(_speakerService, _conferenceService);
            CurrentView = vm;
        }

        [RelayCommand]
        private void NavigateToProfile()
        {
            var app = (App)System.Windows.Application.Current;
            var vm = new ProfileViewModel(
                app.Services.GetRequiredService<IUserService>(),
                _conferenceService,
                _speakerService,
                _documentService
            );
            CurrentView = vm;
        }

        [RelayCommand]
        private void Exit()
        {
            Application.Current.Shutdown();
        }

        [RelayCommand]
        private async Task CreateNewConferenceAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;
                var dialog = new ConferenceManager.WPF.Views.ConferenceDialog();
                if (dialog.ShowDialog() == true)
                {
                    var conference = dialog.Conference;
                    await _conferenceService.AddConferenceAsync(conference);
                    await LoadDataAsync();
                }
            }
            catch (Exception ex)
            {
                var inner = ex.InnerException?.Message ?? string.Empty;
                ErrorMessage = $"Erreur lors de la création de la conférence : {ex.Message} {(string.IsNullOrEmpty(inner) ? "" : "Détail : " + inner)}";
                Debug.WriteLine($"Error in CreateNewConferenceAsync: {ex}\nInner: {ex.InnerException}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task CreateNewSpeakerAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;
                var viewModel = new ConferenceManager.WPF.ViewModels.SpeakerDialogViewModel(_speakerService);
                var dialog = new ConferenceManager.WPF.Views.SpeakerDialog(viewModel);
                if (dialog.ShowDialog() == true)
                {
                    // Crée un nouvel objet Speaker à partir des propriétés du ViewModel
                    var speaker = new Speaker
                    {
                        FirstName = viewModel.FirstName,
                        LastName = viewModel.LastName,
                        Email = viewModel.Email,
                        Phone = viewModel.Phone,
                        Bio = viewModel.Bio,
                        Expertise = viewModel.Expertise
                        // Ajoute les autres propriétés nécessaires ici
                    };
                    await _speakerService.CreateSpeakerAsync(speaker);
                    await LoadDataAsync();
                }
            }
            catch (Exception ex)
            {
                var inner = ex.InnerException?.Message ?? string.Empty;
                ErrorMessage = $"Erreur lors de la création de l'intervenant : {ex.Message} {(string.IsNullOrEmpty(inner) ? "" : "Détail : " + inner)}";
                Debug.WriteLine($"Error in CreateNewSpeakerAsync: {ex}\nInner: {ex.InnerException}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task CreateNewDocumentAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;
                var dialog = new ConferenceManager.WPF.Views.DocumentDialog(_documentService, _conferenceService);
                if (dialog.ShowDialog() == true)
                {
                    var document = dialog.Document;
                    document.Id = 0;
                    Debug.WriteLine($"Document avant ajout : Id={document.Id}, Title={document.Title}, ConferenceId={document.ConferenceId}, FilePath={document.FilePath}, Type={document.Type}, Status={document.Status}, Date={document.Date}, IsPublic={document.IsPublic}, FileName={document.FileName}");
                    if (document.ConferenceId == 0)
                    {
                        MessageBox.Show("Aucune conférence valide sélectionnée. Veuillez sélectionner une conférence existante.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    var result = await _documentService.AddDocumentAsync(document);
                    Debug.WriteLine($"Document ajouté avec Id={result.Id}");
                    await LoadDataAsync();
                }
            }
            catch (Exception ex)
            {
                var inner = ex.InnerException?.Message ?? string.Empty;
                ErrorMessage = $"Erreur lors de la création du document : {ex.Message} {(string.IsNullOrEmpty(inner) ? "" : "Détail : " + inner)}";
                Debug.WriteLine($"Error in CreateNewDocumentAsync: {ex}\nInner: {ex.InnerException}");
                MessageBox.Show($"Erreur lors de la création du document : {ex.Message}\n\nStack trace:\n{ex.StackTrace}", "Erreur critique", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }
    }

    public class ActivityItem
    {
        public string Type { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
    }
} 