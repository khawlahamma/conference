using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ConferenceManager.WPF.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        public string Role { get; set; } = "Utilisateur";
        public string Phone { get; set; }
        public string ProfilePicture { get; set; }
        public bool EmailNotifications { get; set; }
        public bool DarkMode { get; set; }
        public string Language { get; set; } = "fr";
        public string Username { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLogin { get; set; }
    }
} 