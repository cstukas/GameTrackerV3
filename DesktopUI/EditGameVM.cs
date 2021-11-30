using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GameBL;
using Prism.Commands;

namespace DesktopUI
{
    public class EditGameVM : Utilities.BaseClasses.NotifyPropertyChanged
    {
        public event EventHandler CloseWindowEvent;


        public ICommand AddGenreCommand { get; private set; }
        public ICommand UpdateGameCommand { get; private set; }

        public GameDto OgGame { get; set; }
        public List<Game> GameList { get; set; }
        public List<Game> RemakeOfList { get; set; }
        public GenreList Genres { get; set; }
        public RemakeTypes RemakeTypeList { get; set; }
        public Series SeriesList { get; set; }
        public SeriesTypeList SeriesTypeList { get; set; }


        public int GameKey { get; set; }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged("Name"); }
        }

        private string platform;
        public string Platform
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

        private int remakeType;
        public int RemakeType
        {
            get { return remakeType; }
            set { remakeType = value; OnPropertyChanged("RemakeType"); }
        }

        private int remakeOf;
        public int RemakeOf
        {
            get { return remakeOf; }
            set { remakeOf = value; OnPropertyChanged("RemakeOf"); }
        }

        public DateTime DateAdded { get; set; }


        private int seriesKey;
        public int SeriesKey
        {
            get { return seriesKey; }
            set { seriesKey = value; OnPropertyChanged("SeriesKey"); }
        }


        public int SeriesOrderNum { get; set; }
        public int SeriesType { get; set; }


        public EditGameVM(Game game)
        {
            this.AddGenreCommand = new DelegateCommand<object>(this.OnAddGenre);
            this.UpdateGameCommand = new DelegateCommand<object>(this.OnUpdateGame);


            GameList = Utilities.General.CloneList(LoadedData.AllGames);
            RemakeOfList = Utilities.General.CloneList(LoadedData.AllGames);
            Game empty = new Game();
            empty.GameKey = 0;
            RemakeOfList.Insert(0, empty);

            Genres = Globals.GenreList;
            RemakeTypeList = LoadedData.RemakeTypeList;
            SeriesList = LoadedData.SeriesList;
            SeriesTypeList = LoadedData.SeriesTypeList;


            OgGame = Utilities.General.Map<Game, GameDto>(game);

            GameKey = game.GameKey;
            Name = game.Name;
            Platform = LoadedData.PlatformList.FirstOrDefault(x=>x.PlatformKey == game.Platform)?.Name;
            YearReleased = game.YearReleased.ToString();
            Genre1 = game.Genre1;
            Genre2 = game.Genre2;
            EstHours = game.HoursToBeat;
            Price = game.Price;
            RemakeOf = game.RemakeOf;
            RemakeType = game.RemakeType;
            DateAdded = game.DateAdded;
            SeriesKey = game.SeriesKey;

            if(game.SeriesOrderNum > 0)
                SeriesOrderNum = game.SeriesOrderNum / 100;

            SeriesType = game.SeriesType;

        }
        private void OnAddGenre(object obj)
        {
            var agWindow = new HelperUI.AddGenreWindow(null);
            agWindow.ShowDialog();
        }

        private void OnUpdateGame(object obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;

            var g = new GameDto();
            g.GameKey = GameKey;
            g.Name = Name;
            g.Platform = LoadedData.PlatformList.FirstOrDefault(x => x.Name == Platform).PlatformKey;
            g.YearReleased = Convert.ToInt32(YearReleased);
            g.Genre1 = Genre1;
            g.Genre2 = Genre2;
            g.HoursToBeat = EstHours;
            g.Price = Price;
            g.RemakeOf = RemakeOf;
            g.RemakeType = RemakeType;
            g.DateAdded = DateAdded;
            g.SeriesKey = SeriesKey;
            g.SeriesOrderNum = SeriesOrderNum * 100;
            g.SeriesType = SeriesType;

            Game.UpdateGame(OgGame, g);

            var findGame = LoadedData.AllGames.FirstOrDefault(x => x.GameKey == g.GameKey);
            findGame = Utilities.General.Map<GameDto, Game>(g);
            OnPropertyChanged("LoadedData.AllGames");

            Mouse.OverrideCursor = null;

            CloseWindowEvent?.Invoke(null, EventArgs.Empty);

        }

    }

}
