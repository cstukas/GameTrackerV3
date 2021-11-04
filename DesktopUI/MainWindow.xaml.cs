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
using System.Windows.Navigation;
using System.Windows.Shapes;
using GameBL;

namespace DesktopUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(TabVMs.CollectionMediaVM loadedCollection, TabVMs.FriendsVM loadedFriends, TabVMs.StatsVM loadedStats)
        {
            InitializeComponent();
            this.DataContext = new MainVM(loadedCollection, loadedFriends, loadedStats);
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

        }

        private void ConsoleStatsRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListBoxItem;
            var stat = item.Content as Stat;
            if (stat?.Name == "") return;

            (DataContext as MainVM).ConsoleStatsClicked(stat);
        }

        private void YearStatsRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListBoxItem;
            var stat = item.Content as Stat;
            if (stat?.Name == "") return;

            (DataContext as MainVM).YearStatsClicked(stat);
        }

        private void TopMonthsStatsRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListBoxItem;
            var stat = item.Content as Stat;
            if (stat?.Name == "") return;

            (DataContext as MainVM).TopYearStatsClicked(stat);

        }

        private void SeriesStatsRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //var item = sender as ListBoxItem;
            //var stat = item.Content as SeriesStat;
            //if (stat != null)
            //{
            //    MainTabControl.SelectedIndex = 2; // Series Tab
            //    seriesFilterCombo.SelectedItem = stat.Name;
            //}
        }

        public void LoadSeriesStatsList(int sort)
        {
            //if (IsOnSeriesStatsTab)
            //{
            //    Mouse.OverrideCursor = Cursors.Wait;

            //    if (sort == 1)
            //        Data.SeriesStats = new SeriesStats("BeatenPercentage");
            //    else if (sort == 2)
            //        Data.SeriesStats = new SeriesStats("OwnPercentage");
            //    if (sort == 3)
            //        Data.SeriesStats = new SeriesStats("TotalGames");

            //    SeriesStatsListBox.ItemsSource = Data.SeriesStats;
            //    Mouse.OverrideCursor = null;
            //}
        }


        private void CompletionRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            LoadSeriesStatsList(1);
        }

        private void OwnedRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            LoadSeriesStatsList(2);
        }

        private void TotalRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            LoadSeriesStatsList(3);
        }

   


        public void LoadTopMonths()
        {
            //if (IsOnTopMonthsStatsTab)
            //{
            //    Mouse.OverrideCursor = Cursors.Wait;

            //    var monthStats = new ObservableCollection<Stat>();
            //    for (int y = 1992; y <= DateTime.Now.Year; y++)
            //    {
            //        for (int m = 1; m <= 12; m++)
            //        {
            //            var count = Data.AllPlayedGames.Count(x => x.Beaten && x.YearCompleted == y && x.MonthCompleted == m);
            //            var stat = new Stat();
            //            stat.Name = Utils.GetMonthNameFromNumber(m);
            //            stat.Value = y.ToString();
            //            stat.Value2 = count.ToString();
            //            stat.IntValue1 = count;
            //            stat.IntValue2 = m;

            //            monthStats.Add(stat);
            //        }
            //    }

            //    int rank = 0;
            //    string prevCount = "";
            //    var displayStats = new ObservableCollection<Stat>();
            //    var ordered = monthStats.OrderByDescending(x => x.IntValue1).ToList();
            //    for (int o = 0; o < ordered.Count; o++)
            //    {
            //        var stat = ordered[o];
            //        if (prevCount != stat.Value2)
            //        {
            //            prevCount = stat.Value2;
            //            rank++;
            //            if (rank > 5)
            //                break;
            //        }

            //        stat.Value3 = rank.ToString();

            //        displayStats.Add(stat);
            //    }

            //    TopMonthsListBox.ItemsSource = displayStats.ToList();

            //    Mouse.OverrideCursor = null;
            //}


        }


    }
}
