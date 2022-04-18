using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MessagingCenter
{
    public partial class MainPage : ContentPage
    {
        //declare variables and initialize as necessary
        String saveFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "info.txt");

        String savedName = "";
        DateTime savedDate = new DateTime();
        TimeSpan savedTime = new TimeSpan();
        String savedPhoto = "";


        //this will hold the actively selected post
        public ObservableCollection<Info> selectedInfo = new ObservableCollection<Info>(){
            new Info()
            {name = "", date = new DateTime(), time = new TimeSpan(), photo = ""}
        };
        public MainPage()
        {
            InitializeComponent();

            //check if saved file exists and hide delete all button if it doesn't
            IfSaveFileExists();

            Xamarin.Forms.MessagingCenter.Subscribe<InputPage, Info>(this, "save", (sender, data) =>
            {
                // received message from Input Page
                // new object has been created
                //save data and force refresh the image list
                imageListView.ItemsSource = null;
                selectedInfo[0] = data;
                imageListView.ItemsSource = new[] { data };

                //save to file
                Save(data);

                //show delete button
                IfSaveFileExists();

            });

            Xamarin.Forms.MessagingCenter.Subscribe<InputPage, Info>(this, "edit", (sender, data) =>
            {
                // received message from Input Page
                //data has been edited
                //save and force refresh the image list
                imageListView.ItemsSource = null;
                selectedInfo[0] = data;
                imageListView.ItemsSource = new[] { data };

                //save to file
                Save(data);

                //show delete button
                IfSaveFileExists();

            });

            Xamarin.Forms.MessagingCenter.Subscribe<InputPage>(this, "delete", (sender) =>
            {
                // received message from Input Page
                //data has been deleted
                //save and force refresh the image list
                imageListView.ItemsSource = null;
                selectedInfo[0] = null;

                //hide delete button
                IfSaveFileExists();

            });
        }

        public void ImageList_ItemTapped(object sender, EventArgs e)
        {
            //user has tapped an item in the image list 
            Info selection = (Info)imageListView.SelectedItem;

            //redirect to input page for user to edit the data
            App.Current.MainPage = new InputPage(selection);
        }
        
        public void OnNewButtonClicked(object sender, EventArgs args)
        {
            //redirect to new page for user to enter data
            App.Current.MainPage = new InputPage();
        }

        public async void OnDeleteButtonClicked(object sender, EventArgs args)
        {
            bool answer = await DisplayAlert("Delete", "Are you sure you want to delete event?", "YES", "NO");

            if (answer == true) {
                //clear the imagelist
                selectedInfo[0] = null;

                //delete savedFile
                if (File.Exists(saveFile))
                {
                    File.Delete(saveFile);
                    //hide Delete All button
                    IfSaveFileExists();
                }
            }
        }

        public void IfSaveFileExists()
        {
            //check if saved file exists and hide load button
            if (File.Exists(saveFile))
            {
                deleteButton.IsVisible = true;
                LoadValues();

                //data has been loaded, force refresh the image list
                imageListView.ItemsSource = null;
                imageListView.ItemsSource = selectedInfo;
            }
            else
            {
                //no saved data, hide delete button
                deleteButton.IsVisible = false;

            }
        }

        public void Save(Info info)
        {
            try
            {
                //save the data to a file
                StreamWriter writer = new StreamWriter(saveFile);

                writer.WriteLine("name=" + info.name);
                writer.WriteLine("date=" + info.date);
                writer.WriteLine("time=" + info.time);
                writer.WriteLine("photo=" + info.photo);
                writer.Close();
            }
            catch (Exception e)
            {
                //an error occured during saving, do nothing
            }
        }

        public void LoadValues()
        {
            //load values from the previously saved file
            try
            {
                StreamReader reader = new StreamReader(saveFile);

                String line;

                //parse the saved data line by line and update GUI
                //also update that data variable that holds the information
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith("name="))
                    {
                        savedName = line.Split('=')[1].Trim();
                        selectedInfo[0].name = savedName;
                    }
                    else if (line.StartsWith("date="))
                    {
                        try
                        {
                            DateTime.TryParse(line.Split('=')[1].Trim(), out savedDate);
                            selectedInfo[0].date = savedDate;
                        }

                        catch (Exception e)
                        {
                        }
                    }
                    else if (line.StartsWith("time="))
                    {
                        try
                        {
                            TimeSpan.TryParse(line.Split('=')[1].Trim(), out savedTime);
                            selectedInfo[0].time = savedTime;
                        }

                        catch (Exception e)
                        {
                        }
                    }
                    else if (line.StartsWith("photo="))
                    {
                        savedPhoto = line.Split('=')[1].Trim();
                        selectedInfo[0].photo = savedPhoto;
                    }
                }

                //close the file
                reader.Close();
            }
            catch (Exception e)
            {
                //an error occured during loading the file
            }

        }
    }
}
