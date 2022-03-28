using GameBL;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DesktopUI.TabVMs
{
    public class BaseMediaVM : Utilities.BaseClasses.NotifyPropertyChanged
    {
        public ICommand MediaClickedCommand { get; private set; }


        public MainVM ParentVM { get; set; }

        private PlayedGame selectedPlayedGame;
        public PlayedGame SelectedPlayedGame
        {
            get { return selectedPlayedGame; }
            set { selectedPlayedGame = value; OnPropertyChanged("SelectedPlayedGame"); }
        }

        private CollectionGame selectedCollectionMedia;
        public CollectionGame SelectedCollectionMedia
        {
            get { return selectedCollectionMedia; }
            set { selectedCollectionMedia = value; OnPropertyChanged("SelectedCollectionMedia"); }
        }


        private TopGame selectedTopGame;
        public TopGame SelectedTopGame
        {
            get { return selectedTopGame; }
            set { selectedTopGame = value; OnPropertyChanged("SelectedTopGame"); }
        }

        public void Init()
        {
            this.MediaClickedCommand = new DelegateCommand<object>(this.OnMediaClicked);
        }

        public BaseMediaVM()
        {
            Init();
        }

        public BaseMediaVM(MainVM parentVM)
        {
            Init();
            ParentVM = parentVM;
        }


        private void OnMediaClicked(object obj)
        {

            var media = new Game();
            if (SelectedPlayedGame != null)
            {
                media = SelectedPlayedGame.MatchingMedia;
            }
            else if (SelectedCollectionMedia != null)
            {
                media = SelectedCollectionMedia.MatchingMedia;
            }
            else
            {

            }

            ParentVM.ViewMedia(media);
        }


    }
}
