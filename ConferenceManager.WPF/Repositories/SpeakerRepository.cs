using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ConferenceManager.WPF.Models;
using ConferenceManager.WPF.Data;

namespace ConferenceManager.WPF.Repositories
{
    public class SpeakerRepository : Repository<Speaker>, ISpeakerRepository
    {
        public SpeakerRepository(IDbContextFactory<ApplicationDbContext> contextFactory) 
            : base(contextFactory)
        {
        }

        public override async Task<IEnumerable<Speaker>> GetAllAsync()
        {
            using var context = await CreateContextAsync();
            return await context.Speakers
                .Include(s => s.Conferences)
                .ToListAsync();
        }

        public override async Task<Speaker> GetByIdAsync(int id)
        {
            using var context = await CreateContextAsync();
            return await context.Speakers
                .Include(s => s.Conferences)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public override async Task<Speaker> AddAsync(Speaker speaker)
        {
            using var context = await CreateContextAsync();
            await context.Speakers.AddAsync(speaker);
            await context.SaveChangesAsync();
            return speaker;
        }

        public override async Task<Speaker> UpdateAsync(Speaker speaker)
        {
            using var context = await CreateContextAsync();
            context.Speakers.Update(speaker);
            await context.SaveChangesAsync();
            return speaker;
        }

        public override async Task DeleteAsync(int id)
        {
            using var context = await CreateContextAsync();
            var speaker = await context.Speakers.FindAsync(id);
            if (speaker != null)
            {
                context.Speakers.Remove(speaker);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Speaker>> GetByConferenceIdAsync(int conferenceId)
        {
            using var context = await CreateContextAsync();
            var conference = await context.Conferences
                .Include(c => c.Speakers)
                .FirstOrDefaultAsync(c => c.Id == conferenceId);

            return conference?.Speakers ?? Enumerable.Empty<Speaker>();
        }

        public async Task<IEnumerable<Speaker>> GetByExpertiseAsync(string expertise)
        {
            using var context = await CreateContextAsync();
            return await context.Speakers
                .Where(s => s.Expertise.Contains(expertise))
                .ToListAsync();
        }

        public async Task<IEnumerable<Conference>> GetConferencesAsync(int speakerId)
        {
            using var context = await CreateContextAsync();
            return await context.Conferences
                .Include(c => c.Speakers)
                .Where(c => c.Speakers.Any(s => s.Id == speakerId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Document>> GetDocumentsAsync(int speakerId)
        {
            using var context = await CreateContextAsync();
            return await context.Documents
                .Where(d => d.SpeakerId == speakerId)
                .ToListAsync();
        }
    }
} 