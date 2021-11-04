using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBL
{
    public class UpdateAvailableDto
    {
        public string UpdateName { get; set; }
        public bool Available { get; set; }
    }

    public class UpdateAvailable
    {

        public static bool UpdateIsAvailable()
        {
            var updateName = "Update1";
            var sql = $"SELECT TOP 1 * FROM AvailableUpdate WHERE UpdateName = '{updateName}'";
            var update = DataAccess.DBFunctions.LoadObject<UpdateAvailableDto>(sql);
            if(update != null)
            {
                return update.Available;

            }
            else
            {
                return false;
            }

        }
    }
}
