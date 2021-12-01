using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;

namespace GameBL
{
    public class SeriesGame : Game
    {
        public int TimesBeat { get; set; }
        public int Own { get; set; }
        public int OwnDigitally { get; set; }
        public string Color { get; set; }


        public SeriesGame()
        {

        }

        public static void UpdateOrderNum(int orderNum, int gameKey)
        {
            DBFunctions.RunQuery($"UPDATE Game SET SeriesOrderNum = {orderNum} WHERE GameKey = '{gameKey}'");
            var thisGame = LoadedData.AllGames.FirstOrDefault(x => x.GameKey == gameKey);
            thisGame.SeriesOrderNum = orderNum;

        }


    }
}
