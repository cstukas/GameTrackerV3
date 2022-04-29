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

    public class ToPlayVM : BaseMediaVM
    {
        //******************************************
        // Commands
        //******************************************
        public ICommand EditSelectedCommand { get; private set; }
        public ICommand ProgressCommand { get; private set; }
        //public ICommand MediaClickedCommand { get; private set; }


        //******************************************
        // Properties
        //******************************************
        private List<CollectionGame> toPlayGamesList;
        public List<CollectionGame> ToPlayGamesList
        {
            get { return toPlayGamesList; }
            set { toPlayGamesList = value; OnPropertyChanged("ToPlayGamesList"); }
        }

        private int gameCount;
        public int GameCount
        {
            get { return gameCount; }
            set { gameCount = value; OnPropertyChanged("GameCount"); }
        }

        //private CollectionGame selectedGameToPlay;

        //public CollectionGame SelectedGameToPlay
        //{
        //    get { return selectedGameToPlay; }
        //    set { selectedGameToPlay = value; OnPropertyChanged("SelectedGameToPlay"); }
        //}



        //******************************************
        // Constructor
        //******************************************
        public ToPlayVM(MainVM parentVM) 
            : base(parentVM)
        {
            this.EditSelectedCommand = new DelegateCommand<object>(this.OnEditSelected);
            this.ProgressCommand = new DelegateCommand<object>(this.OnProgress);
            //this.MediaClickedCommand = new DelegateCommand<object>(this.OnMediaClicked);

            ToPlayGamesList = new List<CollectionGame>();
        }


        //******************************************
        // Methods
        //******************************************
        public void RefreshData(bool onlyFriends = false)
        {
            var toPlay = LoadedData.MyCollection.Where(x => x.Finished == 0 && x.Playing == 1 && x.Own == 1).OrderByDescending(x => x.PercentBeaten).ToList();
            ToPlayGamesList = Utilities.General.CloneList(toPlay);
            GameCount = ToPlayGamesList.Count;
        }

        private void OnEditSelected(object obj)
        {
            //if (SelectedPlayedGame != null)
            //{
            //    ParentVM.OpenAddToPlayedWindow(SelectedPlayedGame);
            //}
        }
        private void OnProgress(object obj)
        {
            if(SelectedCollectionMedia != null)
            {
                var para = obj.ToString();
                if (para.ToLower() == "up")
                {
                    Console.WriteLine("up");

                    if (SelectedCollectionMedia.PercentBeaten < 3)
                    {
                        SelectedCollectionMedia.PercentBeaten++;
                        SelectedCollectionMedia.UpdatePercentBeaten();
                    }


                }
                else if (para.ToLower() == "down")
                {
                    if (SelectedCollectionMedia.PercentBeaten > 0)
                    {
                        SelectedCollectionMedia.PercentBeaten--;
                        SelectedCollectionMedia.UpdatePercentBeaten();

                    }
                }
            }

        }

        //private void OnMediaClicked(object obj)
        //{


        //}


    }
}
