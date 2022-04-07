using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GameBL;
using DesktopUI.TabVMs;
using System.Collections.ObjectModel;

namespace DesktopUI
{
    public class MainVM : Utilities.BaseClasses.NotifyPropertyChanged
    {

        public ICommand AddToCollectionCommand { get; private set; }
        public ICommand AddToPlayedCommand { get; private set; }
        public ICommand AddRemoveFriendCommand { get; private set; }
        public ICommand MyProfileCommand { get; private set; }
        public ICommand ViewUserCommand { get; private set; }
        public ICommand ViewMediaCommand { get; private set; }
        public ICommand OpenDownloadCommand { get; private set; }
        public ICommand OpenGamesWindowCommand { get; private set; }
        public ICommand ShowKeysCommand { get; private set; }
        public ICommand ConsoleStatOrderCommand { get; private set; }

        private bool showKeys;
        public bool ShowKeys
        {
            get { return showKeys; }
            set { showKeys = value; OnPropertyChanged("ShowKeys"); }
        }

        public string WindowTitle => $"Game Tracker - Logged in as {Utilities.UserUtils.CurrentUser.UserName}";

        private Utilities.User profileToView;
        public Utilities.User ProfileToView
        {
            get { return profileToView; }
            set { profileToView = value; OnPropertyChanged("ProfileToView"); }
        }

        public Utilities.User CurrentUser => Utilities.UserUtils.CurrentUser;

        enum MenuTabs
        {
            Feed,
            Collection,
            PlayedGames,
            Series,
            ToPlay,
            ToBuy,
            Friends
        }

        public FeedVM FeedVM { get; set; }
        public PlayedGameVM PlayedVM { get; set; }
        public FriendsVM FriendsVM { get; set; }
        public CollectionMediaVM CollectionVM { get; set; }
        public ToPlayVM ToPlayVM { get; set; }
        public SeriesVM SeriesVM { get; set; }
        public ToBuyVm ToBuyVM { get; set; }
        public PlatformsVM PlatformsVM { get; set; }
        public StatsVM StatsVM { get; set; }



        private ObservableCollection<Stat> consoleStats;
        public ObservableCollection<Stat> ConsoleStats
        {
            get { return consoleStats; }
            set { consoleStats = value; OnPropertyChanged("ConsoleStats"); }
        }

        private ObservableCollection<Stat> yearStats;
        public ObservableCollection<Stat> YearStats
        {
            get { return yearStats; }
            set { yearStats = value; OnPropertyChanged("YearStats"); }
        }

        private SeriesStats seriesStatsList;
        public SeriesStats SeriesStatsList
        {
            get { return seriesStatsList; }
            set { seriesStatsList = value; OnPropertyChanged("SeriesStatsList"); }
        }

        public SeriesStat SelectedSeriesStat { get; set; }

        private ObservableCollection<Stat> rankedYearStats;
        public ObservableCollection<Stat> RankedYearStats
        {
            get { return rankedYearStats; }
            set { rankedYearStats = value; OnPropertyChanged("RankedYearStats"); }
        }

        public ObservableCollection<Stat> TopMonthsStats { get; set; }

        public bool IsOnSeriesTab { get; set; }

        private int statsSelectedTabIndex;

        public int StatsSelectedTabIndex
        {
            get
            {
                switch (statsSelectedTabIndex)
                {
                    case 3: // series stat
                        LoadTopMonths();
                        break;
                }

                if (statsSelectedTabIndex == 2)
                    IsOnSeriesTab = true;
                else
                    IsOnSeriesTab = false;



                return statsSelectedTabIndex;
            }
            set { statsSelectedTabIndex = value; }
        }

        private int selectedTabIndex; // Default Tab
        /// <summary>
        /// Keeps track of what tab we are on.
        /// </summary>
        public int SelectedTabIndex
        {
            get { return selectedTabIndex; }
            set
            {
                selectedTabIndex = value;
                OnPropertyChanged("SelectedTabIndex");
                if (selectedTabIndex == (int)MenuTabs.Feed)
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                    FeedVM.UpdateFeed();
                    Mouse.OverrideCursor = null;
                }
                else if (selectedTabIndex == (int)MenuTabs.PlayedGames)
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                    PlayedVM.Load(ProfileToView);
                    Mouse.OverrideCursor = null;

                }
                else if (selectedTabIndex == (int)MenuTabs.Collection)
                {
                    CollectionVM.RefreshData();
                }
                else if (selectedTabIndex == (int)MenuTabs.Series)
                {

                }
                else if (selectedTabIndex == (int)MenuTabs.ToPlay)
                {
                    ToPlayVM.RefreshData();
                }
                else if (selectedTabIndex == (int)MenuTabs.ToBuy)
                {
                    ToBuyVM.RefreshData();

                }
                else if (selectedTabIndex == (int)MenuTabs.Friends)
                {

                }


                Mouse.OverrideCursor = null;
            }
        }

        private bool updateAvailable;
        public bool UpdateAvailable
        {
            get { return updateAvailable; }
            set { updateAvailable = value; OnPropertyChanged("UpdateAvailable"); }
        }

        private string completionString;
        public string CompletionString
        {
            get { return completionString; }
            set { completionString = value; OnPropertyChanged("CompletionString"); }
        }

        private string totalGamesString;
        public string TotalGamesString
        {
            get { return totalGamesString; }
            set { totalGamesString = value; OnPropertyChanged("TotalGamesString"); }
        }

        private string uniqueGamesString;
        public string UniqueGamesString
        {
            get { return uniqueGamesString; }
            set { uniqueGamesString = value; OnPropertyChanged("UniqueGamesString"); }
        }

        private string gamesToBeatString;
        public string GamesToBeatString
        {
            get { return gamesToBeatString; }
            set { gamesToBeatString = value; OnPropertyChanged("GamesToBeatString"); }
        }

        private string totalGamesBeatString;
        public string TotalGamesBeatString
        {
            get { return totalGamesBeatString; }
            set { totalGamesBeatString = value; OnPropertyChanged("TotalGamesBeatString"); }
        }




        public MainVM(CollectionMediaVM loadedCollection, FriendsVM loadedFriends, StatsVM loadedStats)
        {
            this.AddToCollectionCommand = new DelegateCommand<object>(this.OnAddToCollection);
            this.AddToPlayedCommand = new DelegateCommand<object>(this.OnAddToPlayed);
            this.AddRemoveFriendCommand = new DelegateCommand<object>(this.OnAddRemoveFriendCommand);
            this.MyProfileCommand = new DelegateCommand<object>(this.OnMyProfile);
            this.ViewUserCommand = new DelegateCommand<object>(this.OnViewUser);
            this.ViewMediaCommand = new DelegateCommand<object>(this.OnViewMedia);
            this.OpenDownloadCommand = new DelegateCommand<object>(this.OnOpenDownload);
            this.OpenGamesWindowCommand = new DelegateCommand<object>(this.OnOpenGamesWindow);
            this.ShowKeysCommand = new DelegateCommand<object>(this.OnShowKeys);
            this.ConsoleStatOrderCommand = new DelegateCommand<object>(this.OnConsoleStatOrder);


            FeedVM = new FeedVM(this);
            PlayedVM = new PlayedGameVM(this);
            CollectionVM = loadedCollection;
            CollectionVM.ParentVM = this;
            FriendsVM = loadedFriends;
            FriendsVM.ParentVM = this;
            ToPlayVM = new ToPlayVM(this);
            ToBuyVM = new ToBuyVm(this);
            SeriesVM = new SeriesVM(this);
            PlatformsVM = new PlatformsVM(this);
            StatsVM = loadedStats;
            StatsVM.ParentVM = this;

            YearStats = new ObservableCollection<Stat>();
            RankedYearStats = new ObservableCollection<Stat>();

            UpdateAvailable = Globals.UpdateAvailable;

            ProfileToView = Utilities.UserUtils.CurrentUser;

            // Default tab
            //selectedTabIndex = (int)MenuTabs.FinishedMedia;
            //FinishedVM.Load(ProfileToView);
            selectedTabIndex = (int)MenuTabs.Collection;
            CollectionVM.RefreshData();

            ShowKeys = false;

            UpdateStats();


            LoadSeriesStatsList(1);
        }

        public void UpdateStats()
        {
            Console.WriteLine("Updating Stats");
            var collection = LoadedData.MyCollection.Where(x => x.Playing == 1 && x.Own == 1 && x.UserKey == CurrentUser.UserKey).ToList();
            var beaten = collection.Where(x => x.Finished == 1).ToList();
            Stats.CollectionCount = collection.Count;
            Stats.CollectionBeatenCount = beaten.Count;
            var allBeaten = PlayedGameList.LoadYourPlayed(CurrentUser.UserKey);

            Stats.TotalBeatenCount = allBeaten.Count;

            string percent = Utilities.General.CalculatePercentString(Stats.CollectionBeatenCount, Stats.CollectionCount, 4);

            TotalGamesString = $"{Stats.CollectionBeatenCount} / {Stats.CollectionCount}   {percent}%";

            // unique played
            var uniqueBeaten = new List<PlayedGame>();
            for (int fc = 0; fc < allBeaten.Count; fc++)
            {
                var game = allBeaten[fc];

                if (game.MatchingMedia?.RemakeOf > 0)
                {
                    var orig = allBeaten.FirstOrDefault(x => x.GameKey == game.MatchingMedia.RemakeOf);
                    if (orig != null)
                    {
                        game = orig;
                    }
                }

                uniqueBeaten.Add(game);
            }

            uniqueBeaten = uniqueBeaten.GroupBy(x => x.MatchingMedia?.Name).Select(g => g.First()).ToList();


            // Unique collection
            var uniqueCollection = new List<CollectionGame>();
            for (int fc = 0; fc < collection.Count; fc++)
            {
                var game = collection[fc];

                if (game.MatchingMedia?.RemakeOf > 0)
                {
                    var orig = collection.SingleOrDefault(x => x.GameKey == game.MatchingMedia.RemakeOf);
                    if (orig != null)
                    {
                        game = orig;
                    }
                }

                uniqueCollection.Add(game);
            }

            uniqueCollection = uniqueCollection.GroupBy(x => x.MatchingMedia.Name).Select(g => g.First()).ToList();



            //Unique Beat
            var uniqueBeat = new List<CollectionGame>();
            for (int fc = 0; fc < beaten.Count; fc++)
            {
                var game = beaten[fc];

                if (game.MatchingMedia.RemakeOf > 0)
                {
                    var orig = beaten.FirstOrDefault(x => x.GameKey == game.MatchingMedia.RemakeOf); // todo change from SingleOrDefault to FirstOrDefault
                    if (orig != null)
                    {
                        game = orig;
                    }
                }

                uniqueBeat.Add(game);

            }

            uniqueBeat = uniqueBeat.GroupBy(x => x.MatchingMedia.Name).Select(g => g.First()).ToList();




            Stats.UniqueCollectionCount = uniqueCollection.Count;
            Stats.UniqueBeatenCount = uniqueBeaten.Count;

            var uniqueGamesToBeat = uniqueCollection.Count - uniqueBeat.Count;

            string uniqPercent = Utilities.General.CalculatePercentString(uniqueBeat.Count, Stats.UniqueCollectionCount, 4);
            UniqueGamesString = $"{uniqueBeat.Count} / {Stats.UniqueCollectionCount}   {uniqPercent}%";

            // TOTAL COMPLETION
            decimal num1 = 0;
            if (percent != "NaN")
                num1 = Convert.ToDecimal(percent);

            decimal num2 = 0;
            if (uniqPercent != "NaN")
                num2 = Convert.ToDecimal(uniqPercent);

            decimal totalperc = num1 + num2;
            decimal completionPercent = totalperc / 2m;
            CompletionString = $"{completionPercent.ToString("F1")}%";

            Stats.ToBeatCount = collection.Where(x => x.Finished == 0).ToList().Count;
            Stats.UniqueToBeatCount = Stats.UniqueCollectionCount - uniqueBeat.Count;

            GamesToBeatString = $"Games To Beat: {Stats.ToBeatCount}        Unique: {Stats.UniqueToBeatCount}";


            TotalGamesBeatString = $"Total Games Beat: {Stats.TotalBeatenCount}     Unique: {Stats.UniqueBeatenCount}";


            // Console Counts
            var beatenList = new List<Stat>();
            var otherList = new List<Stat>();

            int totalTotal = 0;
            int totalBeaten = 0;
            foreach (var item in LoadedData.PlatformList)
            {
                var stat = new Stat();
                stat.Name = item.Name;

                if (stat.Name.ToLower() == "gamecube") stat.Name = "GC";
                if (stat.Name.Contains("Emulator")) stat.Name = stat.Name.Replace("Emulator", "Emu");


                var games = collection.Where(x => x.MatchingMedia.Platform == item.PlatformKey).ToList();
              
                int total = games.Count;
                int beat = games.Where(x => x.Finished == 1).ToList().Count;
                totalTotal += total;
                totalBeaten += beat;

                string consolePercent = "";
                if (beat == total)
                    consolePercent = Utilities.General.CalculatePercentString(beat, total, 3);
                else
                    consolePercent = Utilities.General.CalculatePercentString(beat, total, 2);


                // Set Values
                stat.Value = $"{beat} / {total}";
                stat.Value2 = $"{consolePercent}%";
                stat.Value4 = total.ToString(); // for ordering

                if (total != 0)
                {
                    if (beat == total)
                    {
                        beatenList.Add(stat);
                    }
                    else
                    {
                        otherList.Add(stat);
                    }

                }
            }

            // All stat
            string totalConsolePercent = "";
            if (totalBeaten == totalTotal)
                totalConsolePercent = Utilities.General.CalculatePercentString(totalBeaten, totalTotal, 3);
            else
                totalConsolePercent = Utilities.General.CalculatePercentString(totalBeaten, totalTotal, 2);

            ConsoleStats = new ObservableCollection<Stat>();

            var allStat = new Stat();
            allStat.Name = "<All>";
            allStat.Value = $"{totalBeaten} / {totalTotal}";
            allStat.Value2 = $"{totalConsolePercent}%";
            ConsoleStats.Add(allStat);

            //var beatenOrdered = beatenList.OrderByDescending(x => Convert.ToInt32(x.Value4)).ToList();
            //foreach (var item in beatenOrdered)
            //{
            //    ConsoleStats.Add(item);
            //}

            //ConsoleStats.Add(new Stat());

            //var order = otherList.OrderByDescending(x => x.Value2);
            //foreach (var item in order)
            //{
            //    ConsoleStats.Add(item);
            //}

            ConsoleStats = OrderConsoleStats(1, ConsoleStats, beatenList, otherList);


            //// Year Counts
            YearStats.Clear();
            RankedYearStats.Clear();
            int startYear = 1995;
            int yearTotalCount = 0;
            for (int i = DateTime.Now.Year; i > startYear; i--)
            {
                var stat = new Stat();
                stat.Name = i.ToString();

                int count = allBeaten.Count(x => x.DateAdded.Year == i);

                //var games = DBFunctions.LoadList<PlayedGameDto>($"SELECT GameKey, PercentComplete FROM PlayedGames WHERE UserKey = {CurrentUser.UserKey} AND YearCompleted = '{i}'").ToList();
                //for (int k = 0; k < games.Count; k++)
                //{
                //    var obj = (PlayedGameDto)games[k];

                //    bool beat = Data.Percentages.FirstOrDefault(x => x.ItemKey == obj.PercentComplete).Beat;
                //    if (beat) count++;
                //}

                yearTotalCount += count;
                stat.Value = count.ToString();
                YearStats.Add(stat);
            }

            //put total at top
            var allYearsStat = new Stat();
            allYearsStat.Name = "<All>";
            allYearsStat.Value = yearTotalCount.ToString();

            YearStats.Insert(0, allYearsStat);


            // Ranked Years
            int ranking = 0;
            string prevCount = "";
            string prevRanking = "";
            var ranked = YearStats.OrderByDescending(x => Convert.ToInt32(x.Value)).ToList();
            //    RankedYearStats.Add(new Stat());
            for (int r = 0; r < ranked.Count; r++)
            {
                Stat stat = ranked[r];
                if (stat.Name == "<All>") continue;

                Stat newStat = new Stat();
                newStat.Name = stat.Name;
                newStat.Value2 = stat.Value;

                ranking++;
                if (stat.Value == prevCount)
                {
                    newStat.Value = "#" + prevRanking;
                }
                else
                {
                    newStat.Value = "#" + ranking.ToString();
                }

                prevCount = stat.Value;
                prevRanking = ranking.ToString();

                RankedYearStats.Add(newStat);
            }



        }

        public ObservableCollection<Stat> OrderConsoleStats(int sortOrder, ObservableCollection<Stat> consolesStats, List<Stat> beatenList, List<Stat> otherList)
        {
            if(sortOrder == 1)
            {
                var beatenOrdered = beatenList.OrderByDescending(x => Convert.ToInt32(x.Value4)).ToList();
                foreach (var item in beatenOrdered)
                {
                    ConsoleStats.Add(item);
                }

                ConsoleStats.Add(new Stat());

                var order = otherList.OrderByDescending(x => x.Value2);
                foreach (var item in order)
                {
                    ConsoleStats.Add(item);
                }

                return ConsoleStats;
            }
            else
            {
                var newList = new ObservableCollection<Stat>();

                return newList;
            }







        }


        private void OnAddToCollection(object obj)
        {
          //  OpenEditGameWindow(true,true,false);
            OpenAddToCollectionWindow();
        }

        private void OnAddToPlayed(object obj)
        {
          //  OpenEditGameWindow(false, false, true);
            OpenAddToPlayedWindow();
        }

        public void OpenAddToCollectionWindow(CollectionGame game = null)
        {
            Stats.UpdateStats = false;
            var window = new AddToCollectionWindow(this, game);
            window.ShowDialog();

            if (Globals.RefreshCollection && SelectedTabIndex == (int)MenuTabs.Collection)
            {
                CollectionVM.RefreshData();
            }

            if (Globals.RefreshCollection && SelectedTabIndex == (int)MenuTabs.ToPlay)
            {
                ToPlayVM.RefreshData();
            }

            if (Globals.RefreshCollection && SelectedTabIndex == (int)MenuTabs.ToBuy)
            {
                ToBuyVM.RefreshData();
            }

            Globals.RefreshCollection = false;

            if (Stats.UpdateStats)
                UpdateStats();

            Mouse.OverrideCursor = null;

        }

        public void OpenAddToPlayedWindow(PlayedGame game = null)
        {
            Stats.UpdateStats = false;
            var window = new AddToPlayedWindow(this, game);
            window.ShowDialog();

            if (Globals.RefreshPlayed && SelectedTabIndex == (int)MenuTabs.PlayedGames)
            {
                PlayedVM.RefreshData();
            }

            if (Globals.RefreshPlayed && SelectedTabIndex == (int)MenuTabs.Feed)
            {
                FeedVM.UpdateFeed();
            }

            Globals.RefreshPlayed = false;

            if (Stats.UpdateStats)
                UpdateStats();

            Mouse.OverrideCursor = null;
        }

 
        private void OnMyProfile(object obj)
        {
            ViewUser(Utilities.UserUtils.CurrentUser);
        }

        private void OnAddRemoveFriendCommand(object obj)
        {


        }

        // View User
        public void ViewUser(Utilities.User user)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            ProfileToView = user;
            SelectedTabIndex = 2;
            Mouse.OverrideCursor = null;
        }
        public void ViewUser(int userKey)
        {
            ViewUser(Utilities.UserUtils.GetUserFromUserKey(userKey));
        }

        private void OnViewUser(object obj)
        {
            var userKey = (int)obj;
            ViewUser(userKey);

        }

        // View Media
        public void ViewMedia(Game game)
        {
            if (game == null) return;
            if (game.GameKey == 0) return;

            Mouse.OverrideCursor = Cursors.Wait;
            var nextWindow = new ReadGameWindow(this, game);
            nextWindow.ShowDialog();

        }

        private void OnViewMedia(object obj)
        {
            var media = (Game)obj;
            ViewMedia(media);

        }

        private void OnOpenDownload(object obj)
        {
            var website = "http://www.BuggagramMovieTracker.Weebly.com";
            System.Diagnostics.Process.Start(website);
        }

        private void OnOpenGamesWindow(object obj)
        {
            var gamesWindow = new GamesWindow();
            gamesWindow.Show();
        }
        private void OnShowKeys(object obj)
        {
            if (ShowKeys) ShowKeys = false;
            else ShowKeys = true;
        }

        public void OnConsoleStatOrder(object obj)
        {
            var para = obj.ToString();
            if(para.ToLower() == "totalgames")
            {
                Console.WriteLine("totalGames");
                
            }
            else if (para.ToLower() == "beatenpercent")
            {

            }
        }
        public void SeriesStatsClicked(SeriesStat stat)
        {
            SelectedTabIndex = (int)MenuTabs.Series; 
            SeriesVM.SelectedSeries = LoadedData.SeriesList.FirstOrDefault(x => x.SeriesKey == stat.SeriesKey);
        }
        


        public void ConsoleStatsClicked(Stat stat)
        {
            if (stat != null)
            {
                SelectedTabIndex = (int)MenuTabs.Collection; // Collection Tab

                CollectionVM.OwnedGames = true;
                CollectionVM.DigitalGames = false;
                CollectionVM.RomGames = false;
                CollectionVM.ExtraGames = false;
                CollectionVM.DontOwnedGames = false;

                if (stat.Name == "GC")
                    stat.Name = "GameCube";

                CollectionVM.SelectedPlatform = CollectionVM.PlatformList.FirstOrDefault(x => x.Name == stat.Name);
                CollectionVM.RefreshData();
            }
        }

        public void YearStatsClicked(Stat stat)
        {
            if (stat != null)
            {
                SelectedTabIndex = (int)MenuTabs.PlayedGames; // Played Tab

                PlayedVM.SelectedUser = Utilities.UserUtils.CurrentUser;
                //PlayedVM.OnlyBeatenGames = true;
                PlayedVM.SelectedPlatform = PlayedVM.PlatformList[0];
                PlayedVM.SelectedYear = stat.Name;
                PlayedVM.RefreshData();
            }

        }

        public void TopYearStatsClicked(Stat stat)
        {
            if (stat != null)
            {
                SelectedTabIndex = (int)MenuTabs.PlayedGames; // Played Tab

                PlayedVM.SelectedUser = Utilities.UserUtils.CurrentUser;
                //PlayedVM.OnlyBeatenGames = true;
                PlayedVM.SelectedPlatform = PlayedVM.PlatformList[0];
                PlayedVM.SelectedYear = stat.Value;
                PlayedVM.RefreshData();
            }

        }

        public void LoadTopMonths()
        {
            Mouse.OverrideCursor = Cursors.Wait;

            var monthStats = new ObservableCollection<Stat>();
            for (int y = 1992; y <= DateTime.Now.Year; y++)
            {
                for (int m = 1; m <= 12; m++)
                {
                    var count = LoadedData.MyPlayedGames.Count(x => x.Beaten == 1 && x.ExactDate == 1 && x.DateAdded.Year == y && x.DateAdded.Month == m);
                    var stat = new Stat();
                    stat.Name = Utilities.General.GetMonthNameFromNumber(m);
                    stat.Value = y.ToString();
                    stat.Value2 = count.ToString();
                    stat.IntValue1 = count;
                    stat.IntValue2 = m;

                    monthStats.Add(stat);
                }
            }

            int rank = 0;
            string prevCount = "";
            TopMonthsStats = new ObservableCollection<Stat>();
            var ordered = monthStats.OrderByDescending(x => x.IntValue1).ToList();
            for (int o = 0; o < ordered.Count; o++)
            {
                var stat = ordered[o];
                if (prevCount != stat.Value2)
                {
                    prevCount = stat.Value2;
                    rank++;
                    if (rank > 5)
                        break;
                }

                stat.Value3 = rank.ToString();

                if(stat.Value2 != "0")
                    TopMonthsStats.Add(stat);
            }

            OnPropertyChanged("TopMonthsStats");

            Mouse.OverrideCursor = null;
        }

        public void UpdateCollectionFinished(Game game)
        {
            var alikeGames = CollectionGame.GetAllAlikeGames(game, LoadedData.MyCollection);
            var beatenGames = PlayedGame.GetBeatenGamesFromMemory(alikeGames);

            for (int a = 0; a < alikeGames.Count; a++)
            {
                var cg = alikeGames[a];

                PlayedGame.UpdatePlayedCount(beatenGames.Count, cg.GameKey, Utilities.UserUtils.CurrentUser.UserKey);

                var matchInMem = LoadedData.MyCollection.FirstOrDefault(x => x.GameKey == cg.GameKey && x.UserKey == Utilities.UserUtils.CurrentUser.UserKey);
                if (matchInMem != null)
                {
                    matchInMem.TimesBeat = beatenGames.Count;
                    if (beatenGames.Count > 0)
                        matchInMem.Finished = 1;
                    else
                        matchInMem.Finished = 0;
                }
            }

        }

        public void LoadSeriesStatsList(int sort)
        {
            Mouse.OverrideCursor = Cursors.Wait;


            SeriesStats stats;

            if (sort == 1)
                stats = new SeriesStats("BeatenPercentage");
            else if (sort == 2)
                stats = new SeriesStats("OwnPercentage");
            else
                stats = new SeriesStats("TotalGames");

            SeriesStatsList = stats;

            Mouse.OverrideCursor = null;
        }
    }

   
}
