
using System;
using System.ComponentModel;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using GameBL;

namespace DesktopUI
{
    class SplashVM : Utilities.BaseClasses.NotifyPropertyChanged, IDisposable
    {
        //******************************************
        // Properties
        //******************************************
        public static SynchronizationContext WindowContext { get; set; }
        private readonly BackgroundWorker worker;
        public event EventHandler CloseWindowEvent;

        private string loadingStatus = "";
        public string LoadingStatus
        {
            get { return loadingStatus; }
            set
            {
                loadingStatus = value;
                OnPropertyChanged("LoadingStatus");
            }
        }

        public TabVMs.CollectionMediaVM LoadedCollectionVM { get; set; }
        public TabVMs.FriendsVM LoadedFriendsVM { get; set; }
        public TabVMs.StatsVM LoadedStatsVM { get; set; }

        //******************************************
        // Constructor
        //******************************************
        public SplashVM()
        {
            WindowContext = SynchronizationContext.Current;
            Mouse.OverrideCursor = Cursors.AppStarting;

            worker = new BackgroundWorker();
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            worker.RunWorkerAsync();
        }

        //******************************************
        // Methods
        //******************************************
        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            LoadingStatus = "Checking for Updates";
            GameBL.Globals.UpdateAvailable = GameBL.UpdateAvailable.UpdateIsAvailable();

            LoadingStatus = "Loading";
            GameBL.Globals.GenreList = new GenreList();

            LoadingStatus = "Loading Friends";
            Utilities.UserUtils.LoadAllUsers();
            Utilities.UserUtils.LoadFriends();

            LoadingStatus = "Loading Games";
            LoadedData.LoadAllGames();

            LoadingStatus = "Loading User";
            var user = Utilities.UserUtils.CurrentUser;
            LoadedData.Load(user.UserKey);

            LoadingStatus = "Loading Collection";
            LoadedCollectionVM = new TabVMs.CollectionMediaVM();
            LoadedCollectionVM.LoadAll(user);

            LoadingStatus = "Loading Info";
            LoadedFriendsVM = new TabVMs.FriendsVM();

            LoadingStatus = "Loading Stats";
            LoadedStatsVM = new TabVMs.StatsVM();

            LoadingStatus = "Done";

        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Mouse.OverrideCursor = null;
            var nextWindow = new MainWindow(LoadedCollectionVM, LoadedFriendsVM, LoadedStatsVM);
            nextWindow.Show();
            CloseWindowEvent?.Invoke(this, EventArgs.Empty);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            worker.Dispose();
        }
    }
}
