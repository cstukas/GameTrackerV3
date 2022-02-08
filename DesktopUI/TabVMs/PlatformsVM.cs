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

    public class PlatformsVM : BaseMediaVM
    {
        //******************************************
        // Commands
        //******************************************
        public ICommand EditSelectedCommand { get; private set; }
        public ICommand AddNewCommand { get; private set; }


        //******************************************
        // Properties
        //******************************************
        private ObservableCollection<Platform> platformList;
        public ObservableCollection<Platform> PlatformList
        {
            get { return platformList; }
            set { platformList = value; OnPropertyChanged("PlatformList"); }
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
        public PlatformsVM(MainVM parentVM)
            : base(parentVM)
        {
            this.EditSelectedCommand = new DelegateCommand<object>(this.OnEditSelected);
            this.AddNewCommand = new DelegateCommand<object>(this.OnAddNew);

            PlatformList = new ObservableCollection<Platform>();
            var ordered = LoadedData.PlatformList.OrderByDescending(x => x.YearReleased).ToList();
            for (int i = 0; i < ordered.Count; i++)
            {
                PlatformList.Add(ordered[i]);
            }

        }


        //******************************************
        // Methods
        //******************************************
        public void RefreshData(bool onlyFriends = false)
        {
            //var toPlay = LoadedData.MyCollection.Where(x => x.Finished == 0 && x.Playing == 1 && x.Own == 1).OrderBy(x => x.MatchingMedia.Name).ToList();
            //ToPlayGamesList = Utilities.General.CloneList(toPlay);
            //GameCount = ToPlayGamesList.Count;
        }

        private void OnEditSelected(object obj)
        {
            //if (SelectedPlayedGame != null)
            //{
            //    ParentVM.OpenAddToPlayedWindow(SelectedPlayedGame);
            //}
        }

        private void OnAddNew(object obj)
        {
            var newWindow = new HelperUI.AddPlatformWIndow();
            newWindow.ShowDialog();
        }
    }
}
