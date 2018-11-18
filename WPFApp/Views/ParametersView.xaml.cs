using System.Windows.Controls;
using WPFApp.ViewModel;

namespace WPFApp.Views
{
    /// <summary>
    /// Interaction logic for ParametersView.xaml
    /// </summary>
    public partial class ParametersView : Page
    {
        public ParametersView()
        {
            InitializeComponent();
            this.DataContext = new ParametersViewModel();
        }
    }
}
