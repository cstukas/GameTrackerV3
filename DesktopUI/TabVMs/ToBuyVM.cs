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

    public class ToBuyVm : BaseMediaVM
    {
        //******************************************
        // Commands
        //******************************************
        public ICommand EditSelectedCommand { get; private set; }


        //******************************************
        // Properties
        //******************************************
        private List<CollectionGame> toBuyGamesList;
        public List<CollectionGame> ToBuyGamesList
        {
            get { return toBuyGamesList; }
            set { toBuyGamesList = value; OnPropertyChanged("ToBuyGamesList"); }
        }

        private int gameCount;
        public int GameCount
        {
            get { return gameCount; }
            set { gameCount = value; OnPropertyChanged("GameCount"); }
        }

        private int totalPrice;
        public int TotalPrice
        {
            get { return totalPrice; }
            set { totalPrice = value; OnPropertyChanged("TotalPrice"); }
        }


        //******************************************
        // Constructor
        //******************************************
        public ToBuyVm(MainVM parentVM)
            : base(parentVM)
        {
            this.EditSelectedCommand = new DelegateCommand<object>(this.OnEditSelected);

            ToBuyGamesList = new List<CollectionGame>();
        }


        //******************************************
        // Methods
        //******************************************
        public void RefreshData(bool onlyFriends = false)
        {
            var games = ParentVM.CollectionVM.EntireCollection.Where(x => x.Own == 0 && x.Buying == 1 && x.OwnDigitally == 0).OrderBy(x => x.MatchingMedia.Name).ToList();

            ToBuyGamesList = new List<CollectionGame>();
            TotalPrice = 0;
            for (int i = 0; i < games.Count; i++)
            {
                var game = games[i];

         //       var matches = ParentVM.CollectionVM.EntireCollection.Where(x => x.MatchingMedia.Name == game.MatchingMedia.Name || x.MatchingMedia.RemakeOf == game.GameKey).ToList();
                var matches = CollectionGame.GetAllAlikeGames(game.MatchingMedia, ParentVM.CollectionVM.EntireCollection);

                var own = false;
                for (int m = 0; m < matches.Count; m++)
                {
                    var match = matches[m];
                    var collGame = ParentVM.CollectionVM.EntireCollection.FirstOrDefault(x => x.GameKey == match.GameKey);
                    if(collGame != null)
                    {
                        if (collGame.Own == 1)
                        {
                            own = true;
                            break;
                        }

                    }

                }

                if (!ToBuyGamesList.Contains(game) && !own)
                {
                    ToBuyGamesList.Add(game);
                    TotalPrice += game.MatchingMedia.Price;
                }
            }

            GameCount = ToBuyGamesList.Count;
        }

        private void OnEditSelected(object obj)
        {
            if (SelectedCollectionMedia != null)
            {
                ParentVM.OpenAddToCollectionWindow(SelectedCollectionMedia);
            }
        }


    }
}
