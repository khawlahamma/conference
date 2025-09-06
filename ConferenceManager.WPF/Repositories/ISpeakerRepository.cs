using System.Collections.Generic;
using System.Threading.Tasks;
using ConferenceManager.WPF.Models;

namespace ConferenceManager.WPF.Repositories
{
    public interface ISpeakerRepository : IRepository<Speaker>
    {
        Task<IEnumerable<Speaker>> GetByConferenceIdAsync(int conferenceId);
        Task<IEnumerable<Speaker>> GetByExpertiseAsync(string expertise);
        Task<IEnumerable<Conference>> GetConferencesAsync(int speakerId);
        Task<IEnumerable<Document>> GetDocumentsAsync(int speakerId);
    }
} 