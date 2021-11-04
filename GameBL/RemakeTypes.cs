using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBL
{
    public class RemakeType
    {
        public int Key { get; set; }
        public string Type { get; set; }
        public string TypeDescription { get; set; }
    }


    public class RemakeTypes : List<RemakeType>
    {
        public RemakeTypes()
        {
            Load();
        }

        public static string KeyToName(int key)
        {
           return LoadedData.RemakeTypeList.FirstOrDefault(x => x.Key == key)?.Type;
        }



        public void Load()
        {
            this.Clear();

            var type0 = new RemakeType
            {
                Key = 0,
                Type = "Original",
                TypeDescription = "The original."
            };
            this.Add(type0);

            var type1 = new RemakeType
            {
                Key = 1,
                Type = "Remake",
                TypeDescription = "Remaking the game from the ground up to replicate the original."
            };
            this.Add(type1);

            var type2 = new RemakeType
            {
                Key = 2,
                Type = "Remaster",
                TypeDescription = "Taking the original game and just updating the graphics."
            };
            this.Add(type2);

            var type3 = new RemakeType
            {
                Key = 3,
                Type = "Port",
                TypeDescription = "The same game as original, just ported to another system."
            };
            this.Add(type3);

            var type4 = new RemakeType
            {
                Key = 4,
                Type = "Update",
                TypeDescription = "An updated version of the original."
            };
            this.Add(type4);

        }

    }
}
