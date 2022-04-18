using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MessagingCenter
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InputPage : ContentPage
    {
        //declare variables and initialize as necessary
        String saveFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "info.txt");
        bool editing = false;
        
        public InputPage()
        {
            InitializeComponent();
        }

        //another constructor, this is used when there's existing information
        public InputPage(Info info = null)
        {
            InitializeComponent();

            if (info != null)
            {
                // saved information was passed, update fields
                name.Text = info.name;
                date.Date = info.date;
                time.Time = info.time;
                editing = true;
            }
        }

        public async void OnSaveButtonClicked(object sender, EventArgs args)
        {
            //this will keep track of form state 
            bool dirty = false;

            String nameValue = "";
            DateTime dateValue = DateTime.Now;
            TimeSpan timeValue;

            //get name
            try
            {
                if (name.Text.Length == 0)
                {
                    await DisplayAlert("Validation Failed", "We need your name", "OK");
                }
                else
                {
                    nameValue = name.Text;
                }
            }
            catch (Exception e)
            {
                await DisplayAlert("Validation Failed", "We need your name", "OK");
                dirty = true;
            }

            //get date
            if (dirty == false)
            {
                dateValue = date.Date;

                //check if date is in the past
                if (dateValue < DateTime.Now)
                {
                    //user is too young
                    await DisplayAlert("Validation Failed", "Validation Failed. Event cannot be in the past", "OK");
                    dirty = true;
                }
            }

            //get time
            if (dirty == false)
            {
                timeValue = time.Time;
            }

            if (dirty == false)
            {
                MainPage mainPage = new MainPage();

                Info info = new Info();
                info.name = nameValue;
                info.date = dateValue;
                info.time = timeValue;

                //set photo depending on day of the week
                info.photo = info.date.DayOfWeek.ToString().ToLower() + ".png";

                if (editing == true)
                {
                    bool answer = await DisplayAlert("Edit", "Are you sure you want to make these changes?", "YES", "NO");
                    if (answer == false)
                    {
                        //navigate to MainPage without saving
                        App.Current.MainPage = mainPage;
                    }
                    else {
                        //send updated info to Main Page
                        Xamarin.Forms.MessagingCenter.Send<InputPage, Info>(this, "edit", info);

                        //navigate to MainPage
                        App.Current.MainPage = mainPage;
                    }
                }
                else
                {
                    //send data to MainPage
                    Xamarin.Forms.MessagingCenter.Send<InputPage, Info>(this, "save", info);

                    //navigate to MainPage
                    App.Current.MainPage = mainPage;
                }
            }
        }

        public async void OnDeleteButtonClicked(object sender, EventArgs args)
        {
            bool answer = await DisplayAlert("Delete", "Are you sure you want to delete event?", "YES", "NO");

            if (answer == true)
            {
                //delete savedFile
                if (File.Exists(saveFile))
                {
                    File.Delete(saveFile);

                    //inform Main Page data has been deleted
                    MainPage mainPage = new MainPage(); 
                    Xamarin.Forms.MessagingCenter.Send<InputPage>(this, "delete");
                    //redirect to Main Page
                    App.Current.MainPage = mainPage;
                }
            }
        }

        private void Date_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }
        private void Time_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }
    }
}