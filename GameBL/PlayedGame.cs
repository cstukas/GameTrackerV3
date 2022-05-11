using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBL
{
    public class PlayedGameDto
    {
        public int UserKey { get; set; }
        public int GameKey { get; set; }
        public int PlayedKey { get; set; }
        public int Rating { get; set; }
        public string Memo { get; set; }
        public DateTime DateAdded { get; set; }
        public int ExactDate { get; set; }
        public int Private { get; set; }
        public int Hours { get; set; }
        public int PlatformPlayedOn { get; set; }
        public int PercentCompleted { get; set; }
        public int Beaten { get; set; } // bool


    }

    [Serializable]
    public class PlayedGame : Utilities.BaseClasses.NotifyPropertyChanged
    {
        #region DB Properties
        private int userkey;
        public int UserKey
        {
            get { return userkey; }
            set { userkey = value; OnPropertyChanged("UserKey"); }
        }

        private int gamekey;
        public int GameKey
        {
            get { return gamekey; }
            set { gamekey = value; OnPropertyChanged("GameKey"); }
        }

        private int playedkey;
        public int PlayedKey
        {
            get { return playedkey; }
            set { playedkey = value; OnPropertyChanged("PlayedKey"); }
        }

        private int rating;
        public int Rating
        {
            get { return rating; }
            set { rating = value; OnPropertyChanged("Rating"); }
        }

        private string memo;
        public string Memo
        {
            get { return memo; }
            set { memo = value; OnPropertyChanged("Memo"); }
        }

        private DateTime dateadded;
        public DateTime DateAdded
        {
            get { return dateadded; }
            set 
            {
                dateadded = value; 
                OnPropertyChanged("DateAdded");
            }
        }

        private int exactDate;
        public int ExactDate
        {
            get { return exactDate; }
            set { exactDate = value; OnPropertyChanged("ExactDate"); }
        }


        private int _private;
        public int Private
        {
            get { return _private; }
            set
            {
                _private = value; OnPropertyChanged("Private");
            }
        }

        private int hours;
        public int Hours
        {
            get { return hours; }
            set { hours = value; OnPropertyChanged("Hours"); }
        }

        private int platformplayedon;
        public int PlatformPlayedOn
        {
            get { return platformplayedon; }
            set { platformplayedon = value; OnPropertyChanged("PlatformPlayedOn"); }
        }

        private int percentcompleted;
        public int PercentCompleted
        {
            get { return percentcompleted; }
            set { percentcompleted = value; OnPropertyChanged("PercentCompleted"); }
        }

        private int beaten;
        public int Beaten
        {
            get { return beaten; }
            set { beaten = value; OnPropertyChanged("Beaten"); }
        }



        #endregion


        public int Gap { get; set; }
        public int OgGap { get; set; }

        private Game matchingMedia;
        public Game MatchingMedia
        {
            get { return matchingMedia; }
            set { matchingMedia = value; OnPropertyChanged("MatchingMedia"); }
        }

        public string DateInfo => $" {Utilities.General.GetShortMonthNameFromNumber(DateAdded.Month)} {DateAdded.Year}";
        public string RatingInfo
        {
            get
            {
                if (Rating > 0)
                    return "  -  " + Utilities.General.GetRating(Rating);
                else
                    return "";
            }
        }

        public string RatingInfo2
        {
            get
            {
                return Utilities.General.GetRating(Rating);
            }
        }

        private string monthName;
        public string MonthName
        {
            get 
            {
                if (ExactDate == 1)
                    return Utilities.General.GetMonthNameFromNumber(DateAdded.Month);
                else
                    return "";
            }
            set { monthName = value; OnPropertyChanged("MonthName"); }
        }


        public string InfoString
        {
            get
            {

                string extra = "";
                if (Hours > 0) extra = $" - {Hours} hour";
                if (Hours > 1) extra += "s";

                string remakeType = "";
                if (!(MatchingMedia.RemakeType == 0))
                {
                    remakeType = " " + MatchingMedia.RemakeType;
                }

                // month
                var monthName = Utilities.General.GetShortMonthNameFromNumber(DateAdded.Month);
                if (monthName != "")
                    monthName = $" {monthName}";

                // Quarantine
                var quarantine = "";
                if (DateAdded.Year == 2020)
                {
                    if (DateAdded.Month >= 3)
                    {
                        if (DateAdded > new DateTime(2020, 3, 14)) // The day I started the lockdown
                        {
                            quarantine = " (Covid Season)";
                            if (DateAdded < new DateTime(2020, 5, 29)) // The day I went back to work
                            {
                                quarantine = " (Covid Lockdown)";
                            }
                        }
                    }

                }

                var displayPercentageComplete = LoadedData.PercentageList.FirstOrDefault(x => x.ItemKey == PercentCompleted).Name;
                return $"{DateAdded.Year}{monthName}{quarantine} - {Platform.KeyToName(MatchingMedia.Platform)}{remakeType} - {displayPercentageComplete}{extra}";
            }
        }

        public string DateInfoReversed
        {
            get
            {
                // month
                string monthName = "";
                if(ExactDate == 1)
                {
                    monthName = Utilities.General.GetShortMonthNameFromNumber(DateAdded.Month);
                }


                return $"{DateAdded.Year} {monthName}";
            }
        }

        public string PlatformInfo
        {
            get
            {
                string remakeType = "";
                if (MatchingMedia.RemakeType > 0)
                {
                    remakeType = " " + RemakeTypes.KeyToName(MatchingMedia.RemakeType);
                }

                return $"{Platform.KeyToName(MatchingMedia.Platform)}{remakeType}";
            }
        }

        public string PlatformInfo2
        {
            get
            {
                string remakeType = "";
                if (MatchingMedia.RemakeType > 0)
                {
                    remakeType = " " + RemakeTypes.KeyToName(MatchingMedia.RemakeType);
                }

                var str = $"{Platform.KeyToName(MatchingMedia.Platform)}{remakeType}";
                if(MatchingMedia.Platform != PlatformPlayedOn)
                {
                    str += $" played on {Platform.KeyToName(PlatformPlayedOn)}";
                }

                return $"{str}";
            }
        }

        public string GapInfo
        {
            get
            {
                return $"{Gap}";
            }
        }

        public bool OgGapInfo
        {
            get
            {
                if(OgGap != Gap)
                {
                    return true;
                }

                return false;
            }
        }

        public bool PlayedOn
        {
            get
            {
                if (MatchingMedia.Platform != PlatformPlayedOn)
                {
                    return true;
                }

                return false;
            }
        }

        public string PlatformPlayedOnInfo
        {
            get
            {
                return $"{Platform.KeyToName(PlatformPlayedOn)}";
            }
        }

        public string PercentageInfo
        {
            get
            {
                var displayPercentageComplete = LoadedData.PercentageList.FirstOrDefault(x => x.ItemKey == PercentCompleted).Name;
                return displayPercentageComplete;
            }
        }

        public string HourInfo
        {
            get
            {
                string extra = "";
                if (Hours > 0) extra = $"{Hours} hour";
                if (Hours > 1) extra += "s";
                return extra;
            }
        }


        public PlayedGame()
        {
            

        }


        

        public void LoadMatchingGame(GameDto gameDto)
        {
            MatchingMedia = Utilities.General.Map<GameDto, Game>(gameDto);


            if (MatchingMedia.YearReleased > 0)
            {
                int yearReleased = MatchingMedia.YearReleased;
                int origYearsReleased = MatchingMedia.YearReleased;
                if (MatchingMedia.RemakeOf > 0)
                {
                    var newMatch = LoadedData.AllGames.FirstOrDefault(x => x.GameKey == MatchingMedia.RemakeOf);
                    if (newMatch != null)
                    {
                        origYearsReleased = newMatch.YearReleased;
                    }
                }

                Gap = DateAdded.Year - yearReleased;
                OgGap = DateAdded.Year - origYearsReleased;
            }
        }


        public static List<PlayedGame> GetBeatenGamesFromMemory(List<Game> allAlikeGames)
        {
            var beatenGames = new List<PlayedGame>();
            for (int ag = 0; ag < allAlikeGames.Count; ag++)
            {
                var alikeGame = allAlikeGames[ag];

                // Get all Played/Beated for this game
                var playedGames = LoadedData.MyPlayedGames.Where(x => x.GameKey == alikeGame.GameKey).ToList();
                if (playedGames.Count > 0)
                {

                    for (int pgi = 0; pgi < playedGames.Count; pgi++)
                    {
                        var playedGameDto = playedGames[pgi];
                        var beat = LoadedData.PercentageList.FirstOrDefault(x => x.ItemKey == playedGameDto.PercentCompleted)?.Beat;
                        //  var fin = LoadedData.PercentageList.FirstOrDefault(x => x.ItemKey == playedGameDto.PercentCompleted)?.Finished;

                        if (beat == 1)
                            beatenGames.Add(playedGameDto);
                    }

                }
            }

            return beatenGames;




            //var list = LoadedData.MyPlayedGames.Where(x => x.GameKey == game.GameKey && x.UserKey == Utilities.UserUtils.CurrentUser.UserKey).ToList();

            //var count = new List<PlayedGame>();
            //foreach (var item in list)
            //{
            //    var beat = LoadedData.PercentageList.FirstOrDefault(x => x.ItemKey == item.PercentCompleted).Beat;
            //    if (beat == 1)
            //        count.Add(item);

            //}

            //return count.Count;

        }

        public static int GetBeatenGameCount(int gameKey, int userKey)
        {
            var beatenPercentages = LoadedData.PercentageList.Where(x => x.Beat == 1).ToList();

            var beatenSql = new StringBuilder();
            if (beatenPercentages.Count > 0)
            {
                beatenSql.Append(" AND (");
                for (int i = 0; i < beatenPercentages.Count; i++)
                {
                    var percentKey = beatenPercentages[i].ItemKey;
                    beatenSql.Append($"PercentCompleted = {percentKey}");

                    if (i != beatenPercentages.Count - 1)
                        beatenSql.Append(" OR ");

                }
                beatenSql.Append(")");
            }

            var userKeySql = $" AND UserKey = {userKey}";
            if (userKey == 0)
                userKeySql = "";

            var sql = $"SELECT GameKey FROM Played WHERE GameKey = {gameKey}{userKeySql} {beatenSql.ToString()}";
            var list = DataAccess.DBFunctions.LoadList<int>(sql);
            return list.Count;
        }

        public static void UpdatePlayedCount(int newCount, int gameKey, int userKey)
        {
            var sql = $"UPDATE Collection SET TimesBeat = {newCount} WHERE GameKey = {gameKey} AND UserKey = {userKey}";
            DataAccess.DBFunctions.RunQuery(sql);
        }

        public void Insert()
        {
            var dto = Utilities.General.Map<PlayedGame, PlayedGameDto>(this);
            DataAccess.DBFunctions.InsertObject(dto, "Played");
        }

        public void Update(PlayedGameDto ogDto)
        {
            var dto = Utilities.General.Map<PlayedGame, PlayedGameDto>(this);
            string[] keys = { "PlayedKey" };
            DataAccess.DBFunctions.UpdateObject(dto, ogDto, "Played", keys);
        }


        public static void DeleteMedia(PlayedGame game)
        {
            var sql = $"DELETE FROM Played WHERE PlayedKey = {game.PlayedKey}";
            DataAccess.DBFunctions.RunQuery(sql);

        }


    }
}
