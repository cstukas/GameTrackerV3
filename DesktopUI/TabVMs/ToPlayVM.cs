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




        //******************************************
        // Constructor
        //******************************************
        public ToPlayVM(MainVM parentVM) 
            : base(parentVM)
        {
            this.EditSelectedCommand = new DelegateCommand<object>(this.OnEditSelected);

            ToPlayGamesList = new List<CollectionGame>();
        }


        //******************************************
        // Methods
        //******************************************
        public void RefreshData(bool onlyFriends = false)
        {
            var toPlay = LoadedData.MyCollection.Where(x => x.Finished == 0 && x.Playing == 1 && x.Own == 1).OrderBy(x => x.MatchingMedia.Name).ToList();
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


    }
}
