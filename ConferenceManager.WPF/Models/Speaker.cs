using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConferenceManager.WPF.Models
{
    public class Speaker
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Organization { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Biography { get; set; } = string.Empty;
        public string Expertise { get; set; } = string.Empty;
        public string ProfilePicture { get; set; } = string.Empty;
        public DateTime RegistrationDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
        public string LinkedIn { get; set; } = string.Empty;
        public string Twitter { get; set; } = string.Empty;
        public virtual ICollection<Conference> Conferences { get; set; } = new List<Conference>();
        public virtual ICollection<Document> Documents { get; set; } = new List<Document>();
        public int? ConferenceId { get; set; }
        [NotMapped]
        public virtual Conference? Conference { get; set; }
        public string Bio { get; set; }
    }
} 