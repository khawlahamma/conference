using System.Collections.Generic;
using System.Threading.Tasks;
using ConferenceManager.WPF.Models;

namespace ConferenceManager.WPF.Repositories
{
    public interface IConferenceRepository : IRepository<Conference>
    {
        Task<IEnumerable<Speaker>> GetSpeakersAsync(int conferenceId);
        Task<IEnumerable<Document>> GetDocumentsAsync(int conferenceId);
        Task<IEnumerable<Conference>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Conference>> GetUpcomingAsync();
        Task<IEnumerable<Conference>> GetByLocationAsync(string location);
    }
} 