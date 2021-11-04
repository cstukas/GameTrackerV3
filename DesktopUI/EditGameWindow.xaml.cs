using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using GameBL;

namespace DesktopUI
{
    /// <summary>
    /// Interaction logic for EditGameWindow.xaml
    /// </summary>
    public partial class EditGameWindow : Window
    {
        public EditGameWindow(Game game)
        {
            InitializeComponent();
            this.DataContext = new EditGameVM(game);
            (DataContext as EditGameVM).CloseWindowEvent += CommandBench_CloseWindowEvent;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void CommandBench_CloseWindowEvent(object sender, EventArgs e)
        {
            this.DataContext = null;
            Close();
        }

        private void YearReleased_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
