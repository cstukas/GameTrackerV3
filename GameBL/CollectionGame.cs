using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBL
{
    public class CollectionGameDto
    {
        public int GameKey { get; set; }
        public int UserKey { get; set; }
        public int CollectionKey { get; set; }
        public int Finished { get; set; }
        public string Status { get; set; }
        public int Own { get; set; }
        public int OwnDigitally { get; set; }
        public int Playing { get; set; }
        public int Buying { get; set; }
        public string Reason { get; set; }
        public int TimesBeat { get; set; }
        public int PercentBeaten { get; set; }
        public DateTime DateAdded { get; set; }

    }

    [Serializable]
    public class CollectionGame : Utilities.BaseClasses.NotifyPropertyChanged
    {
        #region DB Properties
        private int gamekey;
        public int GameKey
        {
            get { return gamekey; }
            set { gamekey = value; OnPropertyChanged("GameKey"); }
        }

        private int userkey;
        public int UserKey
        {
            get { return userkey; }
            set { userkey = value; OnPropertyChanged("UserKey"); }
        }

        private int collectionkey;
        public int CollectionKey
        {
            get { return collectionkey; }
            set { collectionkey = value; OnPropertyChanged("CollectionKey"); }
        }

        private int finished;
        public int Finished
        {
            get { return finished; }
            set { finished = value; OnPropertyChanged("Finished"); }
        }

        private string status;
        public string Status
        {
            get { return status; }
            set { status = value; OnPropertyChanged("Status"); }
        }

        private int own;
        public int Own
        {
            get { return own; }
            set { own = value; OnPropertyChanged("Own"); }
        }


        private int owndigitally;
        public int OwnDigitally
        {
            get { return owndigitally; }
            set { owndigitally = value; OnPropertyChanged("OwnDigitally"); }
        }

        private int playing;
        public int Playing
        {
            get { return playing; }
            set { playing = value; OnPropertyChanged("Playing"); }
        }

        private int buying;
        public int Buying
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

        private int timesbeat;
        public int TimesBeat
        {
            get { return timesbeat; }
            set { timesbeat = value; OnPropertyChanged("TimesBeat"); }
        }

        private int percentbeaten;
        public int PercentBeaten
        {
            get { return percentbeaten; }
            set { percentbeaten = value; OnPropertyChanged("PercentBeaten"); }
        }

        private DateTime dateadded;
        public DateTime DateAdded
        {
            get { return dateadded; }
            set { dateadded = value; OnPropertyChanged("DateAdded"); }
        }

        #endregion

        private Game matchingMedia;
        public Game MatchingMedia
        {
            get { return matchingMedia; }
            set { matchingMedia = value; OnPropertyChanged("MatchingMedia"); }
        }


        public CollectionGame()
        {

        }

        public static void UpdatePlayedCount(CollectionGame cMedia)
        {
            if (cMedia.GameKey > 0)
            {
                var sql = $"SELECT PlayedKey FROM Played WHERE GameKey = {cMedia.GameKey} AND UserKey = '{cMedia.UserKey}'";
                var count = DataAccess.DBFunctions.LoadList<int>(sql);
                var update = $"UPDATE Collection SET Finished = {count.Count} WHERE CollectionKey = {cMedia.CollectionKey}";
                DataAccess.DBFunctions.RunQuery(update);
            }
        }

        public void LoadMatchingGame(GameDto gameDto)
        {
            MatchingMedia = Utilities.General.Map<GameDto, Game>(gameDto);
        }

        public void Insert()
        {
            var dto = Utilities.General.Map<CollectionGame, CollectionGameDto>(this);
            DataAccess.DBFunctions.InsertObject(dto, "Collection");
        }

        public void Update(CollectionGameDto ogDto)
        {
            var dto = Utilities.General.Map<CollectionGame, CollectionGameDto>(this);
            string[] keys = { "CollectionKey" };
            DataAccess.DBFunctions.UpdateObject(dto, ogDto, "Collection", keys);
        }

        public static bool CheckIfExists(int userKey, int gameKey)
        { 
            var sql = $"SELECT GameKey FROM Collection WHERE UserKey = {userKey} AND GameKey = {gameKey}";
            var count = DataAccess.DBFunctions.LoadList<int>(sql);
            if (count.Count > 0)
                return true;
            else
                return false;

        }

        public static void DeleteMedia(CollectionGame game)
        {
            var sql = $"DELETE FROM Collection WHERE CollectionKey = {game.CollectionKey}";
            DataAccess.DBFunctions.RunQuery(sql);

        }

        public static List<Game> GetAllAlikeGames(Game game, CollectionGameList collection)
        {
            int gameKey;
            if (game.RemakeOf <= 0)
            {
                gameKey = game.GameKey;
            }
            else
            {
                gameKey = game.RemakeOf;
            }


            var ogGame = LoadedData.AllGames.FirstOrDefault(x => x.GameKey == gameKey);
            if (ogGame != null)
            {
                var allAlikeGames = LoadedData.AllGames.Where(x => x.RemakeOf == ogGame.GameKey || x.GameKey == ogGame.GameKey || x.Name.ToLower() == game.Name.ToLower()).ToList();
                return allAlikeGames;
            }

            return new List<Game>();


        }
    }
}
