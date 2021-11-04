using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBL
{
    public static class Globals
    {
        public static bool SkipLogin { get; set; } = true;
        public static string SkipLoginUser { get; set; } = "craig";
        public static bool RefreshPlayed { get; set; } = false;
        public static bool RefreshCollection { get; set; } = false;
        public static bool UpdateAvailable { get; set; } = false;
        public static bool ShowKeys { get; set; }

        public static GenreList GenreList{ get; set; }

    }
}
