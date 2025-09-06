using Microsoft.EntityFrameworkCore;
using ConferenceManager.WPF.Models;

namespace ConferenceManager.WPF.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Conference> Conferences { get; set; }
        public DbSet<Speaker> Speakers { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuration spécifique à MySQL
            modelBuilder.Entity<Conference>()
                .Property(c => c.Title)
                .HasColumnType("varchar(255)");

            modelBuilder.Entity<Speaker>()
                .Property(s => s.Name)
                .HasColumnType("varchar(255)");

            modelBuilder.Entity<Document>()
                .Property(d => d.Title)
                .HasColumnType("varchar(255)");

            // Relations existantes
            modelBuilder.Entity<Conference>()
                .HasMany(c => c.Speakers)
                .WithMany(s => s.Conferences);

            modelBuilder.Entity<Conference>()
                .HasMany(c => c.Documents)
                .WithOne(d => d.Conference)
                .HasForeignKey(d => d.ConferenceId);

            modelBuilder.Entity<Speaker>()
                .HasMany(s => s.Documents)
                .WithOne(d => d.Speaker)
                .HasForeignKey(d => d.SpeakerId);

            modelBuilder.Entity<List<string>>().HasNoKey();
        }
    }
} 