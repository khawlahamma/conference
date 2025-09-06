using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using ConferenceManager.WPF.Data;
using ConferenceManager.WPF.Extensions;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using System.IO;
using ConferenceManager.WPF.Services;
using ConferenceManager.WPF.ViewModels;
using ConferenceManager.WPF.Views;
using Microsoft.EntityFrameworkCore;
using ConferenceManager.WPF.Repositories;

namespace ConferenceManager.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;
        private IConfiguration _configuration;

        public IServiceProvider Services => _serviceProvider;

        public App()
        {
            this.DispatcherUnhandledException += (s, e) =>
            {
                MessageBox.Show(e.Exception.ToString(), "Unhandled Exception");
                e.Handled = true;
            };
            try
            {
                Debug.WriteLine("App constructor called.");
                var services = new ServiceCollection();
                
                // Configuration
                _configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();
                
                Debug.WriteLine("Configuration loaded successfully.");
                
                // Configure services
                services.AddApplicationServices(_configuration);
                Debug.WriteLine("Services configured successfully.");
                
                // Build service provider
                _serviceProvider = services.BuildServiceProvider();
                Debug.WriteLine("Service provider built successfully.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in App constructor: {ex}");
                MessageBox.Show($"Error initializing application: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();
            }
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                Debug.WriteLine("Application_Startup called.");

                // Initialize database
                using (var scope = _serviceProvider.CreateScope())
                {
                    Debug.WriteLine("Initializing database...");
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    context.Database.EnsureCreated();
                    Debug.WriteLine("Database initialized successfully.");
                }

                // Show main window
                Debug.WriteLine("Creating and showing MainWindow...");
                var conferenceService = _serviceProvider.GetRequiredService<IConferenceService>();
                var speakerService = _serviceProvider.GetRequiredService<ISpeakerService>();
                var documentService = _serviceProvider.GetRequiredService<IDocumentService>();
                var mainViewModel = new MainViewModel(conferenceService, speakerService, documentService);
                var mainWindow = new MainWindow(mainViewModel);
                mainWindow.Show();
                Debug.WriteLine("MainWindow shown successfully.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error during startup: {ex}");
                MessageBox.Show($"Error during startup: {ex.Message}\n\nStack trace:\n{ex.StackTrace}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            try
            {
                base.OnExit(e);
                Debug.WriteLine("OnExit called.");
                _serviceProvider?.Dispose();
                Debug.WriteLine("Service provider disposed successfully.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error during exit: {ex}");
                MessageBox.Show($"Error during exit: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
