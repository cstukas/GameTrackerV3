using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;

namespace GameBL
{
    public class SeriesStat
    {
        public int SeriesKey { get; set; }
        public string Name { get; set; }


        public int TotalGames { get; set; }
        public int GamesBeat { get; set; }
        public int GamesOwned { get; set; }
        public decimal OwnPercentage { get; set; }
        public decimal BeatPercentage { get; set; }
        public string DisplayOwnPercentage { get; set; }
        public string DisplayBeatPercentage { get; set; }
    }


    public class SeriesStats : ObservableCollection<SeriesStat>
    {


        public SeriesStats(string sort)
        {
            Load(-1, sort);
        }



        void Load(int seriesKey = -1, string sort = "BeatenPercentage")
        {
            this.Clear();

            var newList = new ObservableCollection<SeriesStat>();

            // Series
            List<SeriesItem> series;
            if (seriesKey > 0)
                series = LoadedData.SeriesList.Where(x => x.SeriesKey == seriesKey).ToList();
            else
                series = LoadedData.SeriesList.ToList();

            // Series Games
            List<Game> seriesGames;
            if (seriesKey > 0)
                seriesGames = LoadedData.AllGames.Where(x => x.SeriesKey == seriesKey).ToList();
            else
                seriesGames = LoadedData.AllGames.ToList();


            for (int i = 0; i < series.Count; i++)
            {
                var serie = series[i];
                var stat = new SeriesStat();
                stat.SeriesKey = serie.SeriesKey;
                stat.Name = serie.Name;

                var games = seriesGames.Where(x => x.SeriesKey == stat.SeriesKey).ToList();
                stat.TotalGames = games.Count;

                int beatCount = 0;
                int ownCount = 0;
                for (int j = 0; j < games.Count; j++)
                {
                    var game = games[j];


                    var otherVersions = LoadedData.AllGames.Where(x => x.RemakeOf == game.GameKey).ToList();
                    otherVersions.Add(game);

                    bool own = false;
                    bool beat = false;
                    for (int ov = 0; ov < otherVersions.Count; ov++)
                    {
                        var ovGame = otherVersions[ov];
                        var collGame = LoadedData.MyCollection.FirstOrDefault(x => x.GameKey == ovGame.GameKey);

                        if (collGame != null)
                        {
                            if (collGame.TimesBeat > 0)
                            {
                                beat = true;
                            }

                            if (collGame.Own == 1 || collGame.OwnDigitally == 1)
                            {
                                own = true;

                            }
                            else
                            {
                                //var check = LoadedData.MyCollection.Where(x => x.MatchingMedia.Name.ToLower() == version.MatchingMedia.Name.ToLower() && (x.Own == 1 || x.OwnDigitally == 1)).ToList();
                                //if (check.Count > 0)
                                //{
                                //    own = true;
                                //}
                            }
                        }
                        else // dont own game, check if you played it
                        {
                            var pGame = LoadedData.MyPlayedGames.FirstOrDefault(x => x.GameKey == ovGame.GameKey);
                            if(pGame != null)
                            {
                                if (pGame.Beaten == 1)
                                    beat = true;
                            }

                        }
                    }

                    if (own) ownCount++;
                    if (beat) beatCount++;
                 //   if (own || beat) break;


                    //for (int k = 0; k < allVersions.Count; k++)
                    //{
                    //    var version = allVersions[k];

                    //    bool own = false;
                    //    bool beat = false;

                    //    var myColl = LoadedData.MyCollection.Where(x=>x.GameKey == gameKey)


                    //    if (version.TimesBeat > 0)
                    //    {
                    //        beat = true;
                    //    }

                    //    if (version.Own == 1 || version.OwnDigitally == 1)
                    //    {
                    //        own = true;

                    //    }
                    //    else
                    //    {
                    //        var check = LoadedData.MyCollection.Where(x => x.MatchingMedia.Name.ToLower() == version.MatchingMedia.Name.ToLower() && (x.Own == 1 || x.OwnDigitally == 1)).ToList();
                    //        if (check.Count > 0)
                    //        {
                    //            own = true;
                    //        }
                    //    }



                    //    if (own) ownCount++;
                    //    if (beat) beatCount++;
                    //    if (own || beat) break;
                    //}


                }

                // stat.GamesBeat = games.Where(X => X.TimesBeat > 0).ToList().Count;
                // stat.GamesOwned = games.Where(X => X.Own).ToList().Count;
                stat.GamesBeat = beatCount;
                stat.GamesOwned = ownCount;
                if(stat.TotalGames > 0)
                    stat.OwnPercentage = ((decimal)stat.GamesOwned / (decimal)stat.TotalGames) * 100;

                if (stat.TotalGames > 0)
                    stat.BeatPercentage = ((decimal)stat.GamesBeat / (decimal)stat.TotalGames) * 100;

                stat.DisplayOwnPercentage = FormatPercentage(stat.OwnPercentage, 2);
                stat.DisplayBeatPercentage = FormatPercentage(stat.BeatPercentage, 2);

                if (stat.GamesOwned == stat.TotalGames) stat.DisplayOwnPercentage = "100";
                if (stat.GamesBeat == stat.TotalGames) stat.DisplayBeatPercentage = "100";

                stat.DisplayOwnPercentage += "%";
                stat.DisplayBeatPercentage += "%";

                newList.Add(stat);
            }

            List<SeriesStat> orderedList = new List<SeriesStat>();
            switch (sort.ToLower())
            {
                case "beatenpercentage":
                    orderedList = newList.OrderByDescending(x => x.BeatPercentage).ToList();
                    break;
                case "ownpercentage":
                    orderedList = newList.OrderByDescending(x => x.OwnPercentage).ToList();
                    break;
                case "totalgames":
                    orderedList = newList.OrderByDescending(x => x.TotalGames).ToList();
                    break;
            }

            for (int o = 0; o < orderedList.Count; o++)
            {
                this.Add(orderedList[o]);
            }

        }


        string FormatPercentage(decimal dec, int places)
        {
            var percent = dec.ToString();

            if (percent.Length > places) percent = percent.Substring(0, places);

            return percent;
        }
    }
}
