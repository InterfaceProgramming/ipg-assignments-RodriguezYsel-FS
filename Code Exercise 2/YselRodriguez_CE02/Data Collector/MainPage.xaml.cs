using System;
using System.ComponentModel;
using Xamarin.Forms;
using System.IO;

namespace Data_Collector
{
    public partial class MainPage : ContentPage
    {
        //declare variables and initialize as necessary
        String saveFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "info.txt");
        String savedName = "";
        String savedGender = "";
        DateTime savedDate;
        String savedMarried = "";

        //info class will be used to hold the data members
        Info info = new Info();

        public MainPage()
        {
            InitializeComponent();

            //by default, hide edit button
            editButton.IsVisible = false;
            //check if saved file exists and hide load button if it doesn't
            IfSaveFileExists();
        }

        public MainPage(Info info = null)
        {
            InitializeComponent(); 
            
            if (info != null)
            {
                // saved information was passed, update fields
                this.info = info;
                UpdateFields(info);
            }
        }

        public void UpdateFields(Info info)
        {
            //set the GUI to show the info
            nameLabel.Text = "Name:  " + info.savedName;
            genderLabel.Text = "Gender: " + info.savedGender;
            dateLabel.Text = "Date of Birth: " + info.savedDate;
            marriedLabel.Text = "Married: " + info.savedMarried;
            ageLabel.Text = "Age: " + calculateAge(info.savedDate);
        }

        public void OnNewButtonClicked(object sender, EventArgs args)
        {
            //redirect to new page for user to enter data
            App.Current.MainPage = new NewPage();
        }

        public void OnSaveButtonClicked(object sender, EventArgs args)
        {
            try
            {
                //save the data to a file
                var saveFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "info.txt");
                StreamWriter writer = new StreamWriter(saveFile);

                writer.WriteLine("name=" + info.savedName);
                writer.WriteLine("gender=" + info.savedGender);
                writer.WriteLine("date=" + info.savedDate);
                //recalculate age as it may have increased since data was saved
                writer.WriteLine("age=" + calculateAge(info.savedDate));
                writer.WriteLine("married=" + info.savedMarried);
                writer.Close();

                displayStatus("success", "Data has been saved");
            }
            catch (Exception e)
            {
                //an error occured during saving
                displayStatus("error", "An error occured during saving");
            }
        }
        public void OnEditButtonClicked(object sender, EventArgs args)
        {
            //redirect to new page, passing data that will populate the fields for editing
            App.Current.MainPage = new NewPage(info);
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
                //also update that data variable that holds the information
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith("name="))
                    {
                        savedName = line.Split('=')[1].Trim();
                        nameLabel.Text = "Name: " + savedName;
                        info.savedName = savedName;
                    }
                    else if (line.StartsWith("gender="))
                    {
                        savedGender = line.Split('=')[1].Trim();
                        genderLabel.Text = "Gender: " + savedGender;
                        info.savedGender = savedGender;
                    }
                    else if (line.StartsWith("date="))
                    {    
                        try
                        {
                            DateTime.TryParse(line.Split('=')[1].Trim(), out savedDate);
                            dateLabel.Text = "Date of Birth: " + savedDate.ToString();
                            info.savedDate = savedDate;
                        }

                        catch (Exception e)
                        {
                        }
                    }
                    else if (line.StartsWith("age="))
                    {
                        //recalculate age as it may have changed
                        ageLabel.Text = "Age: " + calculateAge(savedDate);
                    }
                    else if (line.StartsWith("married="))
                    {
                        savedMarried = line.Split('=')[1].Trim();
                        marriedLabel.Text = "Married: " + savedDate;
                        info.savedMarried = savedMarried;
                    }
                }
                //close the file
                reader.Close();

                IfSaveFileExists();
                //make edit button visible
                editButton.IsVisible = true;

                displayStatus("success", "data successfully loaded");

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                displayStatus("error", "error occured. could not load saved data");
            }
        }
        public void OnClearButtonClicked(object sender, EventArgs args)
        {
            //clear fields
            clearFields();
        }

        public void clearFields() {
            //reset the name label
            nameLabel.Text = "Name:";

            //reset the gender label
            genderLabel.Text = "Gender:";

            //reset the date of birth
            dateLabel.Text = "Date of Birth:";

            //reset the age label
            ageLabel.Text = "Age: Undetermined";

            //reset married checkbox
            marriedLabel.Text = "Married:";

            //delete savedFile
            if (File.Exists(saveFile))
            {
                File.Delete(saveFile);
            }

            //hide load button
            IfSaveFileExists();
            //since fields have been cleared, edit button shouldn't be visible
            editButton.IsVisible = false;

            displayStatus("success", "Data reset successfully");
        }
        public void IfSaveFileExists() {
            //check if saved file exists and hide load button
            if (File.Exists(saveFile))
            {
                loadButton.IsVisible = true;
                editButton.IsVisible = false;
            }
            else
            {
                loadButton.IsVisible = false;
                
            }
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

        public int calculateAge(DateTime d)
        {
            int age;

            //subtract user date of birth year from system date year to get their age 
            age = DateTime.Now.Year - d.Year;

            return age;
        }
    }
}
