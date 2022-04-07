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

        public static List<TopGame> GamesInserted { get; set; }

        static void Insert(int key, int order, int rating)
        {
            var tg = new TopGame();
            tg.GameKey = key;
            tg.OrderNumber = order;
            tg.Rating = rating;

            GamesInserted.Add(tg);

            DataAccess.DBFunctions.InsertObject(tg, "TopRatedGames");
        }

        static void Main(string[] args)
        {
            var file = @"C:\Users\craigs\Documents\TopRatedGames.txt";
            GamesInserted = new List<TopGame>();
            int Key = -1;
            int Order = -1;
            int Rating = -1;
            int Year = -1;
            string gameName = "";
            int PlatformKey = -1;

            int counter = 0;
            int count2 = 0;

            bool valid = true;
            foreach (string line in System.IO.File.ReadLines(file))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                // Rating
                if(counter == 0)
                {
                    bool res;
                    res = int.TryParse(line, out Rating);
                }

                // Order Num
                if (counter == 1)
                {
                    var newLine = line.Replace(".","");

                    bool res;
                    res = int.TryParse(newLine, out Order);
                }

                // Name
                if (counter == 2)
                {
                    string newLine = line;
                    if (line.StartsWith("The Legend of Zelda"))
                    {
                        newLine = line.Substring(4);
                    }

                    newLine = newLine.Replace("'", "");
                    if (newLine == "Grand Theft Auto IV")
                        newLine = "Grand Theft Auto 4";

                    if (newLine == "Grand Theft Auto V")
                        newLine = "Grand Theft Auto 5";

                    if (newLine == "Grand Theft Auto III")
                        newLine = "Grand Theft Auto 3";

                    if (newLine == "Metal Gear Solid 3: Subsistence")
                        newLine = "Metal Gear Solid 3: Snake Eater";
                    
                    if (newLine == "Street Fighter IV")
                        newLine = "Street Fighter 4";


                    newLine = newLine.Replace("Tom Clancys ", "");
                    newLine = newLine.Replace("Bros.", "Bros");
                    newLine = newLine.Replace("Half-Life", "Half Life");
                    newLine = newLine.Replace("Part II", "Part 2");

                    newLine = newLine.Replace("Hawks", "Hawk");

                    if (newLine == "The Elder Scrolls V: Skyrim")
                        newLine = "The Elder Scrolls 5: Skyrim";

                    if (newLine == "The Elder Scrolls IV: Oblivion")
                        newLine = "The Elder Scrolls 4: Oblivion";

                    if (newLine == "Mario Kart Super Circuit")
                        newLine = "Mario Kart: Super Circuit";

                    if (newLine == "Banjo-Kazooie")
                        newLine = "Banjo Kazooie";

                    if (newLine == "Legend of Zelda: A Link to the Past")
                        newLine = "Legend of Zelda: Link to the Past";

                    if (newLine == "The Orange Box") valid = false;
                    if (newLine == "Grand Theft Auto Double Pack") valid = false;
                    if (newLine == "Legend of Zelda Collectors Edition") valid = false;
                    if (newLine == "The Last of Us Remastered") valid = false;
                    if (newLine == "Legend of Zelda: Ocarina of Time 3D") valid = false;

                    newLine.Trim();
                    var keys = DataAccess.DBFunctions.LoadList<int>($"SELECT GameKey FROM Game WHERE Name = '{newLine}'");
                    if (keys.Count > 0)
                        Key = keys[0];


                    gameName = newLine;
                }

                // Platform
                if (counter == 3)
                {
                    var newLine = line.Split(':')[1].Trim();
                    PlatformKey = DataAccess.DBFunctions.LoadObject<int>($"SELECT PlatformKey FROM Platforms WHERE Name = '{newLine}' OR FullName = '{newLine}'");
                }

                // Year
                if (counter == 4)
                {
                    var newLine = line.Split(',')[1];
                    var year = newLine.Replace("Expand", "").Trim();


                    bool res;
                    res = int.TryParse(year, out Year);
                }

                counter++;

                if (counter >= 5)
                {

                    if(valid)
                    {
                        var str = $"#{Order} - Rating: {Rating} - Year: {Year} - Name: {gameName}";
                        if (Key > 0)
                            str += $" - GameKey: {Key}";
                        else
                            str += " - NEW GAME";


                        var keeeey = Key;
                        if (Key <= 0)
                        {
                            try
                            {
                                var newGame = new GameBL.Game();
                                newGame.Name = gameName;
                                newGame.GameKey = DataAccess.DBFunctions.GetNextKey(1, "GameKey");
                                keeeey = newGame.GameKey;
                                newGame.Platform = PlatformKey;
                                newGame.YearReleased = Year;
                                newGame.DateAdded = DateTime.Now;

                                newGame.Insert();
                            }
                            catch (Exception e)
                            {


                            }


                        }
                        else
                        {

                        }

                        if(keeeey > 0)
                        {
                            var match = GamesInserted.FirstOrDefault(x => x.GameKey == Key);
                            if (match == null)
                            {
                                Insert(keeeey, Order, Rating);
                                Console.WriteLine(str);

                            }
                        }







                    }




                    Key = -1;
                    Order = -1;
                    Rating = -1;
                    Year = -1;
                    gameName = "";
                    PlatformKey = -1;

                    counter = 0;

                    count2++;
                    valid = true;

                }

                if(count2 >= 5)
                {
                   // Console.ReadKey();
                    count2 = 0;
                }

            }


            Console.WriteLine("DONE");
            Console.ReadKey();
        }

    }
}
