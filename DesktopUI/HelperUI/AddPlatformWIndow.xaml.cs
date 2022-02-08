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

namespace DesktopUI.HelperUI
{
    /// <summary>
    /// Interaction logic for AddPlatformWIndow.xaml
    /// </summary>
    public partial class AddPlatformWIndow : Window
    {
        public AddPlatformWIndow()
        {
            InitializeComponent();
            this.DataContext = new AddPlatformVM();
            (DataContext as AddPlatformVM).CloseWindowEvent += CommandBench_CloseWindowEvent;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
        private void CommandBench_CloseWindowEvent(object sender, EventArgs e)
        {
            this.DataContext = null;
            Close();
        }
    }
}
