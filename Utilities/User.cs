using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class User : Utilities.BaseClasses.NotifyPropertyChanged
    {
        public string UserName { get; set; }
        public int UserKey { get; set; }

        private string friends;
        public string Friends
        {
            get { return friends; }
            set { friends = value; OnPropertyChanged("Friends"); }
        }

    }
}
