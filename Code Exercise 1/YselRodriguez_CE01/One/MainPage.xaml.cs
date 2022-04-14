using System;
using System.ComponentModel;
using Xamarin.Forms;
using System.IO;
//Ysel Rodriguez
//DEV2500
//TermC202204
//CE01:Introduction to Xamarin Forms
namespace One
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public void OnResetButtonClicked(object sender, EventArgs args)
        {
            //reset the name text field
            name.Text = "";

            //unselect all items in the gender radio group
            male.IsChecked = false;
            female.IsChecked = false;
            other.IsChecked = false;

            //reset the date of birth
            date.Date = DateTime.Now;

            //reset the age label
            age.Text = "Age: Undetermined";

            displayStatus("success", "Data reset successfully");
        }

        public void OnLoadButtonClicked(object sender, EventArgs args)
        {
            //load values from the previously saved file
            try
            {
                var saveFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "info.txt");
                StreamReader reader = new StreamReader(saveFile);

                String line;

                //parse the saved data line by line and update GUI
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith("name="))
                    {
                        string savedName = line.Split('=')[1].Trim();
                        name.Text = savedName;
                    }
                    else if (line.StartsWith("gender="))
                    {
                        string savedGender = line.Split('=')[1].Trim();

                        if (savedGender == "male")
                        {
                            male.IsChecked = true;
                        }
                        else if (savedGender == "female")
                        {
                            female.IsChecked = true;
                        }
                        else
                        {
                            other.IsChecked = true;
                        }
                    }
                    else if (line.StartsWith("date="))
                    {
                        DateTime savedDate;

                        try
                        {
                            DateTime.TryParse(line.Split('=')[1].Trim(), out savedDate);
                            date.Date = savedDate;
                        }

                        catch (Exception e)
                        {
                        }
                    }
                    else if (line.StartsWith("age="))
                    {
                        //no need to do anything as the form will automatically recalculate this
                    }
                }
                //close the file
                reader.Close();
                displayStatus("success", "data successfully loaded");
            }
            catch (Exception e)
            {
                displayStatus("error", "error occured. could not load saved data");
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


            if (dirty == false)
            {
                try
                {
                    //save the data to a file
                    var saveFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "info.txt");
                    StreamWriter writer = new StreamWriter(saveFile);

                    writer.WriteLine("name=" + nameValue);
                    writer.WriteLine("gender=" + genderValue);
                    writer.WriteLine("date=" + dateValue);
                    writer.WriteLine("age=" + ageValue);
                    writer.Close();

                    displayStatus("success", "Data has been saved");
                }
                catch (Exception e)
                {
                    //an error occured during saving
                    displayStatus("error", "An error occured during saving");
                }
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

        public void displayStatus(String type, String message)
        {
            //this displays the status of the most recent operation conveniently
            if (type == "success")
            {
                status.TextColor = Color.Green;
                status.Text = message;
            }
            else if (type == "error")
            {
                status.TextColor = Color.Red;
                status.Text = message;
            }
        }
    }
}
