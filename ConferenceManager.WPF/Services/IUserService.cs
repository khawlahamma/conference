using System.Threading.Tasks;
using ConferenceManager.WPF.Models;

namespace ConferenceManager.WPF.Services
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetCurrentUserAsync();
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByUsernameAsync(string username);
        Task<User> CreateUserAsync(User user, string password);
        Task<User> UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
        Task ChangePasswordAsync(string currentPassword, string newPassword);
        Task<bool> ValidateUserAsync(string username, string password);
    }
} 