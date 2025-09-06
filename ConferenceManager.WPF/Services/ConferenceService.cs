using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ConferenceManager.WPF.Data;
using ConferenceManager.WPF.Models;
using ConferenceManager.WPF.Repositories;

namespace ConferenceManager.WPF.Services
{
    public class ConferenceService : IConferenceService
    {
        private readonly IConferenceRepository _conferenceRepository;
        private readonly ISpeakerRepository _speakerRepository;
        private readonly IDocumentRepository _documentRepository;
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public ConferenceService(
            IConferenceRepository conferenceRepository,
            ISpeakerRepository speakerRepository,
            IDocumentRepository documentRepository,
            IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _conferenceRepository = conferenceRepository;
            _speakerRepository = speakerRepository;
            _documentRepository = documentRepository;
            _contextFactory = contextFactory;
        }

        public async Task<IEnumerable<Conference>> GetConferencesAsync()
        {
            return await _conferenceRepository.GetAllAsync();
        }

        public async Task<Conference> GetConferenceByIdAsync(int id)
        {
            return await _conferenceRepository.GetByIdAsync(id);
        }

        public async Task<Conference> AddConferenceAsync(Conference conference)
        {
            if (conference == null)
            {
                throw new ArgumentNullException(nameof(conference));
            }

            if (string.IsNullOrWhiteSpace(conference.Title))
            {
                throw new ArgumentException("Le titre de la conférence ne peut pas être vide.", nameof(conference));
            }

            return await _conferenceRepository.AddAsync(conference);
        }

        public async Task<Conference> UpdateConferenceAsync(Conference conference)
        {
            return await _conferenceRepository.UpdateAsync(conference);
        }

        public async Task DeleteConferenceAsync(int id)
        {
            await _conferenceRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Conference>> GetAllConferencesAsync()
        {
            return await _conferenceRepository.GetAllAsync();
        }

        public async Task<Conference> CreateConferenceAsync(Conference conference)
        {
            return await _conferenceRepository.AddAsync(conference);
        }

        public async Task<IEnumerable<Conference>> GetConferencesBySpeakerIdAsync(int speakerId)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Conferences
                .Include(c => c.Speakers)
                .Where(c => c.Speakers.Any(s => s.Id == speakerId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Speaker>> GetConferenceSpeakersAsync(int conferenceId)
        {
            return await _conferenceRepository.GetSpeakersAsync(conferenceId);
        }

        public async Task<IEnumerable<Conference>> GetConferencesByDocumentIdAsync(int documentId)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Conferences
                .Include(c => c.Documents)
                .Where(c => c.Documents.Any(d => d.Id == documentId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Document>> GetConferenceDocumentsAsync(int conferenceId)
        {
            return await _conferenceRepository.GetDocumentsAsync(conferenceId);
        }

        public async Task<IEnumerable<Conference>> GetConferencesByStatusAsync(string status)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Conferences
                .Where(c => c.Status == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<Conference>> GetConferencesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _conferenceRepository.GetByDateRangeAsync(startDate, endDate);
        }

        public async Task<IEnumerable<Conference>> GetUpcomingConferencesAsync()
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Conferences
                .Where(c => c.Date > DateTime.Now)
                .OrderBy(c => c.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Conference>> GetPastConferencesAsync()
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Conferences
                .Where(c => c.Date < DateTime.Now)
                .OrderByDescending(c => c.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Conference>> GetConferencesByLocationAsync(string location)
        {
            return await _conferenceRepository.GetByLocationAsync(location);
        }

        public async Task<IEnumerable<Conference>> SearchConferencesAsync(string searchTerm, string location, DateTime? startDate, DateTime? endDate)
        {
            var conferences = await _conferenceRepository.GetAllAsync();
            return conferences.Where(c =>
                (string.IsNullOrEmpty(searchTerm) || c.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) || c.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(location) || c.Location.Contains(location, StringComparison.OrdinalIgnoreCase)) &&
                (!startDate.HasValue || c.Date >= startDate.Value) &&
                (!endDate.HasValue || c.Date <= endDate.Value)
            );
        }
    }
} 