using System.Windows.Controls;
using ConferenceManager.WPF.ViewModels;
using ConferenceManager.WPF.Services;
using System.Windows;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace ConferenceManager.WPF.Views
{
    public partial class MainView : UserControl
    {
        private readonly MainViewModel _viewModel;

        public MainView() : this(((App)Application.Current).Services.GetRequiredService<MainViewModel>()) { }

        public MainView(MainViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
            Loaded += MainView_Loaded;
        }

        private async void MainView_Loaded(object sender, RoutedEventArgs e)
        {
            await _viewModel.LoadDataAsync();
        }
    }
} 