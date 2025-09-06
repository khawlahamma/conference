using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ConferenceManager.WPF.Models;
using ConferenceManager.WPF.Data;

namespace ConferenceManager.WPF.Repositories
{
    public class ConferenceRepository : Repository<Conference>, IConferenceRepository
    {
        public ConferenceRepository(IDbContextFactory<ApplicationDbContext> contextFactory) 
            : base(contextFactory)
        {
        }

        public async Task<IEnumerable<Conference>> GetUpcomingAsync()
        {
            using var context = await CreateContextAsync();
            return await context.Conferences
                .Where(c => c.Date > DateTime.Now)
                .OrderBy(c => c.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Conference>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            using var context = await CreateContextAsync();
            return await context.Conferences
                .Where(c => c.Date >= startDate && c.Date <= endDate)
                .OrderBy(c => c.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Conference>> GetByLocationAsync(string location)
        {
            using var context = await CreateContextAsync();
            return await context.Conferences
                .Where(c => c.Location.Contains(location))
                .OrderBy(c => c.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Speaker>> GetSpeakersAsync(int conferenceId)
        {
            using var context = await CreateContextAsync();
            var conference = await context.Conferences
                .Include(c => c.Speakers)
                .FirstOrDefaultAsync(c => c.Id == conferenceId);

            return conference?.Speakers ?? Enumerable.Empty<Speaker>();
        }

        public async Task<IEnumerable<Document>> GetDocumentsAsync(int conferenceId)
        {
            using var context = await CreateContextAsync();
            return await context.Documents
                .Where(d => d.ConferenceId == conferenceId)
                .ToListAsync();
        }

        public override async Task<IEnumerable<Conference>> GetAllAsync()
        {
            using var context = await CreateContextAsync();
            return await context.Conferences
                .Include(c => c.Speakers)
                .Include(c => c.Documents)
                .ToListAsync();
        }

        public override async Task<Conference> GetByIdAsync(int id)
        {
            using var context = await CreateContextAsync();
            var conference = await context.Conferences
                .Include(c => c.Speakers)
                .Include(c => c.Documents)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (conference == null)
            {
                throw new KeyNotFoundException($"Conference with ID {id} not found.");
            }

            return conference;
        }

        public override async Task<Conference> AddAsync(Conference conference)
        {
            using var context = await CreateContextAsync();
            await context.Conferences.AddAsync(conference);
            await context.SaveChangesAsync();
            return conference;
        }

        public override async Task<Conference> UpdateAsync(Conference conference)
        {
            using var context = await CreateContextAsync();
            context.Conferences.Update(conference);
            await context.SaveChangesAsync();
            return conference;
        }

        public override async Task DeleteAsync(int id)
        {
            using var context = await CreateContextAsync();
            var conference = await context.Conferences.FindAsync(id);
            if (conference != null)
            {
                context.Conferences.Remove(conference);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Conference>> GetBySpeakerIdAsync(int speakerId)
        {
            using var context = await CreateContextAsync();
            return await context.Conferences
                .Include(c => c.Speakers)
                .Where(c => c.Speakers.Any(s => s.Id == speakerId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Conference>> GetByDocumentIdAsync(int documentId)
        {
            using var context = await CreateContextAsync();
            return await context.Conferences
                .Include(c => c.Documents)
                .Where(c => c.Documents.Any(d => d.Id == documentId))
                .ToListAsync();
        }
    }
} 