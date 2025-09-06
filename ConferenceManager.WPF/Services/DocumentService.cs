using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ConferenceManager.WPF.Data;
using ConferenceManager.WPF.Models;
using ConferenceManager.WPF.Repositories;

namespace ConferenceManager.WPF.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public DocumentService(
            IDocumentRepository documentRepository,
            IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _documentRepository = documentRepository;
            _contextFactory = contextFactory;
        }

        public async Task<IEnumerable<Document>> GetDocumentsAsync()
        {
            return await _documentRepository.GetAllAsync();
        }

        public async Task<Document> GetDocumentByIdAsync(int id)
        {
            return await _documentRepository.GetByIdAsync(id);
        }

        public async Task<Document> AddDocumentAsync(Document document)
        {
            return await _documentRepository.AddAsync(document);
        }

        public async Task<Document> UpdateDocumentAsync(Document document)
        {
            return await _documentRepository.UpdateAsync(document);
        }

        public async Task DeleteDocumentAsync(int id)
        {
            await _documentRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Document>> GetDocumentsByConferenceAsync(int conferenceId)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Documents
                .Where(d => d.ConferenceId == conferenceId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Document>> GetDocumentsBySpeakerAsync(int speakerId)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Documents
                .Where(d => d.SpeakerId == speakerId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Document>> SearchDocumentsAsync(string title, string type, string author)
        {
            var documents = await _documentRepository.GetAllAsync();
            
            return documents.Where(d =>
                (string.IsNullOrEmpty(title) || d.Title.Contains(title, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(type) || d.Type.Contains(type, StringComparison.OrdinalIgnoreCase))
            );
        }

        public async Task<byte[]> DownloadDocumentAsync(int id)
        {
            var document = await _documentRepository.GetByIdAsync(id);
            if (document == null || string.IsNullOrEmpty(document.FilePath) || !File.Exists(document.FilePath))
                throw new FileNotFoundException("Document not found.");
            return await File.ReadAllBytesAsync(document.FilePath);
        }

        public async Task<string> UploadDocumentAsync(byte[] fileContent, string fileName)
        {
            var uploadPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "ConferenceManager",
                "Documents");

            Directory.CreateDirectory(uploadPath);

            var filePath = Path.Combine(uploadPath, fileName);
            await File.WriteAllBytesAsync(filePath, fileContent);

            return filePath;
        }

        public async Task<IEnumerable<Conference>> GetConferences()
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Conferences.ToListAsync();
        }

        public async Task ExportToExcelAsync(string filePath)
        {
            // TODO: Implémenter l'export Excel
            await Task.CompletedTask;
        }

        public async Task ImportFromExcelAsync(string filePath)
        {
            // TODO: Implémenter l'import Excel
            await Task.CompletedTask;
        }
    }
} 