using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ConferenceManager.WPF.Models;
using ConferenceManager.WPF.Data;
using ConferenceManager.WPF.Repositories;
using BCrypt.Net;

namespace ConferenceManager.WPF.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public UserService(
            IUserRepository userRepository,
            IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _userRepository = userRepository;
            _contextFactory = contextFactory;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new Exception($"User with ID {id} not found.");
            }
            return user;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                throw new Exception($"User with email {email} not found.");
            }
            return user;
        }

        public async Task<User> CreateUserAsync(User user, string password)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Password is required", nameof(password));
            }

            // Check if user with same email or username already exists
            var existingUser = await _userRepository.GetByEmailAsync(user.Email);
            if (existingUser != null)
            {
                throw new Exception($"User with email {user.Email} already exists.");
            }

            existingUser = await _userRepository.GetByUsernameAsync(user.Username);
            if (existingUser != null)
            {
                throw new Exception($"User with username {user.Username} already exists.");
            }

            // Hash the password
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
            user.CreatedAt = DateTime.UtcNow;

            // Set default values
            user.EmailNotifications = true;
            user.DarkMode = false;
            user.Language = "English";

            return await _userRepository.AddAsync(user);
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var existingUser = await _userRepository.GetByIdAsync(user.Id);
            if (existingUser == null)
            {
                throw new Exception($"User with ID {user.Id} not found.");
            }

            // Preserve password hash
            user.PasswordHash = existingUser.PasswordHash;

            return await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new Exception($"User with ID {id} not found.");
            }

            await _userRepository.DeleteAsync(id);
        }

        public async Task<bool> ValidateUserAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null)
            {
                return false;
            }

            return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        }

        public async Task<User> GetCurrentUserAsync()
        {
            // TODO: Implement actual current user retrieval logic
            // For now, return the first user in the database
            var users = await _userRepository.GetAllAsync();
            return users.FirstOrDefault() ?? throw new Exception("No users found in the database.");
        }

        public async Task UpdateUserProfileAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var existingUser = await _userRepository.GetByIdAsync(user.Id);
            if (existingUser == null)
            {
                throw new Exception($"User with ID {user.Id} not found.");
            }

            await _userRepository.UpdateAsync(user);
        }

        public async Task ChangePasswordAsync(string currentPassword, string newPassword)
        {
            var currentUser = await GetCurrentUserAsync();
            if (!BCrypt.Net.BCrypt.Verify(currentPassword, currentUser.PasswordHash))
            {
                throw new Exception("Current password is incorrect.");
            }

            currentUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await _userRepository.UpdateAsync(currentUser);
        }

        public async Task<bool> IsEmailAvailableAsync(string email)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return !await context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null)
            {
                throw new Exception($"User with username {username} not found.");
            }
            return user;
        }

        public async Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            if (!BCrypt.Net.BCrypt.Verify(oldPassword, user.PasswordHash))
            {
                return false;
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await _userRepository.UpdateAsync(user);
            return true;
        }

        public async Task<IEnumerable<User>> SearchUsersAsync(string searchTerm)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Users
                .Where(u => u.Username.Contains(searchTerm) || 
                           u.Email.Contains(searchTerm) ||
                           (u.FirstName + " " + u.LastName).Contains(searchTerm))
                .ToListAsync();
        }
    }
} 