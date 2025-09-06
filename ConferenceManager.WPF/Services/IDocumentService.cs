using System.Collections.Generic;
using System.Threading.Tasks;
using ConferenceManager.WPF.Models;

namespace ConferenceManager.WPF.Services
{
    public interface IDocumentService
    {
        Task<IEnumerable<Document>> GetDocumentsAsync();
        Task<Document> GetDocumentByIdAsync(int id);
        Task<Document> AddDocumentAsync(Document document);
        Task<Document> UpdateDocumentAsync(Document document);
        Task DeleteDocumentAsync(int id);
        Task<IEnumerable<Document>> GetDocumentsByConferenceAsync(int conferenceId);
        Task<IEnumerable<Document>> GetDocumentsBySpeakerAsync(int speakerId);
        Task<IEnumerable<Document>> SearchDocumentsAsync(string searchTerm, string type, string status);
        Task<byte[]> DownloadDocumentAsync(int id);
        Task<string> UploadDocumentAsync(byte[] fileContent, string fileName);
        Task<IEnumerable<Conference>> GetConferences();
        Task ExportToExcelAsync(string filePath);
        Task ImportFromExcelAsync(string filePath);
    }
} 