using System.Windows;
using ConferenceManager.WPF.ViewModels;

namespace ConferenceManager.WPF.Views
{
    public partial class SpeakerDialog : Window
    {
        private readonly SpeakerDialogViewModel _viewModel;

        public SpeakerDialog(SpeakerDialogViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
        }
    }
} 