using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.ObjectModel;
using DataAccess;

namespace GameBL
{
    [Serializable]
    public class CollectionGameList : ObservableCollection<CollectionGame>
    {

        public CollectionGameList()
        {

        }

        public void LoadCollection(int userKey)
        {
            this.Clear();
            var list = new List<CollectionGame>();

            // Create Finished Media sql
            var userSql = $" AND UserKey = {userKey}";
            if (userKey == 0) // Admin
                userSql = ""; // load all

            var sql = $"SELECT * FROM Collection WHERE GameKey > 0{userSql}";
            var collectionDtos = DataAccess.DBFunctions.LoadList<CollectionGameDto>(sql);
            var collection = new List<CollectionGame>();
            for (int cd = 0; cd < collectionDtos.Count; cd++)
            {
                var dto = collectionDtos[cd];
                collection.Add(Utilities.General.Map<CollectionGameDto, CollectionGame>(dto));
            }


            if (collection.Count > 0)
            {
                List<GameDto> gameDtos = new List<GameDto>();
                var allGames = LoadedData.AllGames;
                for (int a = 0; a < allGames.Count; a++)
                {
                    var game = allGames[a];
                    var dto = Utilities.General.Map<Game, GameDto>(game);
                    gameDtos.Add(dto);
                }


                // Go through collection games and load them up
                for (int j = 0; j < collection.Count; j++)
                {
                    var cm = collection[j];

                    // Match a game object to the Collection Game
                    var matchingMedia = gameDtos.FirstOrDefault(x => x.GameKey == cm.GameKey);
                    if (matchingMedia != null)
                    {
                        matchingMedia.Name = matchingMedia.Name.Replace("|", ",");
                        cm.LoadMatchingGame(matchingMedia);
                    }
                    else
                    {
                        Utilities.Logger.Log($"No matching media for gameKey = {cm.GameKey}", true);
                    }

                    list.Add(cm);
                }

                var pgDtos = new List<PlayedGameDto>();
                var allPlayed = LoadedData.MyPlayedGames;
                for (int a = 0; a < allPlayed.Count; a++)
                {
                    var game = allPlayed[a];
                    var dto = Utilities.General.Map<PlayedGame, PlayedGameDto>(game);
                    pgDtos.Add(dto);
                }

              
                // Go through your collection again and set Finished/Times beat
                var ordered = list.OrderBy(x => x.MatchingMedia?.Name).ToList();
                for (int o = 0; o < ordered.Count; o++)
                {
                    var cm = ordered[o];

                    var beatenCount = 0;


                    // Find OG Game
                    var ogs = collection.Where(x => x.GameKey == cm.MatchingMedia?.RemakeOf).ToList();

                    CollectionGame ogGame;
                    if(ogs.Count == 0)
                    {
                        ogGame = cm;
                    }
                    else 
                    {
                        ogGame = ogs[0];
                    }

                    // todo change this from collection to ALL games
                    //        var allAlikeGames = collection.Where(x => x.MatchingMedia?.RemakeOf == ogGame.GameKey || x.MatchingMedia?.Name.ToLower() == ogGame.MatchingMedia.Name.ToLower()).ToList();
                    var allAlikeGames = LoadedData.AllGames.Where(x => x.RemakeOf == ogGame.GameKey || x.GameKey == ogGame.MatchingMedia.RemakeOf || x.Name.ToLower() == ogGame.MatchingMedia.Name.ToLower()).ToList();

                    if (allAlikeGames.Where(x=>x.GameKey == ogGame.GameKey).ToList().Count == 0)
                        allAlikeGames.Add(ogGame.MatchingMedia);


                    bool finished = false;

                    for (int ag = 0; ag < allAlikeGames.Count; ag++)
                    {
                        var game = allAlikeGames[ag];



                        // Get all Played/Beated for this game
                        var playedGames = pgDtos.Where(x => x.GameKey == game.GameKey).ToList();
                        if (playedGames.Count > 0)
                        {

                            for (int pgi = 0; pgi < playedGames.Count; pgi++)
                            {
                                var playedGameDto = playedGames[pgi];
                                var beat = LoadedData.PercentageList.FirstOrDefault(x => x.ItemKey == playedGameDto.PercentCompleted)?.Beat;
                                var fin = LoadedData.PercentageList.FirstOrDefault(x => x.ItemKey == playedGameDto.PercentCompleted)?.Finished;

                                if (beat == 1)
                                    beatenCount++;

                                if (fin == 1)
                                    finished = true;
                            }



                        }
                    }



                    cm.TimesBeat = beatenCount;
                    cm.Finished = Utilities.General.BoolToInt(finished);


                    // INSERT COLLECTION GAME
                    this.Add(cm);
                }


            }


        }

        public static void RemoveFromCollection(CollectionGame cMedia)
        {
            DBFunctions.RunQuery($"DELETE FROM Collection WHERE CollectionKey = '{cMedia.CollectionKey}'");

        }



    }
}
