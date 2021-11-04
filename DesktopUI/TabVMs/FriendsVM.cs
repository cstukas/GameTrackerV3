using GameBL;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DesktopUI.TabVMs
{
    public class FriendsVM : Utilities.BaseClasses.NotifyPropertyChanged
    {
        public ICommand FriendClickedCommand { get; private set; }
        public ICommand SwapFriendCommand { get; private set; }

        public MainVM ParentVM { get; set; }


        private ObservableCollection<Utilities.User> allFriends;
        public ObservableCollection<Utilities.User> AllFriends
        {
            get { return allFriends; }
            set { allFriends = value; OnPropertyChanged("AllFriends"); }
        }

        private ObservableCollection<Utilities.User> otherUsers;
        public ObservableCollection<Utilities.User> OtherUsers
        {
            get { return otherUsers; }
            set { otherUsers = value; OnPropertyChanged("OtherUsers"); }
        }

        private Utilities.User selectedUser;
        public Utilities.User SelectedUser
        {
            get { return  selectedUser; }
            set { selectedUser = value; OnPropertyChanged("SelectedUser"); }
        }

        public FriendsVM()
        {
            this.FriendClickedCommand = new DelegateCommand<object>(this.OnFriendClicked);
            this.SwapFriendCommand = new DelegateCommand<object>(this.OnSwapUser);

            AllFriends = new ObservableCollection<Utilities.User>();
            OtherUsers = new ObservableCollection<Utilities.User>();

            for (int af = 0; af < Utilities.UserUtils.Friends.Count; af++)
            {
                var friend = Utilities.UserUtils.Friends[af];
                AllFriends.Add(friend);
            }

            OtherUsers.Clear();
            var remainingUsers = Utilities.UserUtils.AllUsers.Where(x => x.UserKey != 1 && x.UserKey != Utilities.UserUtils.CurrentUser.UserKey).ToList();
            for (int f = 0; f < AllFriends.Count; f++)
            {
                var friend = AllFriends[f];
                if (remainingUsers.Contains(friend))
                    remainingUsers.Remove(friend);
            }

            for (int ru = 0; ru < remainingUsers.Count; ru++)
            {
                var friend = remainingUsers[ru];
                OtherUsers.Add(friend);
            }

        }

        private void OnSwapUser(object obj)
        {
            SwapUser(SelectedUser);
        }

        /// <summary>
        /// Either adds the user as your friend or removes them as a friend
        /// Returns: 0 = no longer friends, 1 = friends now
        /// </summary>
        /// <param name="newFriend"></param>
        /// <returns></returns>
        public int SwapUser(Utilities.User newFriend, bool showMessageBoxOnRemove = false)
        {
            Mouse.OverrideCursor = Cursors.Wait;

            int areNowFriends = -1;

            if (newFriend == null)
            {
                Mouse.OverrideCursor = null;
                return areNowFriends;

            }

            if (AllFriends.Contains(newFriend))
            {
                if(showMessageBoxOnRemove)
                {
                    var result = MessageBox.Show($"Are you sure you want to remove {newFriend.UserName} as a friend?", "Really?", MessageBoxButton.YesNo);
                    if(result == MessageBoxResult.No)
                    {
                        Mouse.OverrideCursor = null;
                        return -1;
                    }

                }

                OtherUsers.Add(newFriend);
                AllFriends.Remove(newFriend);
                areNowFriends = 0;
            }
            else if (OtherUsers.Contains(newFriend))
            {
                AllFriends.Add(newFriend);
                OtherUsers.Remove(newFriend);
                areNowFriends = 1;

            }

            // Update Friends string
            var newFriendsString = Utilities.UserUtils.CreateFriendsString(AllFriends.ToList());
            Utilities.UserUtils.CurrentUser.Friends = newFriendsString;

            // Update DB
            Utilities.UserUtils.UpdateFriendsList(Utilities.UserUtils.CurrentUser.UserKey, newFriendsString);

            Mouse.OverrideCursor = null;


            return areNowFriends;
        }

        private void OnFriendClicked(object obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            ParentVM.ProfileToView = SelectedUser;
            ParentVM.SelectedTabIndex = 2;
            Mouse.OverrideCursor = null;

        }

    }
}
