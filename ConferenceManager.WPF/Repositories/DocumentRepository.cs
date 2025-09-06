using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ConferenceManager.WPF.Models;
using ConferenceManager.WPF.Data;

namespace ConferenceManager.WPF.Repositories
{
    public class DocumentRepository : Repository<Document>, IDocumentRepository
    {
        public DocumentRepository(IDbContextFactory<ApplicationDbContext> contextFactory) 
            : base(contextFactory)
        {
        }

        public override async Task<IEnumerable<Document>> GetAllAsync()
        {
            using var context = await CreateContextAsync();
            return await context.Documents
                .Include(d => d.Conference)
                .ToListAsync();
        }

        public override async Task<Document> GetByIdAsync(int id)
        {
            using var context = await CreateContextAsync();
            return await context.Documents
                .Include(d => d.Conference)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public override async Task<Document> AddAsync(Document document)
        {
            using var context = await CreateContextAsync();
            await context.Documents.AddAsync(document);
            await context.SaveChangesAsync();
            return document;
        }

        public override async Task<Document> UpdateAsync(Document document)
        {
            using var context = await CreateContextAsync();
            context.Documents.Update(document);
            await context.SaveChangesAsync();
            return document;
        }

        public override async Task DeleteAsync(int id)
        {
            using var context = await CreateContextAsync();
            var document = await context.Documents.FindAsync(id);
            if (document != null)
            {
                context.Documents.Remove(document);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Document>> GetByConferenceIdAsync(int conferenceId)
        {
            using var context = await CreateContextAsync();
            return await context.Documents
                .Where(d => d.ConferenceId == conferenceId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Document>> GetBySpeakerIdAsync(int speakerId)
        {
            using var context = await CreateContextAsync();
            return await context.Documents
                .Where(d => d.SpeakerId == speakerId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Document>> GetPublicDocumentsAsync()
        {
            using var context = await CreateContextAsync();
            return await context.Documents
                .Where(d => d.IsPublic)
                .ToListAsync();
        }

        public async Task<IEnumerable<Document>> GetByTypeAsync(string type)
        {
            using var context = await CreateContextAsync();
            return await context.Documents
                .Where(d => d.Type == type)
                .ToListAsync();
        }

        public async Task<IEnumerable<Document>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            using var context = await CreateContextAsync();
            return await context.Documents
                .Where(d => d.Date >= startDate && d.Date <= endDate)
                .ToListAsync();
        }
    }
} 