using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConferenceManager.WPF.Models;

namespace ConferenceManager.WPF.Services
{
    public interface IConferenceService
    {
        Task<IEnumerable<Conference>> GetConferencesAsync();
        Task<Conference> GetConferenceByIdAsync(int id);
        Task<Conference> AddConferenceAsync(Conference conference);
        Task<Conference> UpdateConferenceAsync(Conference conference);
        Task DeleteConferenceAsync(int id);
        Task<IEnumerable<Conference>> GetConferencesBySpeakerIdAsync(int speakerId);
        Task<IEnumerable<Speaker>> GetConferenceSpeakersAsync(int conferenceId);
        Task<IEnumerable<Conference>> GetConferencesByDocumentIdAsync(int documentId);
        Task<IEnumerable<Document>> GetConferenceDocumentsAsync(int conferenceId);
        Task<IEnumerable<Conference>> GetConferencesByStatusAsync(string status);
        Task<IEnumerable<Conference>> GetConferencesByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Conference>> SearchConferencesAsync(string searchTerm, string location, DateTime? startDate, DateTime? endDate);
    }
} 