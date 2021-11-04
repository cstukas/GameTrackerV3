using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DesktopUI.HelperUI
{
    class AddGenreVM : Utilities.BaseClasses.NotifyPropertyChanged
    {
        public event EventHandler CloseWindowEvent;
        public ICommand SubmitCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        private string newGenre;
        public string NewGenre
        {
            get { return newGenre; }
            set { newGenre = value; OnPropertyChanged("NewGenre"); }
        }

    //    public AddToCollectionVM ParentVM { get; set; }

        public AddGenreVM(AddToCollectionVM parentVM)
        {
            this.SubmitCommand = new DelegateCommand<object>(this.OnSubmit);
            this.CancelCommand = new DelegateCommand<object>(this.OnCancel);

       //     ParentVM = parentVM;

        }

        private void OnSubmit(object obj)
        {
            if(!string.IsNullOrWhiteSpace(NewGenre))
            {
                NewGenre = NewGenre.Trim();

                var result = MessageBox.Show($"Are you sure you want to add {NewGenre} to the public genre list?","Are you sure?", MessageBoxButton.YesNoCancel);

                if(result == MessageBoxResult.Yes)
                {
                    // Check if exists
                    var exists = Utilities.Connection.ExistsInTable<string>("Genres", "GenreName", NewGenre);
                    if(exists)
                    {
                        MessageBox.Show($"{NewGenre} already exists in the genre list.");
                        NewGenre = "";
                        return;
                    }

                    var genre = new GameBL.Genre();
                    genre.GenreKey = Utilities.General.GetNextKey(1, "GenreKey");
                    genre.GenreName = NewGenre;
                    genre.Insert();

                    GameBL.Globals.GenreList.Add(genre);
                //    ParentVM.Genre1 = genre.GenreKey;

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
