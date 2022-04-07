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

    public class StatGame
    {
        public int GameKey { get; set; }
        public string Name { get; set; }
        public int TimesBeat { get; set; }
        public int Year { get; set; }
        public string Platform{ get; set; }

    }


    public class StatsVM : BaseMediaVM
    {
        //******************************************
        // Commands
        //******************************************
        public ICommand GameClickedCommand { get; private set; }
        public ICommand TopGameClickedCommand { get; private set; }
        public ICommand OnThisDayGameClickedCommand { get; private set; }


        //******************************************
        // Properties
        //******************************************
        public MainVM ParentVM { get; set; }

        public StatGame SelectedMostPlayed { get; set; }
        public List<StatGame> MostPlayedGames { get; set; }
        public ObservableCollection<TopGame> TopGames { get; set; }

        public StatGame SelectedOnThisDay { get; set; }
        public ObservableCollection<StatGame> OnThisDayGames { get; set; }

        private DateTime onThisDayDate;
        public DateTime OnThisDayDate
        {
            get { return onThisDayDate; }
            set 
            { 
                onThisDayDate = value; 
                OnPropertyChanged("OnThisDayDate"); 
                LoadOnThisDayGames();
            }
        }

        private decimal finishedGames;
        public decimal FinishedGames
        {
            get { return finishedGames; }
            set { finishedGames = value; OnPropertyChanged("FinishedGames"); }
        }

        private decimal totalGames;
        public decimal TotalGames
        {
            get { return totalGames; }
            set { totalGames = value; OnPropertyChanged("TotalGames"); }
        }

        private decimal beatenPercent;
        public decimal BeatenPercent
        {
            get { return beatenPercent; }
            set { beatenPercent = value; OnPropertyChanged("BeatenPercent"); }
        }

        private string beatenPercentString;
        public string BeatenPercentString
        {
            get { return beatenPercentString; }
            set { beatenPercentString = value; OnPropertyChanged("BeatenPercentString"); }
        }



        public List<string> TopRatedGamesOptions { get; set; }

        private string selectedTopRatedGameOption;
        public string SelectedTopRatedGameOption
        {
            get { return selectedTopRatedGameOption; }
            set 
            { 
                selectedTopRatedGameOption = value; 
                OnPropertyChanged("SelectedTopRatedGameOption");

                LoadTopGames();

            }
        }


        //******************************************
        // Constructor
        //******************************************
        public StatsVM()
            : base()
        {
            this.GameClickedCommand = new DelegateCommand<object>(this.OnGameClicked);
            this.TopGameClickedCommand = new DelegateCommand<object>(this.OnTopGameClicked);
            this.OnThisDayGameClickedCommand = new DelegateCommand<object>(this.OnOnThisDayGameClicked);

            MostPlayedGames = new List<StatGame>();
            OnThisDayGames = new ObservableCollection<StatGame>();

            OnThisDayDate = DateTime.Now;

            Load();


            TopRatedGamesOptions = new List<string>();
            for (int i = 90; i < 100; i++)
            {
                TopRatedGamesOptions.Insert(0,$"{i}+");
            }

            SelectedTopRatedGameOption = TopRatedGamesOptions[5];
        }
        
        public void Load()
        {
            LoadMostPlayedGames();
        }

        //******************************************
        // Methods
        //******************************************
        private void OnGameClicked(object obj)
        {
            if (SelectedMostPlayed != null)
            {
                var game = LoadedData.AllGames.FirstOrDefault(x => x.GameKey == SelectedMostPlayed.GameKey);
                if(game != null)
                    ParentVM.ViewMedia(game);
            }
        }

        private void OnTopGameClicked(object obj)
        {
            if (SelectedTopGame != null)
            {
                var game = LoadedData.AllGames.FirstOrDefault(x => x.GameKey == SelectedTopGame.GameKey);
                if (game != null)
                    ParentVM.ViewMedia(game);
            }
        }

        private void OnOnThisDayGameClicked(object obj)
        {
            if (SelectedOnThisDay != null)
            {
                var game = LoadedData.AllGames.FirstOrDefault(x => x.GameKey == SelectedOnThisDay.GameKey);
                if (game != null)
                    ParentVM.ViewMedia(game);
            }
        }

        public void LoadTopGames()
        {
            TopGames = new ObservableCollection<TopGame>();

            for (int i = 0; i < LoadedData.TopGames.Count; i++)
            {
                var g = LoadedData.TopGames[i];

                var sRating = SelectedTopRatedGameOption.Replace("+", "");
                var selectedRating = Convert.ToInt32(sRating);

                if (g.Rating < selectedRating) continue;

                var game = LoadedData.AllGames.FirstOrDefault(x => x.GameKey == g.GameKey);
                if (game != null)
                {
                    g.Name = game.Name;
                }
                else
                {
                    game = Game.LoadGame(g.GameKey);
                    g.Name = game?.Name;

                }

                var playedGames = LoadedData.MyPlayedGames.Where(x => x.GameKey == game.GameKey || x.GameKey == game.RemakeOf || x.MatchingMedia.Name == game.Name || x.MatchingMedia.RemakeOf == game.GameKey).ToList();
                var finished = false;
                for (int k = 0; k < playedGames.Count; k++)
                {
                    var pg = playedGames[k];
                    if (pg.PercentCompleted == 1 || pg.PercentCompleted == 2 || pg.PercentCompleted == 3 || pg.PercentCompleted == 4
                        || pg.PercentCompleted == 7 || pg.PercentCompleted == 9 || pg.PercentCompleted == 10)
                    {
                        finished = true;
                    }
                }

                g.Platform = game.Platform;
                g.Finished = finished;

                TopGames.Add(g);
            }

            TotalGames = TopGames.Count;
            FinishedGames = TopGames.Count(x => x.Finished);

            if(TotalGames> 0)
            {
                decimal result = FinishedGames / TotalGames;
                BeatenPercent = Math.Round((Decimal)result, 2) * 100;




                BeatenPercentString = BeatenPercent.ToString();
                if(BeatenPercentString.Contains('.'))
                {
                    BeatenPercentString = BeatenPercentString.Split('.')[0];
                }


            }

            OnPropertyChanged("TopGames");

        }

        public void LoadOnThisDayGames()
        {
            OnThisDayGames.Clear();
            var games = LoadedData.MyPlayedGames.Where(x => x.ExactDate == 1 && x.DateAdded.Month == OnThisDayDate.Month && x.DateAdded.Day == OnThisDayDate.Day).ToList();

            for (int i = 0; i < games.Count; i++)
            {
                var game = games[i];

                var finished = LoadedData.PercentageList.FirstOrDefault(x => x.ItemKey == game.PercentCompleted)?.Finished;
                if(finished == 1)
                {
                    var stat = new StatGame();
                    stat.GameKey = game.GameKey;
                    stat.Name = game.MatchingMedia.Name;
                    stat.Year = game.DateAdded.Year;
                    stat.Platform = game.PlatformInfo;

                    OnThisDayGames.Add(stat);
                }

            }


        }

        public void LoadMostPlayedGames()
        {
            MostPlayedGames.Clear();

            var list = new List<StatGame>();
            var beaten = LoadedData.MyPlayedGames.Where(x => x.Beaten == 1).ToList();
            for (int i = 0; i < beaten.Count; i++)
            {
                var game = beaten[i];
                var statGame = new StatGame();

                if(game.MatchingMedia.RemakeOf > 0) // is a remake
                {
                    statGame.GameKey = game.MatchingMedia.RemakeOf;

                    var ogName = LoadedData.AllGames.FirstOrDefault(x => x.GameKey == statGame.GameKey);
                    if(ogName != null)
                    {
                        statGame.Name = ogName.Name;
                    }
                }
                else
                {
                    statGame.GameKey = game.GameKey;
                    statGame.Name = game.MatchingMedia.Name;
                }

                var count = beaten.Count(x => x.GameKey == statGame.GameKey || x.MatchingMedia.RemakeOf == statGame.GameKey);
                statGame.TimesBeat = count;

                var dup = list.Where(x => x.GameKey == statGame.GameKey).ToList();
                if (dup.Count == 0)
                    list.Add(statGame);
            }

            list = list.OrderByDescending(x => x.TimesBeat).ToList();



            //var prevNum = -1;
            //var max = 0;
            for (int i = 0; i < list.Count; i++)
            {
                var g = list[i];

                if(g.TimesBeat > 2)
                {
                    MostPlayedGames.Add(g);

                }


                //if (g.TimesBeat <= 1) break;


                ////if (prevNum != g.TimesBeat)
                ////{
                ////    prevNum = g.TimesBeat;
                ////    max++;
                ////}

                ////if (max > 5)
                ////    break;

            }

         //   MostPlayedGames.AddRange(list);

        }




    }
}
