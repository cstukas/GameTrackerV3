using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Prism.Commands;

namespace DesktopUI.HelperUI
{
    class AddPlatformVM : Utilities.BaseClasses.NotifyPropertyChanged
    {
        public event EventHandler CloseWindowEvent;
        public ICommand SubmitCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        private string newPlatform;
        public string NewPlatform
        {
            get { return newPlatform; }
            set { newPlatform = value; OnPropertyChanged("NewPlatform"); }
        }

        private int yearReleased;
        public int YearReleased
        {
            get { return yearReleased; }
            set { yearReleased = value; OnPropertyChanged("YearReleased"); }
        }



        public AddPlatformVM()
        {
            this.SubmitCommand = new DelegateCommand<object>(this.OnSubmit);
            this.CancelCommand = new DelegateCommand<object>(this.OnCancel);
        }

        private void OnSubmit(object obj)
        {
            if (!string.IsNullOrWhiteSpace(NewPlatform))
            {
                NewPlatform = NewPlatform.Trim();

                var result = MessageBox.Show($"Are you sure you want to add {NewPlatform} to the public platforms list?", "Are you sure?", MessageBoxButton.YesNoCancel);

                if (result == MessageBoxResult.Yes)
                {
                    // Check if exists
                    var exists = Utilities.Connection.ExistsInTable<string>("Genres", "GenreName", NewPlatform);
                    if (exists)
                    {
                        MessageBox.Show($"{NewPlatform} already exists in the platform list.");
                        NewPlatform = "";
                        return;
                    }

                    var plat = new GameBL.Platform();
                    plat.PlatformKey = Utilities.General.GetNextKey(1, "PlatformKey");
                    plat.Name = NewPlatform;
                    plat.YearReleased = YearReleased;
                    plat.Insert();

                    GameBL.LoadedData.PlatformList.Add(plat);

                    CloseWindowEvent?.Invoke(null, EventArgs.Empty);
                }
                else if (result == MessageBoxResult.No)
                {

                }
                else if (result == MessageBoxResult.Cancel)
                {
                    CloseWindowEvent?.Invoke(null, EventArgs.Empty);
                }

            }


        }

        private void OnCancel(object obj)
        {
            CloseWindowEvent?.Invoke(null, EventArgs.Empty);
        }


    }
}
