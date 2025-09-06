using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ConferenceManager.WPF.Models;
using ConferenceManager.WPF.Services;

namespace ConferenceManager.WPF.ViewModels
{
    public class ExportViewModel : ViewModelBase
    {
        public IConferenceService ConferenceService { get; }
        public ISpeakerService SpeakerService { get; }
        public IDocumentService DocumentService { get; }
        private readonly IExportService _exportService;

        public ExportViewModel(
            IConferenceService conferenceService,
            ISpeakerService speakerService,
            IDocumentService documentService,
            IExportService exportService)
        {
            ConferenceService = conferenceService;
            SpeakerService = speakerService;
            DocumentService = documentService;
            _exportService = exportService;
        }

        public async Task ExportConferencesAsync(string filePath)
        {
            var conferences = await ConferenceService.GetConferencesAsync();
            await _exportService.ExportConferencesAsync(filePath, conferences);
        }

        public async Task ExportSpeakersAsync(string filePath)
        {
            var speakers = await SpeakerService.GetSpeakersAsync();
            await _exportService.ExportSpeakersAsync(filePath, speakers);
        }

        public async Task ExportDocumentsAsync(string filePath)
        {
            var documents = await DocumentService.GetDocumentsAsync();
            await _exportService.ExportDocumentsAsync(filePath, documents);
        }
    }
} 