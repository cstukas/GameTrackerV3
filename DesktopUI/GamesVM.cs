using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GameBL;
using Prism.Commands;

namespace DesktopUI
{
    public class GamesVM : TabVMs.BaseMediaVM
    {
        public ICommand EditGameCommand { get; private set; }
        public ICommand MediaClickedCommand { get; private set; }


        private List<Game> gameList;
        public List<Game> GameList
        {
            get { return gameList; }
            set { gameList = value; OnPropertyChanged("GameList"); }
        }

        private ObservableCollection<Game> displayGameList;
        public ObservableCollection<Game> DisplayGameList
        {
            get { return displayGameList; }
            set { displayGameList = value; OnPropertyChanged("DisplayGameList"); }
        }

        public Game SelectedGame { get; set; }

        private string gameFilterText;
        public string GameFilterText
        {
            get { return gameFilterText; }
            set
            {
                gameFilterText = value;
                OnPropertyChanged("GameFilterText");

                DisplayGameList.Clear();

                for (int i = 0; i < GameList.Count; i++)
                {
                    var cm = GameList[i];

                    var good = cm.Name.ToLower().StartsWith(GameFilterText.ToLower());

                    if (good)
                    {
                        DisplayGameList.Add(cm);
                    }
                }


            }
        }

        public GamesVM()
        {
            this.EditGameCommand = new DelegateCommand<object>(this.OnEditGame);
            this.MediaClickedCommand = new DelegateCommand<object>(this.OnMediaClicked);

            GameList = Utilities.General.CloneList(LoadedData.AllGames);
            DisplayGameList = new ObservableCollection<Game>();

            for (int i = 0; i < GameList.Count; i++)
            {
                var game = GameList[i];
                DisplayGameList.Add(game);
            }

        }

        private void OnEditGame(object obj)
        {
            var geWindow = new EditGameWindow(SelectedGame);
            geWindow.ShowDialog();
        }

        private void OnMediaClicked(object obj)
        {

            var media = new Game();
            if (SelectedGame != null)
            {
                Mouse.OverrideCursor = Cursors.Wait;
                var nextWindow = new ReadGameWindow(null, SelectedGame);
                nextWindow.Show();

            }
         


        }
    }
}
