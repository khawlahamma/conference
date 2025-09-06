using System.Collections.Generic;
using System.Threading.Tasks;
using ConferenceManager.WPF.Models;

namespace ConferenceManager.WPF.Services
{
    public interface IExportService
    {
        Task ExportConferencesAsync(string filePath, IEnumerable<Conference> conferences);
        Task ExportSpeakersAsync(string filePath, IEnumerable<Speaker> speakers);
        Task ExportDocumentsAsync(string filePath, IEnumerable<Document> documents);
        Task<string> GetExportDirectoryAsync();
    }
} 