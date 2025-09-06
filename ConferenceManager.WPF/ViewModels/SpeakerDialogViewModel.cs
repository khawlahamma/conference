using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ConferenceManager.WPF.Models;
using ConferenceManager.WPF.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ConferenceManager.WPF.ViewModels
{
    public partial class SpeakerDialogViewModel : ObservableObject
    {
        private readonly ISpeakerService _speakerService;
        private readonly Speaker _speaker;

        [ObservableProperty]
        private string _firstName = string.Empty;

        [ObservableProperty]
        private string _lastName = string.Empty;

        [ObservableProperty]
        private string _email = string.Empty;

        [ObservableProperty]
        private string _phone = string.Empty;

        [ObservableProperty]
        private string _bio = string.Empty;

        [ObservableProperty]
        private string _expertise = string.Empty;

        [ObservableProperty]
        private bool _isEditMode;

        [ObservableProperty]
        private bool _isSaving;

        public Speaker Speaker => _speaker;

        public SpeakerDialogViewModel(ISpeakerService speakerService, Speaker? speaker = null)
        {
            _speakerService = speakerService;
            _speaker = speaker ?? new Speaker();
            IsEditMode = speaker != null;

            if (IsEditMode)
            {
                FirstName = _speaker.FirstName;
                LastName = _speaker.LastName;
                Email = _speaker.Email;
                Phone = _speaker.Phone;
                Bio = _speaker.Bio;
                Expertise = _speaker.Expertise;
            }
        }

        [RelayCommand]
        public async Task SaveAsync()
        {
            if (IsSaving) return;
            IsSaving = true;
            try
            {
                _speaker.FirstName = FirstName;
                _speaker.LastName = LastName;
                _speaker.Email = Email;
                _speaker.Phone = Phone;
                _speaker.Bio = Bio;
                _speaker.Expertise = Expertise;

                if (IsEditMode)
                {
                    await _speakerService.UpdateSpeakerAsync(_speaker);
                }
                else
                {
                    await _speakerService.CreateSpeakerAsync(_speaker);
                }

                CloseDialog(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'enregistrement : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsSaving = false;
            }
        }

        [RelayCommand]
        public void Cancel()
        {
            CloseDialog(false);
        }

        private void CloseDialog(bool result)
        {
            if (Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive) is Window window)
            {
                window.DialogResult = result;
                window.Close();
            }
        }
    }
} 