using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;

namespace GameBL
{
    [Serializable]
    public class SeriesItem
    {
        public int SeriesKey { get; set; }
        public string Name { get; set; }
        public string Subscribers { get; set; }
    }

    [Serializable]
    public class Series : ObservableCollection<SeriesItem>
    {
        public Series()
        {
            this.Clear();
            var series = DBFunctions.LoadList<SeriesItem>("SELECT * FROM Series ORDER BY Name");
            for (int i = 0; i < series.Count; i++)
            {
                this.Add(series[i]);
            }

        }
    }


    public class SeriesType
    {
        public int SeriesTypeKey { get; set; }
        public string Name { get; set; }
    }

    public class SeriesTypeList : List<SeriesType>
    {
        public SeriesTypeList()
        {
            this.Clear();
            this.Add(new SeriesType { SeriesTypeKey = 0, Name = "" });
            this.Add(new SeriesType { SeriesTypeKey = 1, Name = "2D" });
            this.Add(new SeriesType { SeriesTypeKey = 2, Name = "3D" });

        }
    }
}
