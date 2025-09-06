using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using ConferenceManager.WPF.Models;
using ConferenceManager.WPF.Services;
using Microsoft.Win32;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace ConferenceManager.WPF.ViewModels
{
    public partial class ProfileViewModel : ObservableObject
    {
        private readonly IUserService _userService;
        private readonly IConferenceService _conferenceService;
        private readonly ISpeakerService _speakerService;
        private readonly IDocumentService _documentService;
        private User _currentUser;

        public IUserService UserService => _userService;

        new public event PropertyChangedEventHandler? PropertyChanged;

        [ObservableProperty]
        private string _username = string.Empty;

        [ObservableProperty]
        private string _firstName = string.Empty;

        [ObservableProperty]
        private string _lastName = string.Empty;

        [ObservableProperty]
        private string _email = string.Empty;

        [ObservableProperty]
        private string _phone = string.Empty;

        [ObservableProperty]
        private bool _emailNotifications;

        [ObservableProperty]
        private bool _darkMode;

        [ObservableProperty]
        private string _language = string.Empty;

        [ObservableProperty]
        private BitmapImage? _profilePicture;

        [ObservableProperty]
        private string _oldPassword = string.Empty;

        [ObservableProperty]
        private string _newPassword = string.Empty;

        [ObservableProperty]
        private string _confirmPassword = string.Empty;

        [ObservableProperty]
        private string _errorMessage = string.Empty;

        [ObservableProperty]
        private ObservableCollection<Conference> _userConferences = new();

        [ObservableProperty]
        private ObservableCollection<Speaker> _userSpeakers = new();

        [ObservableProperty]
        private ObservableCollection<Document> _userDocuments = new();

        public ProfileViewModel(
            IUserService userService,
            IConferenceService conferenceService,
            ISpeakerService speakerService,
            IDocumentService documentService)
        {
            _userService = userService;
            _conferenceService = conferenceService;
            _speakerService = speakerService;
            _documentService = documentService;
            _currentUser = new User();
            _ = LoadUserProfileAsync();
        }

        public User? CurrentUser
        {
            get => _currentUser;
            set => SetProperty(ref _currentUser, value);
        }

        public async Task LoadUserProfileAsync()
        {
            try
            {
                // TODO: Get current user ID from authentication service
                var userId = 1; // Temporary hardcoded value
                CurrentUser = await _userService.GetUserByIdAsync(userId);
                if (CurrentUser != null)
                {
                    Username = CurrentUser.Username;
                    FirstName = CurrentUser.FirstName;
                    LastName = CurrentUser.LastName;
                    Email = CurrentUser.Email;
                    Phone = CurrentUser.Phone;
                    EmailNotifications = CurrentUser.EmailNotifications;
                    DarkMode = CurrentUser.DarkMode;
                    Language = CurrentUser.Language;

                    if (!string.IsNullOrEmpty(CurrentUser.ProfilePicture))
                    {
                        ProfilePicture = new BitmapImage(new Uri(CurrentUser.ProfilePicture));
                    }

                    await LoadUserContent();
                    ErrorMessage = string.Empty;
                }
                else
                {
                    ErrorMessage = "Aucun utilisateur trouvé. Veuillez créer un profil utilisateur.";
                    Username = string.Empty;
                    FirstName = string.Empty;
                    LastName = string.Empty;
                    Email = string.Empty;
                    Phone = string.Empty;
                    EmailNotifications = false;
                    DarkMode = false;
                    Language = string.Empty;
                    ProfilePicture = null;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement du profil : {ex.Message}";
            }
        }

        private async Task LoadUserContent()
        {
            try
            {
                var conferences = await _conferenceService.GetConferencesAsync();
                var speakers = await _speakerService.GetAllSpeakersAsync();
                var documents = await _documentService.GetDocumentsAsync();

                UserConferences.Clear();
                UserSpeakers.Clear();
                UserDocuments.Clear();

                foreach (var conference in conferences)
                {
                    UserConferences.Add(conference);
                }

                foreach (var speaker in speakers)
                {
                    UserSpeakers.Add(speaker);
                }

                foreach (var document in documents)
                {
                    UserDocuments.Add(document);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement des données : {ex.Message}";
            }
        }

        [RelayCommand]
        private async Task ChangeProfilePicture()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Images|*.jpg;*.jpeg;*.png;*.gif;*.bmp",
                Title = "Select Profile Picture"
            };

            if (dialog.ShowDialog() == true)
            {
                ProfilePicture = new BitmapImage(new Uri(dialog.FileName));
                await SaveProfileAsync();
            }
        }

        [RelayCommand]
        public async Task SaveProfileAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Email))
                {
                    ErrorMessage = "Le nom d'utilisateur et l'email sont requis.";
                    return;
                }

                var user = new User
                {
                    Username = Username,
                    FirstName = FirstName,
                    LastName = LastName,
                    Email = Email,
                    Phone = Phone,
                    Language = Language
                };

                if (!string.IsNullOrWhiteSpace(NewPassword))
                {
                    if (NewPassword != ConfirmPassword)
                    {
                        ErrorMessage = "Les mots de passe ne correspondent pas.";
                        return;
                    }

                    user.PasswordHash = NewPassword; // Note: Dans une vraie application, il faudrait hasher le mot de passe
                }

                await _userService.UpdateUserAsync(user);
                ErrorMessage = "Profil mis à jour avec succès.";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de la mise à jour du profil : {ex.Message}";
            }
        }

        public async Task UpdateProfilePicture(string filePath)
        {
            try
            {
                // Implémentation à venir
                ErrorMessage = "Fonctionnalité à venir : Mise à jour de la photo de profil";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de la mise à jour de la photo : {ex.Message}";
            }
        }

        [RelayCommand]
        public async Task ChangePasswordAsync()
        {
            if (CurrentUser == null)
            {
                throw new InvalidOperationException("User not logged in");
            }

            if (string.IsNullOrWhiteSpace(OldPassword))
            {
                throw new InvalidOperationException("Current password is required");
            }

            if (string.IsNullOrWhiteSpace(NewPassword))
            {
                throw new InvalidOperationException("New password is required");
            }

            if (NewPassword != ConfirmPassword)
            {
                throw new InvalidOperationException("Passwords do not match");
            }

            await _userService.ChangePasswordAsync(OldPassword, NewPassword);

            // Clear password fields
            OldPassword = string.Empty;
            NewPassword = string.Empty;
            ConfirmPassword = string.Empty;
        }

        new protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (!Equals(field, value))
            {
                field = value;
                OnPropertyChanged(propertyName);
            }
        }
    }
} 