using GameBL;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DesktopUI
{

    public class LoginVM : Utilities.BaseClasses.NotifyPropertyChanged
    {
        public event EventHandler CloseWindowEvent;

        public ICommand LoginCommand { get; private set; }


        private string userName;
        public string UserName
        {
            get { return userName; }
            set { userName = value; OnPropertyChanged("UserName"); }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set { password = value; OnPropertyChanged("Password"); }
        }

        private bool remember = true;
        public bool Remember
        {
            get { return remember; }
            set { remember = value; OnPropertyChanged("Remember"); }
        }


        public string Identifier { get; set; }

        public bool LoadedFromDB { get; set; } = false;


        public LoginVM()
        {
            this.LoginCommand = new DelegateCommand<object>(this.OnLogin);

            Identifier = NetworkInterface.GetAllNetworkInterfaces().Where(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                                          .Select(nic => nic.GetPhysicalAddress().ToString()).FirstOrDefault();

            var rLogin = new RememberLogin();
            rLogin = RememberLogin.Load(Identifier);

            if(rLogin != null)
            {
                UserName = rLogin.LoginName;   
                Password = rLogin.LoginPassword;
                LoadedFromDB = true;
            }

        }

        private void OnLogin(object obj)
        {
            if(string.IsNullOrWhiteSpace(UserName))
            {
                return;
            }

            bool isAuthed = Utilities.UserUtils.Login(UserName, Password);
            if (isAuthed)
            {
                Mouse.OverrideCursor = Cursors.Wait;

                var rLogin = new RememberLogin();
                rLogin.PcIdentifier = Identifier;

                // save info
                if (Remember)
                {
                    if(LoadedFromDB)
                        rLogin.Delete();

                    rLogin.LoginName = UserName;
                    rLogin.LoginPassword = Password;
                    rLogin.Insert();
                }
                else
                {
                    rLogin.Delete();
                }

                var login = new Login();
                login.UserKey = Utilities.UserUtils.CurrentUser.UserKey;
                login.UserName = Utilities.UserUtils.CurrentUser.UserName;
                login.LoginDateTime = DateTime.Now;
                login.Insert();

                var window = new SplashWindow();
                window.Show();
                CloseWindowEvent?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                MessageBox.Show($"User name or password is wrong.");
                UserName = "";
                Password = "";
            }



        }
    }

}

