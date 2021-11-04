using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBL
{
    public class Login
    {
        public int UserKey { get; set; }
        public string UserName { get; set; }
        public DateTime LoginDateTime { get; set; }

        public void Insert()
        {
            DataAccess.DBFunctions.InsertObject(this, "Logins");
        }
    }
    public class RememberLoginDto
    {
        public string PcIdentifier { get; set; }
        public string LoginName { get; set; }
        public string LoginPassword { get; set; }
    }

    public class RememberLogin
    {
        public string PcIdentifier { get; set; }
        public string LoginName { get; set; }
        public string LoginPassword { get; set; }


        public static RememberLogin Load(string ident)
        {
            var item = DataAccess.DBFunctions.LoadObject<RememberLoginDto>($"SELECT * FROM RememberLogin WHERE PcIdentifier = '{ident}'");
            if (item != null)
            {
                return Utilities.General.Map<RememberLoginDto, RememberLogin>(item);
            }
            else
                return null;
        }


        public void Insert()
        {
            var dto = Utilities.General.Map<RememberLogin, RememberLoginDto>(this);
            DataAccess.DBFunctions.InsertObject(dto, "RememberLogin");
        }

        public void Delete()
        {
            var sql = $"DELETE FROM RememberLogin WHERE PcIdentifier = '{this.PcIdentifier}'";
            DataAccess.DBFunctions.RunQuery(sql);
        }
    }
}
