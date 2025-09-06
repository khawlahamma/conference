using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConferenceManager.WPF.Models
{
    public class Document
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; } = string.Empty;
        [NotMapped]
        public List<string> Tags { get; set; } = new List<string>();
        public int ConferenceId { get; set; }
        public Conference Conference { get; set; } = null!;
        public int? SpeakerId { get; set; }
        public Speaker? Speaker { get; set; }
        public string DocumentType { get; set; } = string.Empty;
        public DateTime LastModified { get; set; }
        public DateTime Date { get; set; }
        public bool IsPublic { get; set; }
        public string FileName { get; set; } = string.Empty;
    }
} 