using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GameBL;
using Prism.Commands;

namespace DesktopUI.TabVMs
{
    public class SeriesVM : BaseMediaVM
    {
        //******************************************
        // Commands
        //******************************************
        public ICommand GameClickedCommand { get; private set; }
        public ICommand SeriesOrderCommand { get; private set; }


        //******************************************
        // Properties
        //******************************************
        public Series SeriesList { get; set; }

        private ObservableCollection<PlayedGame> seriesPlayedGames;
        public ObservableCollection<PlayedGame> SeriesPlayedGames
        {
            get { return seriesPlayedGames; }
            set { seriesPlayedGames = value; OnPropertyChanged("SeriesPlayedGames"); }
        }


        private SeriesItem selectedSeries;
        public SeriesItem SelectedSeries
        {
            get { return selectedSeries; }
            set 
            { 
                selectedSeries = value; 
                OnPropertyChanged("SelectedSeries");

                var seriesGames = LoadedData.AllGames.Where(x => x.SeriesKey == SelectedSeries.SeriesKey).OrderBy(x => x.SeriesOrderNum).ToList();
                ReloadSeriesGrid(seriesGames);

                SeriesPlayedGames.Clear();

                int beatCount = 0;
                int ownCount = 0;

                var allSeriesGames = new List<Game>();

                for (int k = 0; k < seriesGames.Count; k++)
                {
                    var sGame = seriesGames[k];
                    var otherVersions = LoadedData.AllGames.Where(x => x.Name == sGame.Name || x.RemakeOf == sGame.GameKey).ToList();

                    foreach (var item in otherVersions)
                    {
                        allSeriesGames.Add(item);
                       
                    }
                }

                var allGames = new List<PlayedGame>();
                for (int i = 0; i < allSeriesGames.Count; i++)
                {
                    var asGame = allSeriesGames[i];
                    var pGames = LoadedData.MyPlayedGames.Where(x => x.GameKey == asGame.GameKey).ToList();
                    for (int j = 0; j < pGames.Count; j++)
                    {
                        var pgame = pGames[j];
                        allGames.Add(pgame);
                        if (pgame.Beaten == 1) beatCount++;
                    }

                }

                allGames = allGames.OrderByDescending(x => x.DateAdded).ToList();
                for (int j = 0; j < allGames.Count; j++)
                {
                    SeriesPlayedGames.Add(allGames[j]);
                }

                TotalGames = seriesGames.Count;

                GamesBeat = beatCount;
                BeatPercentage = (beatCount / totalGames) ;
                OnPropertyChanged("BeatPercentage");




            }
        }   

        private ObservableCollection<SeriesGame> seriesGames;
        public ObservableCollection<SeriesGame> SeriesGames
        {
            get { return seriesGames; }
            set { seriesGames = value; OnPropertyChanged("SeriesGames"); }
        }

        private SeriesGame selectedSeriesGame;
        public SeriesGame SelectedSeriesGame
        {
            get { return selectedSeriesGame; }
            set { selectedSeriesGame = value; OnPropertyChanged("SelectedSeriesGame"); }
        }

        private int gamesBeat;
        public int GamesBeat
        {
            get { return gamesBeat; }
            set { gamesBeat = value; OnPropertyChanged("GamesBeat"); }
        }

        private int gamesOwn;
        public int GamesOwn
        {
            get { return gamesOwn; }
            set { gamesOwn = value; OnPropertyChanged("GamesOwn"); }
        }

        private double beatPercentage;
        public double BeatPercentage
        {
            get { return beatPercentage; }
            set { beatPercentage = value; OnPropertyChanged("BeatPercentage"); }
        }

        private double ownPercentage;
        public double OwnPercentage
        {
            get { return ownPercentage; }
            set { ownPercentage = value; OnPropertyChanged("OwnPercentage"); }
        }

        private int totalGames;
        public int TotalGames
        {
            get { return totalGames; }
            set { totalGames = value; OnPropertyChanged("TotalGames"); }
        }


        //******************************************
        // Constructor
        //******************************************
        public SeriesVM(MainVM parentVM)
            : base(parentVM)
        {
            this.GameClickedCommand = new DelegateCommand<object>(this.OnGameClicked);
            this.SeriesOrderCommand = new DelegateCommand<object>(this.OnSeriesOrder);

            SeriesList = LoadedData.SeriesList;
            SeriesGames = new ObservableCollection<SeriesGame>();
            SeriesPlayedGames = new ObservableCollection<PlayedGame>();
        }


        //******************************************
        // Methods
        //******************************************
        private void OnGameClicked(object obj)
        {
            ParentVM.ViewMedia((Game)SelectedSeriesGame);
        }
        private void OnSeriesOrder(object obj)
        {
            var para = obj.ToString();

            if(SelectedSeriesGame != null)
            {
                if(para == "up")
                    SelectedSeriesGame.SeriesOrderNum += 100;
                else if (para == "down")
                    SelectedSeriesGame.SeriesOrderNum -= 100;

                SeriesGame.UpdateOrderNum(SelectedSeriesGame.SeriesOrderNum, SelectedSeriesGame.GameKey);
                ReloadSeriesGrid(null);

            }
        }

        public void ReloadSeriesGrid(List<Game> seriesGames)
        {
            List<Game> games;
            if(seriesGames == null)
            {
                games = LoadedData.AllGames.Where(x => x.SeriesKey == SelectedSeries.SeriesKey).OrderBy(x => x.SeriesOrderNum).ToList();
            }
            else
            {
                games = seriesGames;
            }

            SeriesGames.Clear();
            var twoDGames = new List<SeriesGame>();
            var threeDGames = new List<SeriesGame>();

            for (int i = 0; i < games.Count; i++)
            {
                var game = games[i];
                var seriesGame = Utilities.General.Map<Game, SeriesGame>(game);

                var collGames = LoadedData.MyCollection.Where(x => x.GameKey == game.GameKey || x.MatchingMedia.RemakeOf == game.GameKey).ToList();

                

                seriesGame.Own = 0;
                seriesGame.OwnDigitally = 0;
                seriesGame.TimesBeat = 0;
                for (int j = 0; j < collGames.Count; j++)
                {
                    var collGame = collGames[j];
                    if (collGame.Own > 0)
                        seriesGame.Own = 1;

                    if (collGame.OwnDigitally > 0)
                        seriesGame.OwnDigitally = 1;

                    seriesGame.TimesBeat = collGame.TimesBeat;
                }

                seriesGame.Color = "White";

                if (seriesGame.Own > 0 || seriesGame.OwnDigitally > 0)
                    seriesGame.Color = "LightGreen";

                if (seriesGame.TimesBeat > 0)
                    seriesGame.Color = "LightBlue";
                else
                {
                    // check if you played it even if you dont own it
                    var played = LoadedData.MyPlayedGames.Where(x => x.GameKey == game.GameKey || x.MatchingMedia.RemakeOf == game.GameKey).ToList();
                    for (int p = 0; p < played.Count; p++)
                    {
                        var pGame = played[p];
                        if(pGame.Beaten == 1)
                        {
                            seriesGame.Color = "LightBlue";
                            break;
                        }
                    }

                }

                if (seriesGame.Own == 0 && seriesGame.OwnDigitally == 0 && seriesGame.TimesBeat > 0)
                    seriesGame.Color = "LightSkyBlue";

                if (seriesGame.SeriesType == 1)
                    twoDGames.Add(seriesGame);
                else
                    threeDGames.Add(seriesGame);
            }

            // add to main list
            for (int twoD = 0; twoD < twoDGames.Count; twoD++)
            {
                SeriesGames.Add(twoDGames[twoD]);
            }

            if(twoDGames.Count > 0 && threeDGames.Count > 0)
            {
                var emptyGame = new SeriesGame();
                SeriesGames.Add(emptyGame);
            }

            for (int threeD = 0; threeD < threeDGames.Count; threeD++)
            {
                SeriesGames.Add(threeDGames[threeD]);
            }
        }


    }
}
