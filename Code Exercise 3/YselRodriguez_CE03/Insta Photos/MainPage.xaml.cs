using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Insta_Photos
{
    public partial class MainPage : ContentPage
    {
        //declare variables and initialize as necessary
        String saveFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "info.txt");
        String savedTitle = "";
        String savedComments= "";
        String savedPhoto = "";

        //this will hold the actively selected post
        public ObservableCollection<Post> selectedPost = new ObservableCollection<Post>(){
            new Post()
            {title = "", comments = "", photo = ""}
        };

        //constructor
        public MainPage()
        {
            InitializeComponent();

            //check if saved file exists and hide delete all button if it doesn't
            IfSaveFileExists();
        }

        //another constructore, used when there's existing information
        public MainPage(Post post = null)
        {
            InitializeComponent();

            if (post != null)
            {
                // saved information was passed, update fields
                this.selectedPost[0]= post;
                imageListView.ItemsSource = selectedPost;
            }
        }

        public void imageList_ItemTapped(object sender, EventArgs e)
        {
            //user has tapped an item in the image list 
            Post selection = (Post) imageListView.SelectedItem;

            //redirect to input page for user to edit the data
            App.Current.MainPage = new Input(selection);
        }

        public void OnNewButtonClicked(object sender, EventArgs args)
        {
            //redirect to input page for user to enter data
            App.Current.MainPage = new Input();
        }

        public void IfSaveFileExists()
        {
            //check if saved file exists and hide load button
            if (File.Exists(saveFile))
            {
                deleteButton.IsVisible = true;
                loadValues();

                //data has been loaded, force refresh the image list
                imageListView.ItemsSource = null;
                imageListView.ItemsSource = selectedPost;
            }
            else
            {
                //no saved data, hide delete button
                deleteButton.IsVisible = false;

            }
        }

        public void loadValues() {
            //load values from the previously saved file
            try
            {
                StreamReader reader = new StreamReader(saveFile);

                String line;

                //parse the saved data line by line and update GUI
                //also update that data variable that holds the information
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith("title="))
                    {
                        savedTitle = line.Split('=')[1].Trim();
                        selectedPost[0].title = savedTitle;
                    }
                    else if (line.StartsWith("comments="))
                    {
                        savedComments = line.Split('=')[1].Trim();
                        selectedPost[0].comments = savedComments;
                    }
                    else if (line.StartsWith("photo="))
                    {
                        savedPhoto = line.Split('=')[1].Trim();
                        selectedPost[0].photo = savedPhoto;
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

        public void OnDeleteButtonClicked(object sender, EventArgs args)
        {
            //clear the imagelist
            selectedPost[0] = null;

            //delete savedFile
            if (File.Exists(saveFile))
            {
                File.Delete(saveFile);
                //hide Delete All button
                IfSaveFileExists();
            }
        }

    }
}
