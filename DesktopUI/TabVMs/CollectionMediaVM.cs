using GameBL;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DesktopUI.TabVMs
{
    public class CollectionMediaVM : BaseMediaVM
    {
        public ICommand RefreshCommand { get; private set; }
        public ICommand EditSelectedCommand { get; private set; }

        public List<Platform> PlatformList { get; set; }
        public ObservableCollection<Stat> YearReleasedStats { get; set; }


        private Platform selectedPlatform;
        public Platform SelectedPlatform
        {
            get { return selectedPlatform; }
            set { selectedPlatform = value; OnPropertyChanged("SelectedPlatform"); }
        }


        private int mediaCount;
        public int MediaCount
        {
            get { return mediaCount; }
            set { mediaCount = value; OnPropertyChanged("MediaCount"); }
        }

        private Utilities.User selectedUser;
        public Utilities.User SelectedUser
        {
            get { return selectedUser; }
            set { selectedUser = value; OnPropertyChanged("SelectedUser"); }
        }

        private CollectionGameList myCollection;
        public CollectionGameList MyCollection
        {
            get { return myCollection; }
            set { myCollection = value; OnPropertyChanged("MyCollection"); }
        }


        private CollectionGameList collectionMediaList;
        public CollectionGameList CollectionMediaList
        {
            get { return collectionMediaList; }
            set { collectionMediaList = value; OnPropertyChanged("CollectionMediaList"); }
        }

        private CollectionGameList displayCollectionList;
        public CollectionGameList DisplayCollectionList
        {
            get { return displayCollectionList; }
            set { displayCollectionList = value; OnPropertyChanged("DisplayCollectionList"); }
        }


        private bool ownedGames;
        public bool OwnedGames
        {
            get { return ownedGames; }
            set { ownedGames = value; OnPropertyChanged("OwnedGames"); }
        }

        private bool digitalGames;
        public bool DigitalGames
        {
            get { return digitalGames; }
            set { digitalGames = value; OnPropertyChanged("DigitalGames"); }
        }

        private bool romGames;
        public bool RomGames
        {
            get { return romGames; }
            set { romGames = value; OnPropertyChanged("RomGames"); }
        }


        private bool extraGames;
        public bool ExtraGames
        {
            get { return extraGames; }
            set { extraGames = value; OnPropertyChanged("ExtraGames"); }
        }

        private bool dontOwnedGames;
        public bool DontOwnedGames
        {
            get { return dontOwnedGames; }
            set { dontOwnedGames = value; OnPropertyChanged("DontOwnedGames"); }
        }

        private string gameFilterText;
        public string GameFilterText
        {
            get { return gameFilterText; }
            set
            {
                gameFilterText = value;
                OnPropertyChanged("GameFilterText");

                DisplayCollectionList.Clear();

                for (int i = 0; i < CollectionMediaList.Count; i++)
                {
                    var cm = CollectionMediaList[i];

                    var good = cm.MatchingMedia.Name.ToLower().StartsWith(GameFilterText.ToLower());

                    if (good)
                    {
                        DisplayCollectionList.Add(cm);
                    }
                }

                MediaCount = DisplayCollectionList.Count;

            }
        }

        public CollectionMediaVM()
        {
            this.RefreshCommand = new DelegateCommand<object>(this.OnRefresh);
            this.EditSelectedCommand = new DelegateCommand<object>(this.OnEditSelected);

            myCollection = LoadedData.MyCollection;
            CollectionMediaList = new CollectionGameList();
            DisplayCollectionList = new CollectionGameList();

            OwnedGames = true;

            PlatformList = LoadedData.PlatformListWithAll;
            if(PlatformList?.Count > 0)
                SelectedPlatform = PlatformList[0];

        }




        private void OnEditSelected(object obj)
        {
            if (SelectedCollectionMedia != null)
            {
                ParentVM.OpenAddToCollectionWindow(SelectedCollectionMedia);
            }
        }

        public void LoadAll(Utilities.User user)
        {
            SelectedUser = user;

       //     MediaCount = CollectionMediaList.Count;
        }

        private void OnRefresh(object obj)
        {
            RefreshData();
        }

        public void RefreshData()
        {
            GameFilterText = "";

            if (SelectedPlatform == null) return;

            CollectionMediaList.Clear();
            DisplayCollectionList.Clear();
            List<CollectionGame> listToShow = new List<CollectionGame>();

            var allCollection = MyCollection.ToList();

            if (SelectedPlatform.PlatformKey != 0)
              allCollection = MyCollection.Where(x=>x.MatchingMedia.Platform == SelectedPlatform.PlatformKey).ToList();

            List<CollectionGame> collection;
            if(extraGames)
            {
                collection = allCollection.Where(x => x.Playing == 0).ToList();
            }
            else
            {
                collection = allCollection.Where(x => x.Playing == 1).ToList();
            }

            if(DontOwnedGames)
            {
                var set = allCollection.Where(x => x.Own == 0 && x.OwnDigitally == 0).ToList();
                listToShow.AddRange(set);
            }

            if (OwnedGames && !DigitalGames && !RomGames)
            {
                var set = collection.Where(x => x.Own == 1).ToList();
                listToShow.AddRange(set);
            }
            else if (OwnedGames && DigitalGames && !RomGames)
            {
                var set = collection.Where(x => x.Own == 1 || x.OwnDigitally == 1).ToList();
                listToShow.AddRange(set);
            }
            else if (OwnedGames && DigitalGames && RomGames)
            {
                var set = collection.Where(x => x.Own == 1 || x.OwnDigitally == 1 || x.Rom == 1).ToList();
                listToShow.AddRange(set);
            }
            else if (OwnedGames && !DigitalGames && RomGames)
            {
                var set = collection.Where(x => x.Own == 1 || x.Rom == 1).ToList();
                listToShow.AddRange(set);
            }
            else if (!OwnedGames && DigitalGames && !RomGames)
            {
                var set = collection.Where(x => x.OwnDigitally == 1).ToList();
                listToShow.AddRange(set);
            }
            else if (!OwnedGames && DigitalGames && RomGames)
            {
                var set = collection.Where(x => x.OwnDigitally == 1 || x.Rom == 1).ToList();
                listToShow.AddRange(set);
            }
            else if (!OwnedGames && !DigitalGames && RomGames)
            {
                var set = collection.Where(x => x.Rom == 1).ToList();
                listToShow.AddRange(set);
            }


            listToShow = listToShow.OrderBy(x => x.MatchingMedia.Name).ToList();

            for (int lts = 0; lts < listToShow.Count; lts++)
            {
                var media = listToShow[lts];
                CollectionMediaList.Add(media);
            }

            DisplayCollectionList = Utilities.General.CloneList(CollectionMediaList);
            MediaCount = DisplayCollectionList.Count;

            YearReleasedStats = UpdateYearReleasedStats(DisplayCollectionList);
            OnPropertyChanged("YearReleasedStats");

        }

        public void UpdateListPropertyChanged()
        {
            OnPropertyChanged("EntireCollection");
            OnPropertyChanged("CollectionMediaList");
            OnPropertyChanged("DisplayCollectionList");
        }

        public ObservableCollection<Stat> UpdateYearReleasedStats(CollectionGameList games)
        {
            var stats = new ObservableCollection<Stat>();
            if (games.Count == 0) return stats;

            var orderedList = games.OrderBy(x => x.MatchingMedia.YearReleased);
            var earliestYear = orderedList.First().MatchingMedia.YearReleased;

            var foundOldest = false;
            for (int i = DateTime.Now.Year; i >= earliestYear; i--)
            {
                var thisYear = games.Where(x => x.MatchingMedia?.YearReleased == i).ToList();

                var stat = new Stat();
                stat.Name = i.ToString();
                stat.Value = thisYear.Count.ToString();

                if(thisYear.Count != 0)
                {
                    foundOldest = true;
                }

                if(foundOldest)
                {
                    stats.Add(stat);
                }


            }

         



            return stats;
        }

    }
}
