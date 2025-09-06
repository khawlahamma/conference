using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConferenceManager.WPF.Models;

namespace ConferenceManager.WPF.Repositories
{
    public interface IDocumentRepository : IRepository<Document>
    {
        Task<IEnumerable<Document>> GetByConferenceIdAsync(int conferenceId);
        Task<IEnumerable<Document>> GetBySpeakerIdAsync(int speakerId);
        Task<IEnumerable<Document>> GetPublicDocumentsAsync();
        Task<IEnumerable<Document>> GetByTypeAsync(string type);
        Task<IEnumerable<Document>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
} 