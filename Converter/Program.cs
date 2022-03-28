using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Converter
{
    //class Percentages
    //{
    //    public int ItemKey { get; set; }
    //    public string Name { get; set; }
    //    public int Beat { get; set; }
    //    public int Finished { get; set; }
    //}

    //class Platform
    //{
    //    public int PlatformKey { get; set; }
    //    public string Name { get; set; }
    //}

    class TopGame
    {
        public int GameKey { get; set; }
        public int OrderNumber { get; set; }
        public int Rating { get; set; }
    }


    class Program
    {
        //static void Insert(int key, string name)
        //{
        //    var p = new Platform();
        //    p.PlatformKey = key;
        //    p.Name = name;
        //    DataAccess.DBFunctions.InsertObject(p, "Platforms");
        //}

        static void Insert(int key, int order, int rating)
        {
            var tg = new TopGame();
            tg.GameKey = key;
            tg.OrderNumber = order;
            tg.Rating = rating;
            DataAccess.DBFunctions.InsertObject(tg, "TopRatedGames");
        }

        static void Main(string[] args)
        {
            var file = @"C:\Users\craigs\Documents\Personal\TopGames.txt";

            



            //DataAccess.DBFunctions.RunQuery("DELETE FROM Platforms");

            //int key = 1;
            //Insert(key, "3DS");
            //key++;
            //Insert(key, "DS");
            //key++;
            //Insert(key, "GameCube");
            //key++;
            //Insert(key, "GBA");
            //key++;
            //Insert(key, "GBC");
            //key++;
            //Insert(key, "Mobile");
            //key++;
            //Insert(key, "N64");
            //key++;
            //Insert(key, "NES");
            //key++;
            //Insert(key, "PC");
            //key++;
            //Insert(key, "Emulator");
            //key++;
            //Insert(key, "PS Vita");
            //key++;
            //Insert(key, "PS1");
            //key++;
            //Insert(key, "PS2"); 
            //key++;
            //Insert(key, "PS3"); 
            //key++;
            //Insert(key, "PS4"); 
            //key++;
            //Insert(key, "PSP"); 
            //key++;
            //Insert(key, "PSVR"); 
            //key++;
            //key++;
            //Insert(key, "Sega"); 
            //key++;
            //Insert(key, "SNES"); 
            //key++;
            //Insert(key, "Switch"); 
            //key++;
            //Insert(key, "Wii"); 
            //key++;
            //Insert(key, "Wii U"); 
            //key++;
            //Insert(key, "Xbox 360"); 
            //key++;
            //Insert(key, "Xbox One"); 
            //key++;
            //Insert(key, "Xbox");
            //key++;
            //Insert(key, "PS5"); 
            //key++;
            //Insert(key, "Xbox Series X");

        }
    }
}
