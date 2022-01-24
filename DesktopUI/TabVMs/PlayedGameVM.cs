using GameBL;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DesktopUI.TabVMs
{
    public class PlayedGameVM : BaseMediaVM
    {
        public ICommand RefreshCommand { get; private set; }
        public ICommand EditSelectedCommand { get; private set; }
        public ICommand AddRemoveFriendCommand { get; private set; }

        public string AddFriendString => "Add Friend";
        public string RemoveFriendString => "Remove Friend";

        public List<Platform> PlatformList { get; set; }
        public List<string> YearList { get; set; }
        public ObservableCollection<Stat> PlatformPlayedStats { get; set; }
        public ObservableCollection<Stat> GenresPlayedStats { get; set; }
        public ObservableCollection<Stat> TopMonthsStats { get; set; }

        private string gap;
        public string Gap
        {
            get { return gap; }
            set { gap = value; OnPropertyChanged("Gap"); }
        }

        private string ogGap;
        public string OgGap
        {
            get { return ogGap; }
            set { ogGap = value; OnPropertyChanged("OgGap"); }
        }



        private int mediaCount;
        public int MediaCount
        {
            get { return mediaCount; }
            set { mediaCount = value; OnPropertyChanged("MediaCount"); }
        }


        private Utilities.User selectedUser;
        public Utilities.User SelectedUser
        {
            get { return selectedUser; }
            set { 
                selectedUser = value; 
                OnPropertyChanged("SelectedUser"); }
        }


        private PlayedGameList playedGameList;
        public PlayedGameList PlayedGameList
        {
            get { return playedGameList; }
            set { playedGameList = value; OnPropertyChanged("PlayedGameList"); }
        }

        private string addRemoveFriendString;
        public string AddRemoveFriendString
        {
            get { return addRemoveFriendString; }
            set { addRemoveFriendString = value; OnPropertyChanged("AddRemoveFriendString"); }
        }

        private string showPrivateColumn;
        public string ShowPrivateColumn
        {
            get
            {
                return showPrivateColumn;
            }
            set { showPrivateColumn = value; OnPropertyChanged("ShowPrivateColumnumn"); }
        }

        private bool onlyBeatenGames;
        public bool OnlyBeatenGames
        {
            get { return onlyBeatenGames; }
            set { onlyBeatenGames = value; OnPropertyChanged("OnlyBeatenGames"); }
        }

        private string selectedYear;
        public string SelectedYear
        {
            get { return selectedYear; }
            set { selectedYear = value; OnPropertyChanged("SelectedYear"); }
        }

        private Platform selectedPlatform;
        public Platform SelectedPlatform
        {
            get { return selectedPlatform; }
            set { selectedPlatform = value; OnPropertyChanged("SelectedPlatform"); }
        }

        private bool showKeys;
        public bool ShowKeys
        {
            get { return showKeys; }
            set { showKeys = value; OnPropertyChanged("ShowKeys"); }
        }
        
        private bool playedOnIsChecked;
        public bool PlayedOnIsChecked
        {
            get { return playedOnIsChecked; }
            set { playedOnIsChecked = value; OnPropertyChanged("PlayedOnIsChecked"); }
        }

        public PlayedGameVM(MainVM parentVM)
            : base(parentVM)
        {
            this.RefreshCommand = new DelegateCommand<object>(this.OnRefresh);
            this.EditSelectedCommand = new DelegateCommand<object>(this.OnEditSelected);
            this.AddRemoveFriendCommand = new DelegateCommand<object>(this.OnAddRemoveFriend);

            OnlyBeatenGames = true;
            PlayedOnIsChecked = false;

            PlatformList = LoadedData.PlatformListWithAll;
            if (PlatformList.Count > 0)
                SelectedPlatform = PlatformList[0];

            YearList = new List<string>();
            YearList.Add("<All>");
            for (int year = DateTime.Now.Year; year >= 1992; year--)
            {
                YearList.Add(year.ToString());
            }
            SelectedYear = YearList[0];

            ShowKeys = false;
        }

        private void OnEditSelected(object obj)
        {
            // Can only edit your own finished media
            if (SelectedUser.UserKey != Utilities.UserUtils.CurrentUser.UserKey)
            {
                MessageBox.Show("Can not edit another users entry.");
                return;
            }

            if (SelectedPlayedGame != null)
            {
                ParentVM.OpenAddToPlayedWindow(SelectedPlayedGame);
            }
        }

        public void RefreshData(bool onlyFriends = false)
        {
            
            var userKey = SelectedUser.UserKey;
            if (userKey == Utilities.UserUtils.CurrentUser.UserKey)
                PlayedGameList = PlayedGameList.LoadFromMemory(OnlyBeatenGames, SelectedYear, SelectedPlatform.PlatformKey, PlayedOnIsChecked);
            else
                PlayedGameList = PlayedGameList.LoadGame(userKey, true, -1, " ORDER BY DateAdded DESC", OnlyBeatenGames, SelectedYear, SelectedPlatform.PlatformKey);

            // STATS
            MediaCount = PlayedGameList.Count;

            PlatformPlayedStats = UpdatePlayedStats(PlayedGameList);
            OnPropertyChanged("PlatformPlayedStats");

            GenresPlayedStats = UpdateGenreStats(PlayedGameList);
            OnPropertyChanged("GenresPlayedStats");

            UpdateGapStats(PlayedGameList);

            TopMonthsStats = UpdateMonthStats(PlayedGameList);
            OnPropertyChanged("TopMonthsStats");

        }

        private void OnRefresh(object obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            RefreshData();
            Mouse.OverrideCursor = null;

        }

        public void Load(Utilities.User user)
        {
            SelectedUser = user;
            PlayedGameList = new PlayedGameList();

            // Check if we are friends
            if (SelectedUser != null)
            {
                var friends = Utilities.UserUtils.GetFriendsListKeys(Utilities.UserUtils.CurrentUser);
                if (friends.Contains(SelectedUser.UserKey))
                {
                    AddRemoveFriendString = RemoveFriendString;
                }
                else
                {
                    AddRemoveFriendString = AddFriendString;
                }
            }


            RefreshData();
        }

        private void OnAddRemoveFriend(object obj)
        {
            var areNowFriends = ParentVM.FriendsVM.SwapUser(SelectedUser, true);
            if (areNowFriends == 0)
            {
                AddRemoveFriendString = AddFriendString;
            }
            else if (areNowFriends == 1)
            {
                AddRemoveFriendString = RemoveFriendString;
            }
        }

        private ObservableCollection<Stat> UpdatePlayedStats(PlayedGameList games)
        {
            if (games.Count == 0) return new ObservableCollection<Stat>();

            // loop games to get platforms
            var platforms = new List<int>();
            for (int i = 0; i < games.Count; i++)
            {
                var game = games[i];
                //if (!platforms.Contains(game.PlatformPlayedOn))
                //{
                //    platforms.Add(game.PlatformPlayedOn);
                //}
                if (!platforms.Contains(game.MatchingMedia.Platform))
                {
                    platforms.Add(game.MatchingMedia.Platform);
                }
            }





            // loop platforms and get stats
            var stats = new List<Stat>();
            for (int k = 0; k < platforms.Count; k++)
            {
                var plat = platforms[k];
                var newStat = new Stat();
                newStat.Name = LoadedData.PlatformList.FirstOrDefault(x => x.PlatformKey == plat)?.Name;
                //int count = games.Count(x => x.PlatformPlayedOn == plat);
                int count = games.Count(x => x.MatchingMedia.Platform == plat);
                newStat.Value = count.ToString();
                decimal percentage = (decimal)(count / (decimal)games.Count) * 100;
                newStat.Value2 = percentage.ToString("n1") + "%";
                stats.Add(newStat);
            }

            // Create "Total" stat
            // dont really like this
            //var total = new Stat();
            //total.Name = "Total";
            //total.Value = games.Count.ToString();
            //stats.Add(total);

            // order
            var ordered = stats.OrderByDescending(x => Convert.ToInt32(x.Value)).ToList();

            return new ObservableCollection<Stat>(ordered);
        }

        private ObservableCollection<Stat> UpdateGenreStats(PlayedGameList games)
        {
            if (games.Count == 0) return new ObservableCollection<Stat>();

            // loop games to get platforms
            var genres = new List<int>();
            for (int i = 0; i < games.Count; i++)
            {
                var game = games[i];
                if(game.MatchingMedia != null)
                {
                    if (!genres.Contains(game.MatchingMedia.Genre1))
                    {
                        genres.Add(game.MatchingMedia.Genre1);
                    }
                }

            }

            // loop platforms and get stats
            var stats = new List<Stat>();
            for (int k = 0; k < genres.Count; k++)
            {
                var gen = genres[k];
                var newStat = new Stat();

                newStat.Name = Genre.KeyToName(gen);

                int count = games.Count(x => x.MatchingMedia.Genre1 == gen);
                newStat.Value = count.ToString();
                decimal percentage = (decimal)(count / (decimal)games.Count) * 100;
                newStat.Value2 = percentage.ToString("n1") + "%";
                stats.Add(newStat);
            }

            // Create "Total" stat
            // dont really like this
            //var total = new Stat();
            //total.Name = "Total";
            //total.Value = games.Count.ToString();
            //stats.Add(total);

            // order
            var ordered = stats.OrderByDescending(x => Convert.ToInt32(x.Value)).ToList();

            return new ObservableCollection<Stat>(ordered);
        }

        public void UpdateGapStats(PlayedGameList games)
        {
            decimal avgGap = 0m;
            decimal origGap = 0m;
            if (games.Count != 0)
            {
                int totalGap = 0;
                int totalOrigGap = 0;

                for (int pg = 0; pg < games.Count; pg++)
                {
                    var game = games[pg];
                    //if(game.MatchingMedia?.YearReleased > 0)
                    //{
                    //    totalGap += game.DateAdded.Year - game.MatchingMedia.YearReleased;
                    //    totalOrigGap += game.DateAdded.Year - game.MatchingMedia.YearReleased;
                    //}

                    totalGap += game.Gap;
                    totalOrigGap += game.OgGap;

                }

                avgGap = (totalGap * 1m) / games.Count;
                origGap = (totalOrigGap * 1m) / games.Count;
            }


            Gap = avgGap.ToString("F1");
            OgGap = origGap.ToString("F1");
        }

        public ObservableCollection<Stat> UpdateMonthStats(PlayedGameList games)
        {
            Mouse.OverrideCursor = Cursors.Wait;

            // Month/Year Stats
            var monthStats = new ObservableCollection<Stat>();

            var monthCounts = new List<int>();
            for (int i = 0; i <= 12; i++)
                monthCounts.Add(0);

            int total = 0;

            for (int i = 0; i < games.Count; i++)
            {
                var game = games[i];

                if (game.ExactDate == 1)
                {
                    total++;
                    monthCounts[game.DateAdded.Month]++;
                }


            }
            monthCounts[0] = total;

            for (int i = 0; i < monthCounts.Count; i++)
            {
                var count = monthCounts[i];

                Stat stat = new Stat();

                if (i == 0)
                {
                    stat.Name = "Total: ";
                    stat.Value = count.ToString();
                }
                else
                {
                    var month = Utilities.General.GetMonthNameFromNumber(i);
                    stat.Name = $"{month}: ";
                    stat.Value = count.ToString();
                }

                monthStats.Add(stat);
            }

            Mouse.OverrideCursor = null;

            return monthStats;



        }

    }
}
