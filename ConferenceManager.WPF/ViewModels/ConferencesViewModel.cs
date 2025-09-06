using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ConferenceManager.WPF.Models;
using ConferenceManager.WPF.Services;

namespace ConferenceManager.WPF.ViewModels
{
    public class ConferencesViewModel : ViewModelBase
    {
        private readonly IConferenceService _conferenceService;
        private ObservableCollection<Conference> _conferences;

        public ConferencesViewModel(IConferenceService conferenceService)
        {
            _conferenceService = conferenceService;
            _conferences = new ObservableCollection<Conference>();
            LoadConferencesAsync();
        }

        public ObservableCollection<Conference> Conferences
        {
            get => _conferences;
            set
            {
                _conferences = value;
                OnPropertyChanged();
            }
        }

        private async void LoadConferencesAsync()
        {
            var conferences = await _conferenceService.GetConferencesAsync();
            Conferences.Clear();
            foreach (var conference in conferences)
            {
                Conferences.Add(conference);
            }
        }
    }
} 