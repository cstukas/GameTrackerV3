using System;
using System.Collections.Generic;
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

    }

    public class StatsVM : BaseMediaVM
    {
        //******************************************
        // Commands
        //******************************************
        public ICommand GameClickedCommand { get; private set; }


        //******************************************
        // Properties
        //******************************************
        public MainVM ParentVM { get; set; }

        public StatGame SelectedMostPlayed { get; set; }
        public List<StatGame> MostPlayedGames { get; set; }




        //******************************************
        // Constructor
        //******************************************
        public StatsVM()
            : base()
        {
            this.GameClickedCommand = new DelegateCommand<object>(this.OnGameClicked);

            MostPlayedGames = new List<StatGame>();

            Load();
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

            var prevNum = -1;
            var max = 0;
            for (int i = 0; i < list.Count; i++)
            {
                var g = list[i];

                if (g.TimesBeat == 0) break;

                if(prevNum != g.TimesBeat)
                {
                    prevNum = g.TimesBeat;
                    max++;
                }

                if (max > 5)
                    break;


                MostPlayedGames.Add(g);





            }
            
         //   MostPlayedGames.AddRange(list);

        }


    }
}
