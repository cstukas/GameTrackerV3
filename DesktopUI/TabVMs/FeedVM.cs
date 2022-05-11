using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using GameBL;
using Prism.Commands;

namespace DesktopUI.TabVMs
{
    public class FeedVM : Utilities.BaseClasses.NotifyPropertyChanged
    {
        public int InitialMediaCountToLoad => 50;

        private readonly BackgroundWorker feedWorker;
        private SynchronizationContext synchronizationContext;

        public MainVM ParentVM { get; set; }

        public PlayedGameList FeedList { get; set; }

        private int latestPlayedKey = -1;
        public int LatestPlayedKey
        {
            get { return latestPlayedKey; }
            set { latestPlayedKey = value; OnPropertyChanged("LatestPlayedKey");  }
        }

        private DateTime latestDateAdded;
        public DateTime LatestDateAdded
        {
            get { return latestDateAdded; }
            set { latestDateAdded = value; OnPropertyChanged("LatestDateAdded"); }
        }

        public FeedVM(MainVM parentVM)
        {
            synchronizationContext = SynchronizationContext.Current;

            ParentVM = parentVM;

            FeedList = new PlayedGameList();
            FeedList = PlayedGameList.LoadGame(-1, true, InitialMediaCountToLoad, $" {Utilities.UserUtils.GetFriendsSql(Utilities.UserUtils.CurrentUser)} ORDER BY DateAdded DESC", true,"",0,0);
            if (FeedList.Count > 0)
            {
                LatestPlayedKey = FeedList[0].PlayedKey;
                LatestDateAdded = FeedList[0].DateAdded;
            }

            feedWorker = new BackgroundWorker();
            feedWorker.DoWork += feedWorker_DoWork;
            var timer = new System.Timers.Timer(60000);
            timer.Elapsed += timer_Elapsed;
            timer.Start();

        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!feedWorker.IsBusy)
                feedWorker.RunWorkerAsync();
        }

        public void UpdateFeed()
        {
            var currentUser = Utilities.UserUtils.CurrentUser;
            if (currentUser.Friends == "") return;

            var sql = $" AND DateAdded > '{LatestDateAdded}' {Utilities.UserUtils.GetFriendsSql(currentUser)} ORDER BY DateAdded DESC";
            var newEntries = new PlayedGameList();
            newEntries = PlayedGameList.LoadGame(-1, true, -1, sql, true,"",0,0);

            for (int i = newEntries.Count - 1; i >= 0; i--)
            {
                var entry = newEntries[i];
                synchronizationContext.Send(x => FeedList.Insert(0, entry), null);
            }

            if (FeedList.Count > 0)
            {
                LatestPlayedKey = FeedList[0].PlayedKey;
                LatestDateAdded = FeedList[0].DateAdded;
            }
        }


        void feedWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            UpdateFeed();
        }


    }
}
