using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ConferenceManager.WPF.Models;
using ConferenceManager.WPF.Data;

namespace ConferenceManager.WPF.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(IDbContextFactory<ApplicationDbContext> contextFactory) 
            : base(contextFactory)
        {
        }

        public override async Task<IEnumerable<User>> GetAllAsync()
        {
            using var context = await CreateContextAsync();
            return await context.Users.ToListAsync();
        }

        public override async Task<User> GetByIdAsync(int id)
        {
            using var context = await CreateContextAsync();
            return await context.Users.FindAsync(id);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            using var context = await CreateContextAsync();
            return await context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public override async Task<User> AddAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            using var context = await CreateContextAsync();
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            return user;
        }

        public override async Task<User> UpdateAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            using var context = await CreateContextAsync();
            context.Users.Update(user);
            await context.SaveChangesAsync();
            return user;
        }

        public override async Task DeleteAsync(int id)
        {
            using var context = await CreateContextAsync();
            var user = await context.Users.FindAsync(id);
            if (user != null)
            {
                context.Users.Remove(user);
                await context.SaveChangesAsync();
            }
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            using var context = await CreateContextAsync();
            return await context.Users
                .FirstOrDefaultAsync(u => u.Username == username);
        }
    }
} 