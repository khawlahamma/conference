using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ConferenceManager.WPF.Models;
using Microsoft.Win32;
using System.Text.Json;

namespace ConferenceManager.WPF.Services
{
    public class ExportService : IExportService
    {
        private readonly string _exportDirectory;

        public ExportService()
        {
            _exportDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "ConferenceManager",
                "Exports");
            Directory.CreateDirectory(_exportDirectory);
        }

        public async Task ExportConferencesAsync(string filePath, IEnumerable<Conference> conferences)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            var json = JsonSerializer.Serialize(conferences, options);
            await File.WriteAllTextAsync(filePath, json);
        }

        public async Task ExportSpeakersAsync(string filePath, IEnumerable<Speaker> speakers)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            var json = JsonSerializer.Serialize(speakers, options);
            await File.WriteAllTextAsync(filePath, json);
        }

        public async Task ExportDocumentsAsync(string filePath, IEnumerable<Document> documents)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            var json = JsonSerializer.Serialize(documents, options);
            await File.WriteAllTextAsync(filePath, json);
        }

        public Task<string> GetExportDirectoryAsync()
        {
            return Task.FromResult(_exportDirectory);
        }
    }
} 