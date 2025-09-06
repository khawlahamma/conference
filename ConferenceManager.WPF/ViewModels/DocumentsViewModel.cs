using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ConferenceManager.WPF.Models;
using ConferenceManager.WPF.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System.IO;
using ConferenceManager.WPF.Views;

namespace ConferenceManager.WPF.ViewModels
{
    public partial class DocumentsViewModel : ObservableObject
    {
        private readonly IDocumentService _documentService;
        private readonly IConferenceService _conferenceService;
        private readonly ISpeakerService _speakerService;

        [ObservableProperty]
        private ObservableCollection<Document> _documents = new();

        [ObservableProperty]
        private Document? _selectedDocument;

        [ObservableProperty]
        private string _searchText = string.Empty;

        [ObservableProperty]
        private string _selectedType = "All";

        [ObservableProperty]
        private string _selectedStatus = "All";

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private string _errorMessage = string.Empty;

        [ObservableProperty]
        private int? _selectedConferenceId;

        [ObservableProperty]
        private string _selectedDocumentType = string.Empty;

        [ObservableProperty]
        private Conference? _selectedConference;

        [ObservableProperty]
        private ObservableCollection<Conference> _conferences = new();

        public DocumentsViewModel(
            IDocumentService documentService,
            IConferenceService conferenceService,
            ISpeakerService speakerService)
        {
            _documentService = documentService;
            _conferenceService = conferenceService;
            _speakerService = speakerService;

            _ = LoadDataAsync();
        }

        public async Task LoadDataAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;

                // Load conferences
                var conferences = await _conferenceService.GetConferencesAsync();
                Conferences.Clear();
                foreach (var conference in conferences)
                {
                    Conferences.Add(conference);
                }

                // Load documents
                var documents = await _documentService.GetDocumentsAsync();
                Documents.Clear();
                foreach (var document in documents)
                {
                    Documents.Add(document);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement des données : {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        public async Task AddDocumentAsync()
        {
            try
            {
                var dialog = new DocumentDialog(_documentService, _conferenceService);
                if (dialog.ShowDialog() == true)
                {
                    var document = dialog.Document;
                    await _documentService.AddDocumentAsync(document);
                    Documents.Add(document);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de l'ajout du document : {ex.Message}";
            }
        }

        [RelayCommand]
        public async Task UpdateDocumentAsync()
        {
            if (SelectedDocument == null) return;

            try
            {
                var dialog = new DocumentDialog(_documentService, _conferenceService, SelectedDocument);
                if (dialog.ShowDialog() == true)
                {
                    var updatedDocument = dialog.Document;
                    await _documentService.UpdateDocumentAsync(updatedDocument);
                    var index = Documents.IndexOf(SelectedDocument);
                    Documents[index] = updatedDocument;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de la mise à jour du document : {ex.Message}";
            }
        }

        [RelayCommand]
        public async Task DeleteDocumentAsync()
        {
            if (SelectedDocument == null) return;

            try
            {
                var result = MessageBox.Show(
                    "Êtes-vous sûr de vouloir supprimer ce document ?",
                    "Confirmation",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    await _documentService.DeleteDocumentAsync(SelectedDocument.Id);
                    Documents.Remove(SelectedDocument);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de la suppression du document : {ex.Message}";
            }
        }

        [RelayCommand]
        public async Task ExportAsync()
        {
            try
            {
                var dialog = new SaveFileDialog
                {
                    Filter = "Excel Files|*.xlsx",
                    Title = "Exporter les documents"
                };

                if (dialog.ShowDialog() == true)
                {
                    await _documentService.ExportToExcelAsync(dialog.FileName);
                    MessageBox.Show("Export réussi !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de l'export : {ex.Message}";
            }
        }

        [RelayCommand]
        public async Task ImportAsync()
        {
            try
            {
                var dialog = new OpenFileDialog
                {
                    Filter = "Excel Files|*.xlsx",
                    Title = "Importer des documents"
                };

                if (dialog.ShowDialog() == true)
                {
                    await _documentService.ImportFromExcelAsync(dialog.FileName);
                    await LoadDataAsync();
                    MessageBox.Show("Import réussi !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de l'import : {ex.Message}";
            }
        }

        [RelayCommand]
        private async Task SearchAsync()
        {
            await LoadDataAsync();
        }

        [RelayCommand]
        private async Task ClearSearchAsync()
        {
            SearchText = string.Empty;
            SelectedType = "All";
            SelectedStatus = "All";
            await LoadDataAsync();
        }

        [RelayCommand]
        public async Task ViewDocumentAsync()
        {
            if (SelectedDocument == null) return;

            try
            {
                MessageBox.Show("Fonctionnalité de visualisation à implémenter", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de la visualisation du document : {ex.Message}";
            }
        }

        [RelayCommand]
        public async Task DownloadDocumentAsync(Document document)
        {
            if (document == null) return;

            try
            {
                var fileContent = await _documentService.DownloadDocumentAsync(document.Id);
                var dialog = new SaveFileDialog
                {
                    FileName = document.FileName,
                    Filter = "Tous les fichiers|*.*"
                };

                if (dialog.ShowDialog() == true)
                {
                    await File.WriteAllBytesAsync(dialog.FileName, fileContent);
                    MessageBox.Show("Téléchargement réussi !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du téléchargement du document : {ex.Message}";
            }
        }

        [RelayCommand]
        public async Task ExportDocumentsAsync()
        {
            await ExportAsync();
        }

        // Explicitly implement INotifyPropertyChanged to avoid conflicts
        new public event PropertyChangedEventHandler? PropertyChanged;

        // Override OnPropertyChanged to use the new event
        new protected virtual void OnPropertyChanged(string? propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Override SetProperty to use the new event
        new protected bool SetProperty<T>(ref T field, T value, string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
} 