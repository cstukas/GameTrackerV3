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
    public class AddToCollectionVM : Utilities.BaseClasses.NotifyPropertyChanged
    {

        //******************************************
        // Commands
        //******************************************
        public event EventHandler CloseWindowEvent;
        public ICommand SubmitCommand { get; private set; }
        public ICommand AddGenreCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }




        //******************************************
        // Properties
        //******************************************
        public string WindowTitle
        {
            get
            {
                if (!EditMode)
                    return "Add Collection Game";
                else
                    return "Edit Collection Game";
            }
        }
        public bool EditMode { get; set; }

 
        public AddToPlayedWindow ThisWindow { get; set; }
        public List<Game> GameList { get; set; }
        public List<Game> RemakeOfList { get; set; }
        public List<Platform> PlatformList { get; set; }
        public GenreList Genres { get; set; }
        public RemakeTypes RemakeTypeList { get; set; }
        public Series SeriesList { get; set; }
        public SeriesTypeList SeriesTypeList { get; set; }


        private bool isNewGame;
        public bool IsNewGame
        {
            get { return isNewGame; }
            set { isNewGame = value; OnPropertyChanged("IsNewGame"); }
        }


        public MainVM ParentVM { get; set; }
        public CollectionGameDto OgCollectionGameDto { get; set; }


        private CollectionGame ogCollGame;
        public CollectionGame OgCollGame
        {
            get { return ogCollGame; }
            set { ogCollGame = value; OnPropertyChanged("OgCollGame"); }
        }

        private Game selectedGame;
        public Game SelectedGame
        {
            get { return selectedGame; }
            set { selectedGame = value; OnPropertyChanged("SelectedGame"); }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged("Name"); }
        }


        private Platform platform;
        public Platform Platform
        {
            get { return platform; }
            set { platform = value; OnPropertyChanged("Platform"); }
        }

        private string yearReleased;
        public string YearReleased
        {
            get { return yearReleased; }
            set { yearReleased = value; OnPropertyChanged("YearReleased"); }
        }

        private int genre1;
        public int Genre1
        {
            get { return genre1; }
            set { genre1 = value; OnPropertyChanged("Genre1"); }
        }

        private int genre2;
        public int Genre2
        {
            get { return genre2; }
            set { genre2 = value; OnPropertyChanged("Genre2"); }
        }

        private int estHours;
        public int EstHours
        {
            get { return estHours; }
            set { estHours = value; OnPropertyChanged("EstHours"); }
        }

        private int price;
        public int Price
        {
            get { return price; }
            set { price = value; OnPropertyChanged("Price"); }
        }

        private RemakeType selectedRemakeType;
        public RemakeType SelectedRemakeType
        {
            get { return selectedRemakeType; }
            set { selectedRemakeType = value; OnPropertyChanged("SelectedRemakeType"); }
        }


        private int seriesKey;
        public int SeriesKey
        {
            get { return seriesKey; }
            set { seriesKey = value; OnPropertyChanged("SeriesKey"); }
        }

        private int seriesType;
        public int SeriesType
        {
            get { return seriesType; }
            set { seriesType = value; OnPropertyChanged("SeriesType"); }
        }

        private int seriesOrderNum;
        public int SeriesOrderNum
        {
            get { return seriesOrderNum; }
            set { seriesOrderNum = value; OnPropertyChanged("SeriesOrderNum"); }
        }


        private int remakeOf;
        public int RemakeOf
        {
            get { return remakeOf; }
            set { remakeOf = value; OnPropertyChanged("RemakeOf"); }
        }

        private bool own;
        public bool Own
        {
            get { return own; }
            set { own = value; OnPropertyChanged("Own"); }
        }

        private bool ownDigitally;
        public bool OwnDigitally
        {
            get { return ownDigitally; }
            set { ownDigitally = value; OnPropertyChanged("OwnDigitally"); }
        }

        private bool playing;
        public bool Playing
        {
            get { return playing; }
            set { playing = value; OnPropertyChanged("Playing"); }
        }

        private bool buying;
        public bool Buying
        {
            get { return buying; }
            set { buying = value; OnPropertyChanged("Buying"); }
        }

        private string reason;
        public string Reason
        {
            get { return reason; }
            set { reason = value; OnPropertyChanged("Reason"); }
        }

        private string status;
        public string Status
        {
            get { return status; }
            set { status = value; OnPropertyChanged("Status"); }
        }


        //******************************************
        // Constructor
        //******************************************
        public AddToCollectionVM(MainVM parentVM, CollectionGame collGame = null)
        {
            ParentVM = parentVM;

            this.SubmitCommand = new DelegateCommand<object>(this.OnSubmit);
            this.AddGenreCommand = new DelegateCommand<object>(this.OnAddGenre);
            this.DeleteCommand = new DelegateCommand<object>(this.OnDelete);

            isNewGame = true;
            GameList = Utilities.General.CloneList(LoadedData.AllGames);
            RemakeOfList = Utilities.General.CloneList(LoadedData.AllGames);
            Game empty = new Game();
            empty.GameKey = 0;
            RemakeOfList.Insert(0, empty);


            PlatformList = LoadedData.PlatformList;
            Genres = Globals.GenreList;
            RemakeTypeList = LoadedData.RemakeTypeList;

            SeriesList = Utilities.General.CloneList(LoadedData.SeriesList);

            var emptySeriesItem = new SeriesItem();
            emptySeriesItem.SeriesKey = 0;
            emptySeriesItem.Name = "";
            SeriesList.Insert(0, emptySeriesItem);


            SeriesTypeList = LoadedData.SeriesTypeList;

            if (collGame == null)
            {
                EditMode = false;

                Own = true;
                Playing = true;
                Buying = true;
            }
            else
            {
                EditMode = true;

                var game = Game.LoadGame(collGame.GameKey);
                Name = game.Name;
                Platform = PlatformList.FirstOrDefault(x=>x.PlatformKey == game.Platform);
                MapGame(game);
                isNewGame = false;

    
                OgCollGame = collGame;
                OgCollectionGameDto = Utilities.General.Map<CollectionGame, CollectionGameDto>(collGame);

                SelectedGame = GameList.SingleOrDefault(x => x.GameKey == collGame.GameKey);
                if (SelectedGame != null)
                {
                    Own = Utilities.General.IntToBool(collGame.Own);
                    OwnDigitally = Utilities.General.IntToBool(collGame.OwnDigitally);
                    Buying = Utilities.General.IntToBool(collGame.Buying);
                    Playing = Utilities.General.IntToBool(collGame.Playing);
                    Reason = collGame.Reason;
                    Status = collGame.Status;
                    
                   

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

            if (SelectedGame?.GameKey > 0)
            {
                var result = MessageBox.Show($"Are you sure you want to delete {SelectedGame.Name} from your collection?", "Are you sure?", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (result == MessageBoxResult.Yes)
                {
                    Mouse.OverrideCursor = Cursors.Wait;

                    var gameInColl = LoadedData.MyCollection.FirstOrDefault(x => x.CollectionKey == ogCollGame.CollectionKey);
                    if (LoadedData.MyCollection.Contains(gameInColl))
                    {
                        var ndx = LoadedData.MyCollection.IndexOf(gameInColl);
                        LoadedData.MyCollection.RemoveAt(ndx);
                    }

                    CollectionGame.DeleteMedia(OgCollGame);

                    Globals.RefreshCollection = true;
                    Stats.UpdateStats = true;
                }

            }

            CloseWindowEvent?.Invoke(null, EventArgs.Empty);
        }

        public void OnSubmit(object obj)
        {
            if(Name == null)
            {
                MessageBox.Show("Game Name can not be blank.");
                return;

            }
            if (Platform == null)
            {
                MessageBox.Show("Platform can not be blank.");
                return;
            }

            Mouse.OverrideCursor = Cursors.Wait;


            var now = DateTime.Now;

            // Game Object
            var game = new Game();
            game.Name = CleanName(Name);
            if (Platform != null) game.Platform = Platform.PlatformKey;
            game.Genre1 = Genre1;
            game.Genre2 = Genre2;
            game.HoursToBeat = EstHours;
            game.DateAdded = now;
            game.YearReleased = Convert.ToInt32(yearReleased);

            if(SelectedRemakeType != null)
                game.RemakeType = SelectedRemakeType.Key;

            game.SeriesKey = SeriesKey;
            game.SeriesOrderNum = SeriesOrderNum * 100;
            game.SeriesType = SeriesType;


            game.RemakeOf = RemakeOf;
            game.Price = Price;


            var existingMedia = Game.CheckIfExistsInMemory(game.Name, game.Platform);
            if (existingMedia == null) // New media
            {
                if(IsNewGame)
                {
                    game.GameKey = Utilities.General.GetNextKey(1, "GameKey") + 700;
                    game.Insert();
                    Stats.UpdateStats = true;
                    LoadedData.AddGame(game);
                }

            }
            else // Media is already in table
            {
                game.GameKey = existingMedia.GameKey;

            }

            // Collection Object
            var ownGame = new CollectionGame();
            if (Own || OwnDigitally || Buying)
            {

                // check if gamekey already exists in our collection
                var match = LoadedData.MyCollection.FirstOrDefault(x => x.GameKey == game.GameKey);


                ownGame.GameKey = game.GameKey;
                ownGame.Finished = 0;
                ownGame.Status = Status;
                ownGame.Own = Utilities.General.BoolToInt(Own);
                ownGame.OwnDigitally = Utilities.General.BoolToInt(OwnDigitally);
                ownGame.Playing = Utilities.General.BoolToInt(Playing);
                ownGame.Buying = Utilities.General.BoolToInt(Buying);
                ownGame.Reason = Reason;
                ownGame.PercentBeaten = 0;

                if(!EditMode)
                {

                    if (CollectionGame.CheckIfExists(ownGame.UserKey, ownGame.GameKey))
                    {
                        MessageBox.Show("You already own this game");
                    }
                    else
                    {
                        ownGame.UserKey = Utilities.UserUtils.CurrentUser.UserKey;
                        ownGame.CollectionKey = Utilities.General.GetNextKey(1, "CollectionKey") + 600;
                        ownGame.DateAdded = now;

                        if(match == null)
                        {
                            ownGame.Insert();
                            ownGame.MatchingMedia = game;
                            LoadedData.MyCollection.Add(ownGame);
                        }
                        else
                        {
                          
                            var dto = Utilities.General.Map<CollectionGame, CollectionGameDto>(match);
                            ownGame.CollectionKey = dto.CollectionKey;

                            if(dto.Status != "")
                                ownGame.Status = dto.Status;
                            if(dto.Reason != "")
                                ownGame.Reason = dto.Reason;

                            ownGame.Update(dto);
                            ownGame.MatchingMedia = game;

                            var gameInColl = LoadedData.MyCollection.FirstOrDefault(x => x.CollectionKey == match.CollectionKey);
                            if (LoadedData.MyCollection.Contains(gameInColl))
                            {
                                var ndx2 = LoadedData.MyCollection.IndexOf(gameInColl);
                                LoadedData.MyCollection.RemoveAt(ndx2);
                                LoadedData.MyCollection.Insert(ndx2, ownGame);
                            }

                        }

                    }
                }
                else
                {
                    ownGame.CollectionKey = OgCollectionGameDto.CollectionKey;
                    ownGame.DateAdded = OgCollectionGameDto.DateAdded;
                    ownGame.UserKey = OgCollectionGameDto.UserKey;

                    ownGame.Update(OgCollectionGameDto);
                    ownGame.MatchingMedia = game;

                    var gameInColl = LoadedData.MyCollection.FirstOrDefault(x => x.CollectionKey == ogCollGame.CollectionKey);
                    if (LoadedData.MyCollection.Contains(gameInColl))
                    {
                        var ndx2 = LoadedData.MyCollection.IndexOf(gameInColl);
                        LoadedData.MyCollection.RemoveAt(ndx2);
                        LoadedData.MyCollection.Insert(ndx2, ownGame);
                    }

                    OnPropertyChanged("ParentVM.CollectionVM.DisplayCollectionList");
                }

                ParentVM.UpdateCollectionFinished(game);

            }
            else
            {
                if(EditMode)
                {
                    var result = MessageBox.Show($"Are you sure you want to remove {OgCollGame.MatchingMedia.Name} from your collection?", "Are you sure?", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        var gameInColl = LoadedData.MyCollection.FirstOrDefault(x => x.CollectionKey == ogCollGame.CollectionKey);
                        if (LoadedData.MyCollection.Contains(gameInColl))
                        {
                            var ndx = LoadedData.MyCollection.IndexOf(gameInColl);
                            LoadedData.MyCollection.RemoveAt(ndx);
                        }

                        CollectionGame.DeleteMedia(OgCollGame);

                        Globals.RefreshCollection = true;
                        Stats.UpdateStats = true;
                    }
                }

            }


            // Refresh Collection Tab
            if (ownGame != null)
            {
                Globals.RefreshCollection = true;
                Stats.UpdateStats = true;
            }


            CloseWindowEvent?.Invoke(null, EventArgs.Empty);

        }

        private void OnAddGenre(object obj)
        {
            var agWindow = new HelperUI.AddGenreWindow(null);
            agWindow.ShowDialog();
        }



        public void NameUnfocused()
        {

            if (Name != null && Platform != null)
            {
                Mouse.OverrideCursor = Cursors.Wait;

                var contents = CleanName(Name);

                if (!string.IsNullOrWhiteSpace(contents))
                {
                    var check = Game.CheckIfExists(contents, Platform.PlatformKey);
                    if (check != null)
                    {
                        MapGame(check);
                        IsNewGame = false;
                    }


                }

                Mouse.OverrideCursor = null;

            }
        }

        public void MapGame(Game game)
        {
            Genre1 = game.Genre1;
            Genre2 = game.Genre2;
            EstHours = game.HoursToBeat;
            Price = game.Price;
            YearReleased = game.YearReleased.ToString();
            SelectedRemakeType = RemakeTypeList.FirstOrDefault(x=>x.Key ==  game.RemakeType);
        //    SelectedSeriesItem = SeriesList.FirstOrDefault(x => x.SeriesKey == game.SeriesKey);
            remakeOf = game.RemakeOf;
        }

        public string CleanName(string name)
        {
            if (name == null) return "";

            var newName = name.Replace("'", "|");
            if (newName.Contains("-"))
                newName = newName.Split('-')[0].Trim();

            return newName;
        }


    }
}
