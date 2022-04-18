using System;
using System.IO;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Data_Collector
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewPage : ContentPage
    {
        
        public NewPage()
        {
            InitializeComponent();
        }

        public NewPage(Info info = null) {
            InitializeComponent();
            
            if (info != null) {
                // saved information was passed, update fields
                UpdateFields(info);
            }
        }

        public void OnCancelButtonClicked(object sender, EventArgs args)
        {
            //redirect to main page
            App.Current.MainPage = new MainPage();
        }

        public void UpdateFields(Info info)
        {
            //load values from the previously saved file
            try
            {
                //parse the saved data and update GUI
                name.Text = info.savedName;
                   

                if (info.savedGender == "male")
                {
                    male.IsChecked = true;
                }
                else if (info.savedGender == "female")
                {
                    female.IsChecked = true;
                }
                else
                {
                    other.IsChecked = true;
                }
                
                
                date.Date = info.savedDate;
                 
                if (info.savedMarried == "True")
                        {
                            married.IsChecked = true;
                        }
                        else
                        {
                            married.IsChecked = false;
                        }
            }
            catch (Exception e)
            {
                //no need to do anything, we failed gracefully
            }
        }

        public void OnSaveButtonClicked(object sender, EventArgs args)
        {
            // user has clicked save button
            // save data to file

            //this will keep track of form state 
            bool dirty = false;

            String nameValue = "";
            String genderValue = "";
            DateTime dateValue = DateTime.Now;
            int ageValue = 0;
            bool marriedValue = false;

            //perform data validation and get user input

            //get name
            try
            {
                if (name.Text.Length == 0)
                {
                    DisplayAlert("Validation Failed", "We need your name", "OK");
                }
                else
                {
                    nameValue = name.Text;
                }
            }
            catch (Exception e)
            {
                DisplayAlert("Validation Failed", "We need your name", "OK");
                dirty = true;
            }


            //get gender
            if (dirty == false)
            {
                if (male.IsChecked)
                {
                    genderValue = "male";
                }
                else if (female.IsChecked)
                {
                    genderValue = "female";
                }
                else if (other.IsChecked)
                {
                    genderValue = "other";
                }
                else
                {
                    //no gender selected, inform user
                    DisplayAlert("Validation Failed", "Select a gender", "OK");
                    dirty = true;
                }
            }

            //get date and age
            if (dirty == false)
            {
                dateValue = date.Date;

                ageValue = calculateAge(dateValue);

                //check if user lied about their age
                if (ageValue < 1)
                {
                    //user is too young
                    DisplayAlert("Validation Failed", "Too young. Where is mommy?", "OK");
                    dirty = true;
                }
                else if (ageValue > 120)
                {
                    //user is too old
                    DisplayAlert("Validation Failed", "Too old. Contact Guinness World Book of Records", "OK");
                    dirty = true;
                }
            }

            // get user marriage status
            if (dirty == false)
            {
                marriedValue = married.IsChecked;
            }


            if (dirty == false)
            {
                //send data to MainPage
                Info info = new Info(nameValue, genderValue, dateValue, marriedValue.ToString());
                App.Current.MainPage = new MainPage(info);
            }
        }

        private void date_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //user has selected a date, so calculate age
            var user_age = calculateAge(date.Date);

            //display the age
            age.Text = "Age: " + user_age + " years old";
        }

        public int calculateAge(DateTime d)
        {
            int age;

            //subtract user date of birth year from system date year to get their age 
            age = DateTime.Now.Year - d.Year;

            return age;
        }
    }
}