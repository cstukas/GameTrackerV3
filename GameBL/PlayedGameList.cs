using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.ObjectModel;

namespace GameBL
{
    public class PlayedGameList : ObservableCollection<PlayedGame>
    {

        public PlayedGameList()
        {

        }


        public static PlayedGameList LoadYourPlayed(int userKey) 
        {
            var fmList = new PlayedGameList();
            var list = LoadedData.MyPlayedGames.Where(x => x.Beaten == 1).OrderByDescending(x => x.DateAdded).ToList();

            if (list.Count > 0)
            {
                List<GameDto> gameDtos = new List<GameDto>();
                var allGames = LoadedData.AllGames;
                for (int a = 0; a < allGames.Count; a++)
                {
                    var game = allGames[a];
                    var dto = Utilities.General.Map<Game, GameDto>(game);
                    gameDtos.Add(dto);
                }

                for (int j = 0; j < list.Count; j++)
                {
                    var fm = list[j];

                    var matchingMedia = gameDtos.FirstOrDefault(x => x.GameKey == fm.GameKey);
                    if (matchingMedia != null)
                    {
                        matchingMedia.Name = matchingMedia.Name.Replace("|", "'");
                        fm.LoadMatchingGame(matchingMedia);
                    }
                    else
                    {
                        Utilities.Logger.Log($"No matching game for GameKey = {fm.GameKey}", true);
                    }

                    fmList.Add(fm);
                }
            }

            return fmList;
        }



        public static List<GameDto> GetAllAlikeGames(string name, int key)
        {
            return DataAccess.DBFunctions.LoadList<GameDto>($"SELECT * FROM Game WHERE (Name = '{name}' OR RemakeOf = '{key}') ORDER BY YearReleased");
        }


        public static PlayedGameList LoadAllPlayed(List<GameDto> gameDtos, bool onlyShowMine)
        {
            var fmList = new PlayedGameList();

            //var gameName = DataAccess.DBFunctions.LoadObject<string>($"SELECT Name FROM Game WHERE GameKey = '{gameKey}'");
            //var gameDtos = DataAccess.DBFunctions.LoadList<GameDto>($"SELECT * FROM Game WHERE (Name = '{gameName}' OR RemakeOf = '{gameKey}') ");

            int originalYearReleased = 0;
            string whereSql = "";
            for (int i = 0; i < gameDtos.Count; i++)
            {
                var dto = gameDtos[i];
                int key = dto.GameKey;

                if (originalYearReleased == 0)
                    originalYearReleased = dto.YearReleased;
                else
                    if (dto.YearReleased < originalYearReleased)
                        originalYearReleased = dto.YearReleased;
                
                if (i != 0)
                {
                    whereSql += " OR";
                }

                whereSql += $" GameKey = '{key}' ";
            }

            //var mineSql = "";
            //if (onlyShowMine)
            //{
            //    mineSql = $" AND UserKey = {Utilities.UserUtils.CurrentUser.UserKey}";
            //}

            var playsDtos = DataAccess.DBFunctions.LoadList<PlayedGameDto>($"SELECT * FROM Played WHERE {whereSql} ORDER BY DateAdded DESC");

            foreach (var item in playsDtos)
            {
                PlayedGame pg = Utilities.General.Map<PlayedGameDto, PlayedGame>(item);

                var game = gameDtos.FirstOrDefault(x => x.GameKey == item.GameKey);
                pg.LoadMatchingGame(game);
                fmList.Add(pg);
            }

            return fmList;
        }

        public static PlayedGameList LoadFromMemory(bool onlyBeaten, string year, int platformKey, bool playedOnIsChecked, int genreKey)
        {
            var list = new List<PlayedGame>();
            var pmList = new PlayedGameList();

            if (LoadedData.MyPlayedGames != null)
            {
                if (onlyBeaten)
                    list = LoadedData.MyPlayedGames.Where(x => x.Beaten == 1).ToList();
                else
                    list = LoadedData.MyPlayedGames.ToList();


                if (year.ToLower() != "<all>")
                    list = list.Where(x => x.DateAdded.Year == Convert.ToInt32(year)).ToList();

                if (genreKey > 0)
                    list = list.Where(x => x.MatchingMedia.Genre1 == genreKey || x.MatchingMedia.Genre2 == genreKey).ToList();

                if (platformKey > 0)
                {
                    if(playedOnIsChecked)
                        list = list.Where(x => x.PlatformPlayedOn == platformKey).ToList();
                    else
                        list = list.Where(x => x.MatchingMedia.Platform == platformKey).ToList();
                }
            }

            for (int i = 0; i < list.Count; i++)
            {
                var item = list[i];
                pmList.Add(item);
            }

            return pmList;
        }

        public static PlayedGameList LoadGame(int userKey, bool excludePrivate, int selectTop, string endingSqlString, bool onlyBeaten, string year, int plat, int genreKey)
        {

            var list = new List<PlayedGame>();
            var pmList = new PlayedGameList();

            // Create Finished Media sql
            var userSql = $" AND UserKey = {userKey}";
            if (userKey <= 0) // Admin
                userSql = ""; // load all

            var topStr = "";
            if (selectTop > 0)
            {
                topStr = $" TOP {selectTop}";
            }

            var exPriv = "";
            if (userKey != Utilities.UserUtils.CurrentUser.UserKey)
            {
                if (excludePrivate)
                {
                    exPriv = " AND Private = 0 ";
                }
            }

            var beatenSql = "";
            if (onlyBeaten)
                beatenSql = " AND Beaten = 1";

            var platSql = "";
            if (plat != 0)
                platSql = $" AND PlatformPlayedOn = {plat}";

            var yearSql = "";
            if (year != "" && year != "<All>")
                yearSql = $" AND year(DateAdded) = {year}";


            // Load finished media
            var sql = $"SELECT{topStr} * FROM Played WHERE GameKey > 0{userSql}{exPriv}{beatenSql}{platSql}{yearSql} {endingSqlString}";
            var playedDtos = DataAccess.DBFunctions.LoadList<PlayedGameDto>(sql);
            if (playedDtos.Count > 0)
            {
                List<GameDto> gameDtos = new List<GameDto>();
                var allGames = LoadedData.AllGames;
                for (int a = 0; a < allGames.Count; a++)
                {
                    var game = allGames[a];
                    var dto = Utilities.General.Map<Game, GameDto>(game);
                    gameDtos.Add(dto);
                }

                for (int j = 0; j < playedDtos.Count; j++)
                {
                    var dto = playedDtos[j];
                    var fm = new PlayedGame();
                    fm = Utilities.General.Map<PlayedGameDto, PlayedGame>(dto);

                    var matchingMedia = gameDtos.FirstOrDefault(x => x.GameKey == fm.GameKey);
                    if (matchingMedia != null)
                    {
                        matchingMedia.Name = matchingMedia.Name.Replace("|", "'");
                        fm.LoadMatchingGame(matchingMedia);
                    }
                    else
                    {
                        Utilities.Logger.Log($"No matching game for GameKey = {fm.GameKey}", true);
                    }

                    pmList.Add(fm);
                }


            }

            return pmList;
        }


    }
}
