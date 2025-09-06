using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ConferenceManager.WPF.Commands;
using ConferenceManager.WPF.Models;
using System.Collections.ObjectModel;
using ConferenceManager.WPF.Services;

namespace ConferenceManager.WPF.ViewModels
{
    public class DocumentDialogViewModel : INotifyPropertyChanged
    {
        private readonly Document _document;
        private string _title = string.Empty;
        private string _description = string.Empty;
        private DateTime _date = DateTime.Now;
        private string _status = string.Empty;
        private bool _isPublic;
        private string _type = string.Empty;
        private Conference? _conference;
        private ObservableCollection<Conference> _conferences = new();
        private readonly IConferenceService? _conferenceService;

        public event EventHandler<bool>? RequestClose;
        public event PropertyChangedEventHandler? PropertyChanged;

        public DocumentDialogViewModel(Document? document = null, IConferenceService? conferenceService = null)
        {
            _document = new Document();
            _document.Id = 0;
            _title = _document.Title ?? string.Empty;
            _description = _document.Description ?? string.Empty;
            _date = _document.Date;
            _status = _document.Status ?? string.Empty;
            _isPublic = _document.IsPublic;
            _type = _document.Type ?? string.Empty;
            _conference = _document.Conference;
            _conferenceService = conferenceService;
            SaveCommand = new RelayCommand(_ => Save());
            CancelCommand = new RelayCommand(_ => Cancel());
            if (_conferenceService != null)
            {
                _ = LoadConferencesAsync();
            }
        }

        public Document Document => _document;
        public string DialogTitle => _document.Id == 0 ? "Add Document" : "Edit Document";

        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime Date
        {
            get => _date;
            set
            {
                if (_date != value)
                {
                    _date = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Status
        {
            get => _status;
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsPublic
        {
            get => _isPublic;
            set
            {
                if (_isPublic != value)
                {
                    _isPublic = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Type
        {
            get => _type;
            set
            {
                if (_type != value)
                {
                    _type = value;
                    OnPropertyChanged();
                }
            }
        }

        public Conference? Conference
        {
            get => _conference;
            set
            {
                if (_conference != value)
                {
                    _conference = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<Conference> Conferences
        {
            get => _conferences;
            set
            {
                if (_conferences != value)
                {
                    _conferences = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        private void Save()
        {
            if (Conference == null)
            {
                System.Windows.MessageBox.Show("Veuillez sélectionner une conférence.", "Erreur", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }
            _document.Title = Title;
            _document.Description = Description;
            _document.Date = Date;
            _document.Status = Status;
            _document.IsPublic = IsPublic;
            _document.Type = Type;
            _document.Conference = Conference;
            _document.LastModified = DateTime.Now;
            _document.ConferenceId = Conference.Id;
            RequestClose?.Invoke(this, true);
        }

        private void Cancel()
        {
            RequestClose?.Invoke(this, false);
        }

        public async Task LoadConferencesAsync()
        {
            if (_conferenceService == null) return;
            var conferences = await _conferenceService.GetConferencesAsync();
            foreach (var conf in conferences)
            {
                System.Diagnostics.Debug.WriteLine($"Conf loaded: {conf.Id} - {conf.Title}");
            }
            Conferences = new ObservableCollection<Conference>(conferences);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
} 