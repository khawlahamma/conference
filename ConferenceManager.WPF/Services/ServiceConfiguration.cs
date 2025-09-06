using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using ConferenceManager.WPF.Data;
using ConferenceManager.WPF.Repositories;
using ConferenceManager.WPF.Services;
using ConferenceManager.WPF.ViewModels;
using ConferenceManager.WPF.Views;
using ConferenceManager.WPF;

namespace ConferenceManager.WPF.Services
{
    public static class ServiceConfiguration
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            
            // Database
            services.AddDbContextFactory<ApplicationDbContext>(options =>
                options.UseMySql(
                    connectionString,
                    ServerVersion.AutoDetect(connectionString),
                    options => options.EnableRetryOnFailure()
                )
            );

            // Repositories
            services.AddScoped<IConferenceRepository, ConferenceRepository>();
            services.AddScoped<ISpeakerRepository, SpeakerRepository>();
            services.AddScoped<IDocumentRepository, DocumentRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            // Services
            services.AddScoped<IConferenceService, ConferenceService>();
            services.AddScoped<ISpeakerService, SpeakerService>();
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IExportService, ExportService>();

            // ViewModels
            services.AddScoped<MainViewModel>();
            services.AddTransient<AdvancedSearchViewModel>();
            services.AddTransient<ProfileViewModel>();
            services.AddTransient<DocumentsViewModel>();
            services.AddTransient<SpeakersViewModel>();
            services.AddTransient<SpeakerDialogViewModel>();

            // Views
            services.AddScoped<MainView>();
            services.AddTransient<AdvancedSearchView>();
            services.AddTransient<ProfileView>();
            services.AddTransient<DocumentsView>();
            services.AddTransient<SpeakersView>();
            services.AddTransient<SpeakerDialog>();

            // MainWindow
            services.AddScoped<MainWindow>();
        }
    }
} 