using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DesktopUI
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            this.DataContext = new LoginVM();
            (DataContext as LoginVM).CloseWindowEvent += CommandBench_CloseWindowEvent;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

        }

        private void CommandBench_CloseWindowEvent(object sender, EventArgs e)
        {
            this.DataContext = null;
            Close();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            //Utilities.UserUtils.SaveMainWindowPreference(this);
            base.OnClosing(e);
        }

        private void ExitProgram(object sender, RoutedEventArgs e)
        {
            DataContext = null;
            Close();
        }
    }
}
