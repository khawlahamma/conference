using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ConferenceManager.WPF.Data;
using ConferenceManager.WPF.Repositories;
using ConferenceManager.WPF.Services;
using ConferenceManager.WPF.ViewModels;
using ConferenceManager.WPF.Views;

namespace ConferenceManager.WPF.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(
                    connectionString,
                    ServerVersion.AutoDetect(connectionString),
                    options => options.EnableRetryOnFailure()
                )
            );

            // Ajout pour la factory (résout l'erreur DI)
            services.AddDbContextFactory<ApplicationDbContext>(options =>
                options.UseMySql(
                    connectionString,
                    ServerVersion.AutoDetect(connectionString),
                    options => options.EnableRetryOnFailure()
                )
            );

            // Add Repositories
            services.AddScoped<IConferenceRepository, ConferenceRepository>();
            services.AddScoped<ISpeakerRepository, SpeakerRepository>();
            services.AddScoped<IDocumentRepository, DocumentRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            // Add Services
            services.AddScoped<IConferenceService, ConferenceService>();
            services.AddScoped<ISpeakerService, SpeakerService>();
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IExportService, ExportService>();

            // Add ViewModels
            services.AddTransient<MainViewModel>();
            services.AddTransient<ConferencesViewModel>();
            services.AddTransient<SpeakersViewModel>();
            services.AddTransient<DocumentsViewModel>();
            services.AddTransient<ProfileViewModel>();
            services.AddTransient<ExportViewModel>();
            services.AddTransient<AdvancedSearchViewModel>();

            // Add Views
            services.AddSingleton<MainWindow>();
            services.AddTransient<ConferencesView>();
            services.AddTransient<SpeakersView>();
            services.AddTransient<DocumentsView>();
            services.AddTransient<ExportView>();
            services.AddTransient<AdvancedSearchView>();
            services.AddTransient<ProfileView>();

            return services;
        }
    }
} 