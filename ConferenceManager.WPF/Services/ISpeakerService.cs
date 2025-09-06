using System.Collections.Generic;
using System.Threading.Tasks;
using ConferenceManager.WPF.Models;

namespace ConferenceManager.WPF.Services
{
    public interface ISpeakerService
    {
        Task<IEnumerable<Speaker>> GetAllSpeakersAsync();
        Task<Speaker?> GetSpeakerByIdAsync(int id);
        Task<Speaker> CreateSpeakerAsync(Speaker speaker);
        Task<Speaker> UpdateSpeakerAsync(Speaker speaker);
        Task DeleteSpeakerAsync(int id);
        Task<IEnumerable<Speaker>> GetSpeakersByConferenceAsync(int conferenceId);
        Task<IEnumerable<Speaker>> GetSpeakersByExpertiseAsync(string expertise);
        Task<IEnumerable<Conference>> GetConferencesBySpeakerAsync(int speakerId);
        Task<IEnumerable<Document>> GetDocumentsBySpeakerAsync(int speakerId);
        Task<IEnumerable<Speaker>> GetSpeakersAsync();
        Task<IEnumerable<Speaker>> SearchSpeakersAsync(string searchTerm);
        Task ExportSpeakersAsync(string filePath);
        Task ImportSpeakersAsync(string filePath);
    }
} 