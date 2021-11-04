using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DesktopUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            FrameworkCompatibilityPreferences.KeepTextBoxDisplaySynchronizedWithTextProperty = false;

            // Start
            base.OnStartup(e);

            Window startScreen;
            if(GameBL.Globals.SkipLogin)
            {
                bool isAuthed = Utilities.UserUtils.LoginNoPassword(GameBL.Globals.SkipLoginUser);
                startScreen = new SplashWindow();
            }
            else
            {
                startScreen = new LoginWindow();
            }
            startScreen.Show();


        }
    }
}
