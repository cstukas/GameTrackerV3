using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBL
{
    

    public static class LoadedData
    {

        public static List<Game> AllGames { get; set; }
        public static PlayedGameList MyPlayedGames { get; set; }
        public static List<Percentages> PercentageList { get; set; }
        public static List<Platform> PlatformList { get; set; }
        public static List<Platform> PlatformListWithAll { get; set; }
        public static RemakeTypes RemakeTypeList { get; set; }

        public static void Load(int userKey)
        {
            MyPlayedGames = new PlayedGameList();
            MyPlayedGames = PlayedGameList.LoadGame(userKey, false, -1, " ORDER BY DateAdded DESC", false,"",0);

            var percs = DataAccess.DBFunctions.LoadList<Percentages>("SELECT * FROM Percentages;");
            PercentageList = new List<Percentages>();
            PercentageList = percs.OrderBy(x => x.Name).ToList();

            PlatformList = DataAccess.DBFunctions.LoadList<Platform>("SELECT * FROM Platforms ORDER BY Name;");
            PlatformListWithAll = Utilities.General.CloneList(PlatformList);
            var all = new Platform()
            {
                Name = "<All>",
                PlatformKey = 0
            };
            PlatformListWithAll.Insert(0, all);

            RemakeTypeList = new RemakeTypes();

        }


        public static void LoadAllGames()
        {
            AllGames = new List<Game>();
            AllGames = DataAccess.DBFunctions.LoadList<Game>("SELECT * FROM Game ORDER BY Name");
        }

        public static void AddGame(Game game)
        {
            AllGames.Add(game);
            AllGames = AllGames.OrderBy(x => x.Name).ToList();
        }





    }

}
