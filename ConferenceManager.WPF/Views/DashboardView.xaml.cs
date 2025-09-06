using System.Windows.Controls;

namespace ConferenceManager.WPF.Views
{
    public partial class DashboardView : UserControl
    {
        public DashboardView(object dataContext)
        {
            InitializeComponent();
            DataContext = dataContext;
        }
    }
} 