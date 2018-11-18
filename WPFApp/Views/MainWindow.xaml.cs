using log4net;
using System.Reflection;
using System.Windows;
using WPFApp.ViewModel;

namespace WPFApp.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private static readonly ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public MainWindow()
        {
            InitializeComponent();
            Closing += MainView_Closing;
            DataContext = new MainWindowViewModel();
            Content = new ParametersView();
        }

        private void MainView_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Log.Info("Closing App");
        }
    }
}