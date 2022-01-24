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

namespace DesktopUI
{
    public class ReadGameVM : Utilities.BaseClasses.NotifyPropertyChanged
    {
        public event EventHandler CloseWindowEvent;

        public ICommand ViewUserCommand { get; private set; }

        public MainVM ParentVM { get; set; }

        public string WindowTitle { get; set; }

        private Game game;
        public Game Game
        {
            get { return game; }
            set { game = value; OnPropertyChanged("Game"); }
        }

        private Game ogGame;
        public Game OgGame
        {
            get { return ogGame; }
            set { ogGame = value; OnPropertyChanged("OgGame"); }
        }

        public List<string> RemakeGames { get; set; }

        private PlayedGameList allPlayedGames;
        public PlayedGameList AllPlayedGames
        {
            get { return allPlayedGames; }
            set { allPlayedGames = value; OnPropertyChanged("AllPlayedGames"); }
        }


        private List<PlayedGame> playedGames;
        public List<PlayedGame> PlayedGames
        {
            get { return playedGames; }
            set { playedGames = value; OnPropertyChanged("PlayedGames"); }
        }

        private int timesBeat;
        public int TimesBeat
        {
            get { return timesBeat; }
            set { timesBeat = value; OnPropertyChanged("TimesBeat"); }
        }

        private bool onlyShowMine;
        public bool OnlyShowMine
        {
            get { return onlyShowMine; }
            set 
            { 
                onlyShowMine = value; 
                OnPropertyChanged("OnlyShowMine");

                LoadEntries();
            }
        }

        public ReadGameVM(MainVM parentVM, Game game)
        {
            this.ViewUserCommand = new DelegateCommand<object>(this.OnViewUser);

            ParentVM = parentVM;
            Game = game;

            PlayedGames = new List<PlayedGame>();

            var gameKey = 0;
            if (Game.RemakeOf > 0)
                gameKey = Game.RemakeOf;
            else
                gameKey = Game.GameKey;

            OgGame = Game.LoadGame(gameKey);
            var allAlike = PlayedGameList.GetAllAlikeGames(OgGame.Name, OgGame.GameKey);
            AllPlayedGames = PlayedGameList.LoadAllPlayed(allAlike, false);

            // Set List of Remakes


            RemakeGames = new List<string>();
            for (int i = 0; i < allAlike.Count; i++)
            {
                var aGame = allAlike[i];

                // dont include rom hacks
                if (aGame.RemakeType == 5)
                    continue;

                if (aGame.GameKey != OgGame.GameKey)
                {
                    string remakeType = "";
                    if (aGame.RemakeType > 0)
                        remakeType = " " + RemakeTypes.KeyToName(aGame.RemakeType);
                    
                    var displayStr = $"{aGame.Name} ({aGame.YearReleased}) - {Platform.KeyToName(aGame.Platform)}{remakeType}";
                    RemakeGames.Add(displayStr);
                }
            }


            OnlyShowMine = false;

            WindowTitle = "";
        }

        public void LoadEntries()
        {
            Mouse.OverrideCursor = Cursors.Wait;

            if (Game != null)
            {
                if (OnlyShowMine)
                    PlayedGames = AllPlayedGames.Where(x => x.UserKey == Utilities.UserUtils.CurrentUser.UserKey).ToList();
                else
                    PlayedGames = AllPlayedGames.ToList();
            }
            else
            {
                MessageBox.Show("Game could not be loaded. Contact Admin");
            }

            TimesBeat = 0;
            for (int i = 0; i < PlayedGames.Count; i++)
            {
                var pg = PlayedGames[i];
                var beat = LoadedData.PercentageList.FirstOrDefault(x => x.ItemKey == pg.PercentCompleted).Beat;
                TimesBeat += beat;
            }

            OnPropertyChanged("PlayedGames");

            Mouse.OverrideCursor = null;
        }

        private void OnViewUser(object obj)
        {
            var userKey = (int)obj;
            ParentVM.ViewUser(userKey);

            CloseWindowEvent?.Invoke(null, EventArgs.Empty);
        }

    }
}
