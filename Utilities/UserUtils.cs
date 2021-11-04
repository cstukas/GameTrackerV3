using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;

namespace Utilities
{
    public static class UserUtils
    {
        public static User CurrentUser { get; set; }

        public static List<User> AllUsers { get; set; }
        public static List<User> Friends { get; set; }


        public static void LoadAllUsers()
        {
            AllUsers = new List<User>();
            var sql = "SELECT * FROM Users";
            var users = DBFunctions.LoadList<User>(sql);
            for (int i = 0; i < users.Count; i++)
            {
                AllUsers.Add(users[i]);
            }
        }

        public static User GetUserFromUserKey(int userKey)
        {
            return AllUsers.FirstOrDefault(x => x.UserKey == userKey);
        }

        public static void LoadFriends()
        {
            var friendsKeys = GetFriendsListKeys(CurrentUser);
            Friends = new List<User>();

            for (int i = 0; i < friendsKeys.Count; i++)
            {
                var key = friendsKeys[i];
                var user = AllUsers.FirstOrDefault(x => x.UserKey == key);
                Friends.Add(user);
            }


        }

        public static string GetFriendsSql(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Friends))
                return $" AND UserKey = {user.UserKey} ";

            var fs = user.Friends.Split(',');
            var friendsSql = $" AND (UserKey = {user.UserKey} ";
            for (int i = 0; i < fs.Length; i++)
            {
                var friend = fs[i].Trim();
                if (friend != "")
                {
                    friendsSql += " OR";
                    friendsSql += $" UserKey = {friend}";
                }
            }

            friendsSql += ") ";
            return friendsSql;
        }

        public static List<int> GetFriendsListKeys(User user)
        {
            // Find friends
            var keys = new List<int>();
            if (user.Friends != null)
            {
                var friends = user.Friends.Split(',');
                for (int i = 0; i < friends.Length - 1; i++)
                {
                    var friendKey = Convert.ToInt32(friends[i]);
                    keys.Add(friendKey);
                }
            }
            return keys;

        }

        public static string CreateFriendsString(List<User> friends)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < friends.Count; i++)
            {
                var friend = friends[i];
                sb.Append($"{friend.UserKey},");
            }
            return sb.ToString();

        }

        public static string ConvertUserKeyToName(int key)
        {
            return AllUsers.FirstOrDefault(x => x.UserKey == key)?.UserName;

        }

        public static bool Login(string name, string password)
        {
            CurrentUser = DBFunctions.LoadObject<User>($"SELECT * FROM Users WHERE LoginName = '{name.ToLower()}' AND Password = '{password}'");
            if (CurrentUser != null)
                return true;
            else
                return false;
        }

        public static bool LoginNoPassword(string name)
        {
            Console.WriteLine("LOGGING IN");
            CurrentUser = DBFunctions.LoadObject<User>($"SELECT * FROM Users WHERE LoginName = '{name.ToLower()}'");
            if (CurrentUser != null)
                return true;
            else
                return false;
        }


        public static void  UpdateFriendsList(int userKey, string friendsList)
        {
            var sql = $"UPDATE Users SET Friends = '{friendsList}' WHERE UserKey = {userKey}";
            DBFunctions.RunQuery(sql);
        }

    }
}
