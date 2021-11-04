using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBL
{
    [Serializable]
    public class GameDto
    {
        public int GameKey { get; set; }
        public string Name { get; set; }
        public int Platform { get; set; }
        public int Genre1 { get; set; }
        public int Genre2 { get; set; }
        public int HoursToBeat { get; set; }
        public DateTime DateAdded { get; set; }
        public int SeriesKey { get; set; }
        public int SeriesType { get; set; }
        public int SeriesOrderNum { get; set; }
        public int YearReleased { get; set; }
        public int RemakeType { get; set; }
        public int RemakeOf { get; set; }
        public int Price { get; set; }
    }

    [Serializable]
    public class Game : Utilities.BaseClasses.NotifyPropertyChanged
    {
        #region DB Props
        private int gamekey;
        public int GameKey
        {
            get { return gamekey; }
            set { gamekey = value; OnPropertyChanged("GameKey"); }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged("Name"); }
        }

        private int platform;
        public int Platform
        {
            get { return platform; }
            set { platform = value; OnPropertyChanged("Platform"); }
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

        private int hourstobeat;
        public int HoursToBeat
        {
            get { return hourstobeat; }
            set { hourstobeat = value; OnPropertyChanged("HoursToBeat"); }
        }

        private DateTime dateadded;
        public DateTime DateAdded
        {
            get { return dateadded; }
            set { dateadded = value; OnPropertyChanged("DateAdded"); }
        }

        private int serieskey;
        public int SeriesKey
        {
            get { return serieskey; }
            set { serieskey = value; OnPropertyChanged("SeriesKey"); }
        }

        private int seriestype;
        public int SeriesType
        {
            get { return seriestype; }
            set { seriestype = value; OnPropertyChanged("SeriesType"); }
        }

        private int seriesordernum;
        public int SeriesOrderNum
        {
            get { return seriesordernum; }
            set { seriesordernum = value; OnPropertyChanged("SeriesOrderNum"); }
        }

        private int yearreleased;
        public int YearReleased
        {
            get { return yearreleased; }
            set { yearreleased = value; OnPropertyChanged("YearReleased"); }
        }

        private int remakeof;
        public int RemakeOf
        {
            get { return remakeof; }
            set { remakeof = value; OnPropertyChanged("RemakeOf"); }
        }

        private int remaketype;
        public int RemakeType
        {
            get { return remaketype; }
            set { remaketype = value; OnPropertyChanged("RemakeType"); }
        }

        private int price;
        public int Price
        {
            get { return price; }
            set { price = value; OnPropertyChanged("Price"); }
        }




        #endregion


        public string DisplayString
        {
            get 
            {
                if (string.IsNullOrWhiteSpace(Name))
                    return "";

                return $"{Name} - {GameBL.Platform.KeyToName(Platform)}";
            }
        }


        public Game()
        {

        }

        public void Insert()
        {
            var dto = Utilities.General.Map<Game, GameDto>(this);
            DataAccess.DBFunctions.InsertObject(dto, "Game");
        }

        public static Game LoadMedia(string name)
        {
            var media = new Game();
            var sql = $"SELECT * FROM Game WHERE Name = '{name.Replace("'", "|")}'";
            var dto = DataAccess.DBFunctions.LoadObject<GameDto>(sql);
            if(dto != null)
            {
                dto.Name = dto.Name.Replace("|", "'");
                media = Utilities.General.Map<GameDto, Game>(dto);
                
                return media;
            }

            return null;
        }

        public static Game CheckIfExists(string name, int platform)
        {
            var media = new Game();
            var sql = $"SELECT * FROM Game WHERE Name = '{name}' AND Platform = '{platform}'";
            var dto = DataAccess.DBFunctions.LoadObject<GameDto>(sql);
            if (dto != null)
            {
                dto.Name = dto.Name.Replace("|", "'");
                media = Utilities.General.Map<GameDto, Game>(dto);

                return media;
            }

            return null;
        }

        public static Game CheckIfExistsInMemory(string name, int platform)
        {
            var game = LoadedData.AllGames.FirstOrDefault(x => x.Name.ToLower() == name.ToLower() && x.Platform == platform);

            if (game != null)
            {
                game.Name = game.Name.Replace("|", "'");
                return game;
            }

            return null;
        }

        public static Game LoadGame(int gameKey)
        {
            var media = new Game();
            var sql = $"SELECT * FROM Game WHERE GameKey = {gameKey}";
            var dto = DataAccess.DBFunctions.LoadObject<GameDto>(sql);
            if (dto != null)
            {
                media = Utilities.General.Map<GameDto, Game>(dto);
                return media;
            }

            return null;
        }


        public static void UpdateGame(GameDto ogDto, GameDto dto)
        {
            string[] keys = { "GameKey" };
            DataAccess.DBFunctions.UpdateObject(dto, ogDto, "Game", keys);
        }


    }
}
