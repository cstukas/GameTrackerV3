using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBL
{
    public class Genre
    {
        public int GenreKey { get; set; }
        public string GenreName { get; set; }

        public Genre()
        {

        }

        public void Insert()
        {
            DataAccess.DBFunctions.InsertObject<Genre>(this, "Genres");
        }

        public static string KeyToName(int key)
        {
            return Globals.GenreList.FirstOrDefault(x => x.GenreKey == key)?.GenreName;
        }

    }

    public class GenreList : ObservableCollection<Genre>
    {
        public GenreList()
        {
            var sql = "SELECT * FROM Genres ORDER BY GenreName";
            var genres = DataAccess.DBFunctions.LoadList<Genre>(sql);
            for (int i = 0; i < genres.Count; i++)
            {
                this.Add(genres[i]);
            }
        }

    }
}
