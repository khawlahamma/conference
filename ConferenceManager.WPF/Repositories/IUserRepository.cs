using System.Threading.Tasks;
using ConferenceManager.WPF.Models;
using ConferenceManager.WPF.Data;

namespace ConferenceManager.WPF.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByUsernameAsync(string username);
    }
} 