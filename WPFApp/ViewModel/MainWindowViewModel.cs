using MvvmDialogs;
using System.Windows.Input;
using WPFApp.Utils;
using System.Windows.Controls;

namespace WPFApp.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Parameters
        /// <summary>
        /// Title of the application, as displayed in the top bar of the window
        /// </summary>
        public string Title
        {
            get { return "Encoimg"; }
        }

        #endregion

        #region Constructors
        public MainWindowViewModel()
        {
            Log.Info("[MainWindowViewModel] Constructor");
        }
        #endregion

    }
}