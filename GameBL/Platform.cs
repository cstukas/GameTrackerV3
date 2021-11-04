using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBL
{
    [Serializable]
    public class Platform
    {
        public int PlatformKey { get; set; }
        public string Name { get; set; }

        public Platform()
        {

        }

        public Platform(int key, string name)
        {
            PlatformKey = key;
            Name = name;
        }

        public static string KeyToName(int key)
        {
            return LoadedData.PlatformList.FirstOrDefault(x => x.PlatformKey == key)?.Name;
        }
    }
}
