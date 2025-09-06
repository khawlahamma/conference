using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ConferenceManager.WPF.Models;
using ConferenceManager.WPF.Data;
using ConferenceManager.WPF.Repositories;
using System.IO;
using System.Linq;

namespace ConferenceManager.WPF.Services
{
    public class SpeakerService : ISpeakerService
    {
        private readonly ISpeakerRepository _speakerRepository;
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public SpeakerService(
            ISpeakerRepository speakerRepository,
            IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _speakerRepository = speakerRepository;
            _contextFactory = contextFactory;
        }

        public async Task<IEnumerable<Speaker>> GetAllSpeakersAsync()
        {
            return await _speakerRepository.GetAllAsync();
        }

        public async Task<Speaker?> GetSpeakerByIdAsync(int id)
        {
            return await _speakerRepository.GetByIdAsync(id);
        }

        public async Task<Speaker> CreateSpeakerAsync(Speaker speaker)
        {
            return await _speakerRepository.AddAsync(speaker);
        }

        public async Task<Speaker> UpdateSpeakerAsync(Speaker speaker)
        {
            return await _speakerRepository.UpdateAsync(speaker);
        }

        public async Task DeleteSpeakerAsync(int id)
        {
            await _speakerRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Speaker>> GetSpeakersByConferenceAsync(int conferenceId)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Speakers.Where(s => s.ConferenceId == conferenceId).ToListAsync();
        }

        public async Task<IEnumerable<Speaker>> GetSpeakersByExpertiseAsync(string expertise)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Speakers.Where(s => s.Expertise == expertise).ToListAsync();
        }

        public async Task<IEnumerable<Conference>> GetConferencesBySpeakerAsync(int speakerId)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Conferences.Where(c => c.Speakers.Any(s => s.Id == speakerId)).ToListAsync();
        }

        public async Task<IEnumerable<Document>> GetDocumentsBySpeakerAsync(int speakerId)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Documents.Where(d => d.SpeakerId == speakerId).ToListAsync();
        }

        public async Task<IEnumerable<Speaker>> GetSpeakersAsync()
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Speakers.ToListAsync();
        }

        public async Task<IEnumerable<Speaker>> SearchSpeakersAsync(string searchTerm)
        {
            var speakers = await _speakerRepository.GetAllAsync();
            return speakers.Where(s =>
                (string.IsNullOrEmpty(searchTerm) || s.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) || s.Expertise.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            );
        }

        public async Task ExportSpeakersAsync(string filePath)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            var speakers = await context.Speakers.ToListAsync();
            var json = System.Text.Json.JsonSerializer.Serialize(speakers, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
            await System.IO.File.WriteAllTextAsync(filePath, json);
        }

        public async Task ImportSpeakersAsync(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
                throw new FileNotFoundException("File not found.");
            using var context = await _contextFactory.CreateDbContextAsync();
            var json = await System.IO.File.ReadAllTextAsync(filePath);
            var speakers = System.Text.Json.JsonSerializer.Deserialize<List<Speaker>>(json);
            if (speakers != null)
            {
                foreach (var speaker in speakers)
                {
                    if (!context.Speakers.Any(s => s.Email == speaker.Email))
                    {
                        await _speakerRepository.AddAsync(speaker);
                    }
                }
                await context.SaveChangesAsync();
            }
        }
    }
} 