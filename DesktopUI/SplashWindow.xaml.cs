using System;
using System.Collections.Generic;
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
    /// Interaction logic for SplashWindow.xaml
    /// </summary>
    public partial class SplashWindow : Window
    {
        public SplashWindow()
        {
            InitializeComponent();
            this.DataContext = new SplashVM();
            (DataContext as SplashVM).CloseWindowEvent += CommandBench_CloseWindowEvent;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
        private void CommandBench_CloseWindowEvent(object sender, EventArgs e)
        {
            this.DataContext = null;
            Close();
        }
    }
}
