using GameBL;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DesktopUI
{
    public class AddToPlayedVM : Utilities.BaseClasses.NotifyPropertyChanged
    {

        //******************************************
        // Commands
        //******************************************
        public event EventHandler CloseWindowEvent;
        public ICommand SubmitCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }



        //******************************************
        // Properties
        //******************************************
        public string WindowTitle
        {
            get
            {
                if (IsNewMedia)
                    return "Add Game";
                else
                    return "Edit Game";
            }
        }
        public AddToPlayedWindow ThisWindow { get; set; }
        public List<Game> GameList { get; set; }
        public List<Platform> PlatformList { get; set; }
        public List<Percentages> PercentageList { get; set; }
        public MainVM ParentVM { get; set; }

        public bool IsNewMedia { get; set; }
        public PlayedGameDto OgPlayedGameDto { get; set; }


        private PlayedGame ogPlayedGame;
        public PlayedGame OgPlayedGame
        {
            get { return ogPlayedGame; }
            set { ogPlayedGame = value; OnPropertyChanged("OgPlayedGame"); }
        }


        private Percentages percentcompleted;
        public Percentages PercentCompleted
        {
            get { return percentcompleted; }
            set { percentcompleted = value; OnPropertyChanged("PercentCompleted"); }
        }

        private int rating;
        public int Rating
        {
            get { return rating; }
            set { rating = value; OnPropertyChanged("Rating"); }
        }

        private Platform selectedPlatform;
        public Platform SelectedPlatform
        {
            get { return selectedPlatform; }
            set { selectedPlatform = value; OnPropertyChanged("SelectedPlatform"); }
        }

        private string memo;
        public string Memo
        {
            get { return memo; }
            set { memo = value; OnPropertyChanged("Memo"); }
        }       

        private Game selectedGame;
        public Game SelectedGame
        {
            get { return selectedGame; }
            set { selectedGame = value; OnPropertyChanged("SelectedGame"); }
        }

        private bool _private;
        public bool Private
        {
            get { return _private; }
            set { _private = value; OnPropertyChanged("Private"); }
        }

        private DateTime dateAdded;
        public DateTime DateAdded
        {
            get { return dateAdded; }
            set { dateAdded = value; OnPropertyChanged("DateAdded"); }
        }

        private bool exactDate;
        public bool ExactDate
        {
            get { return exactDate; }
            set { exactDate = value; OnPropertyChanged("ExactDate"); }
        }

        private int hours;
        public int Hours
        {
            get { return hours; }
            set { hours = value; OnPropertyChanged("Hours"); }
        }





        //******************************************
        // Constructor
        //******************************************
        public AddToPlayedVM(AddToPlayedWindow window, MainVM parentVM, PlayedGame playedGame = null)
        {
            ThisWindow = window;
            ParentVM = parentVM;

            this.SubmitCommand = new DelegateCommand<object>(this.OnSubmit);
            this.DeleteCommand = new DelegateCommand<object>(this.OnDelete);

            GameList = LoadedData.AllGames;
            PercentageList = LoadedData.PercentageList;
            PlatformList = Utilities.General.CloneList(LoadedData.PlatformList);
            PlatformList.Insert(0, new Platform(0, "<Same as above>", 0));

            // A NEW ENTRY
            if (playedGame == null) 
            {
                IsNewMedia = true;

                ExactDate = true;

                DateAdded = DateTime.Now;
                SelectedPlatform = PlatformList[0];
                PercentCompleted = PercentageList.FirstOrDefault(x => x.ItemKey == 2);

            }
            // EDITTING AN EXISTING ENTRY
            else
            {
                IsNewMedia = false;

                OgPlayedGame = playedGame;
                OgPlayedGameDto = Utilities.General.Map<PlayedGame,PlayedGameDto>(playedGame);

                SelectedGame = GameList.SingleOrDefault(x => x.GameKey == playedGame.GameKey);
                if(SelectedGame != null)
                {
                    DateAdded = playedGame.DateAdded;
                    SelectedPlatform = PlatformList.FirstOrDefault(x => x.PlatformKey == playedGame.PlatformPlayedOn);
                    PercentCompleted = PercentageList.FirstOrDefault(x => x.ItemKey == playedGame.PercentCompleted);
                    Rating = playedGame.Rating;
                    Memo = playedGame.Memo;
                    Private = Utilities.General.IntToBool(playedGame.Private);
                    ExactDate = Utilities.General.IntToBool(playedGame.ExactDate);
                    Hours = playedGame.Hours;
                }
                else
                {
                    MessageBox.Show("Can not find game to edit.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    CloseWindowEvent?.Invoke(null, EventArgs.Empty);
                }

            }

        }

        //******************************************
        // Methods
        //******************************************
        public void OnDelete(object obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;

            if (SelectedGame?.GameKey > 0)
            {
                var result = MessageBox.Show($"Are you sure you want to delete your entry for {SelectedGame.Name}?", "Are you sure?", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if(result == MessageBoxResult.Yes)
                {

                    var ndx = LoadedData.MyPlayedGames.IndexOf(OgPlayedGame);
                    PlayedGame.DeleteMedia(OgPlayedGame);

                    LoadedData.MyPlayedGames.RemoveAt(ndx);
                    OnPropertyChanged("LoadedData.MyPlayedGames");


                    ParentVM.UpdateCollectionFinished(SelectedGame);
                    Globals.RefreshPlayed = true;
                    Stats.UpdateStats = true;
                }

            }

            CloseWindowEvent?.Invoke(null, EventArgs.Empty);
        }



        public void OnSubmit(object obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;

            var playedGame = new PlayedGame();
            if (SelectedGame?.GameKey > 0)
            {
                var mediaDto = Utilities.General.Map<Game, GameDto>(SelectedGame);
                playedGame.LoadMatchingGame(mediaDto);

                playedGame.GameKey = SelectedGame.GameKey;
                playedGame.Rating = Rating;
                playedGame.DateAdded = DateAdded;
                playedGame.Memo = Memo;
                playedGame.Private = Utilities.General.BoolToInt(Private);
                playedGame.Hours = Hours;

                var platKey = SelectedPlatform.PlatformKey;
                if(SelectedPlatform.PlatformKey == 0)
                {
                    platKey = SelectedGame.Platform;
                }
                playedGame.PlatformPlayedOn = platKey;


                playedGame.ExactDate = Utilities.General.BoolToInt(ExactDate); ;
                UpdateRating();

                if (PercentCompleted != null)
                {
                    playedGame.PercentCompleted = PercentCompleted.ItemKey;
                    playedGame.Beaten = PercentCompleted.Beat;
                }

                if (IsNewMedia)
                {
                    playedGame.UserKey = Utilities.UserUtils.CurrentUser.UserKey;
                    playedGame.PlayedKey = Utilities.General.GetNextKey(1, "PlayedKey");

                    playedGame.Insert();
                    LoadedData.MyPlayedGames.Insert(0, playedGame);
                }
                else
                {
                    playedGame.UserKey = OgPlayedGameDto.UserKey;
                    playedGame.PlayedKey = OgPlayedGameDto.PlayedKey;

                    playedGame.Update(OgPlayedGameDto);

                    var ndx = LoadedData.MyPlayedGames.IndexOf(OgPlayedGame);
                    LoadedData.MyPlayedGames.RemoveAt(ndx);

                    LoadedData.MyPlayedGames.Insert(ndx, playedGame);


                }

                ParentVM.UpdateCollectionFinished(SelectedGame);

                // Refresh Played Tab
                if (playedGame != null)
                {
                    Globals.RefreshPlayed = true;
                    Stats.UpdateStats = true;

                }
            }
            else
            {
                MessageBox.Show("Could not save game. Contact admin");
            }

           
            CloseWindowEvent?.Invoke(this, EventArgs.Empty);

        }



        public void UpdateRating()
        {
            ThisWindow.SetRating(Rating);
        }


    }
}
