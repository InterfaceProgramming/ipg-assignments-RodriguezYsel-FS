using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Insta_Photos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Input : ContentPage
    {

        //declare variables and initialize as necessary
        String saveFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "info.txt"); 

        //this will store info for the current post
        public ObservableCollection<Post> selectedPost = new ObservableCollection<Post>(){
            new Post()
            {title = "", comments = "", photo = ""}
        };

        //this will store data to be used by the picker
        public ObservableCollection<Post> PostsContainer = new ObservableCollection<Post>()
        {
            new Post()
            {title = "Forest", comments = "A silent, mysterious forest", photo = "forest.jpg"},
            new Post()
            {title = "Bird", comments = "Not sure what species, but pretty!", photo = "bird.jpg"},
            new Post()
            {title = "Flowers", comments = "A field of carnations", photo = "flowers.jpg"},
            new Post()
            {title = "Beach", comments = "Rocks, sand, water", photo = "beach.jpg"},
            new Post()
            {title = "City", comments = "Concrete jungle, huh", photo = "city.jpg"}

        };

        //constructor
        public Input()
        {
            InitializeComponent();

            //bind the data to be used by picker
            picker.ItemsSource = PostsContainer;
        }

        //another constructor, this is used when there's existing information
        public Input(Post post = null)
        {
            InitializeComponent();

            if (post != null)
            {
                // saved information was passed, update fields
                this.selectedPost[0] = post;
                imageListView.ItemsSource = selectedPost;
                title.Text = selectedPost[0].title;
                comments.Text = selectedPost[0].comments;
            }
        }

        public void OnSaveButtonClicked(object sender, EventArgs args)
        {
            // user has clicked save button
            // save data to file

            //this will keep track of form state 
            bool dirty = false;

            String titleValue = "";
            String commentsValue = "";
            String photoValue = "";


            //perform data validation and get user input

            //get title
            try
            {
                if (title.Text.Length == 0)
                {
                    DisplayAlert("Validation Failed", "Enter a post title", "OK");
                }
                else
                {
                    titleValue = title.Text;
                }
            }
            catch (Exception e)
            {
                DisplayAlert("Validation Failed", "Enter a post title", "OK");
                dirty = true;
            }

            //get comments
            if (dirty == false)
            {
                try
                {
                    if (comments.Text.Length == 0)
                    {
                        DisplayAlert("Validation Failed", "Enter comments", "OK");
                    }
                    else
                    {
                        commentsValue = comments.Text;
                    }
                }
                catch (Exception e)
                {
                    DisplayAlert("Validation Failed", "Enter comments", "OK");
                    dirty = true;
                }
            }


            //get photo
            if (dirty == false)
            {
                try
                {
                    if (selectedPost[0].photo.Length == 0)
                    {
                        DisplayAlert("Validation Failed", "Select a category", "OK");
                    }
                    else
                    {
                        photoValue = selectedPost[0].photo;
                    }
                }
                catch (Exception e)
                {
                    DisplayAlert("Validation Failed", "Select a category", "OK");
                    dirty = true;
                }
            }


            if (dirty == false)
            {
                //save to file
                Post post = new Post() { title = titleValue, comments = commentsValue, photo = photoValue }; 
                save(post);

                //send saved data to MainPage
                App.Current.MainPage = new MainPage(post);
            }
        }

        public void save(Post post)
        {
            try
            {
                //save the data to a file
                var saveFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "info.txt");
                StreamWriter writer = new StreamWriter(saveFile);

                writer.WriteLine("title=" + post.title);
                writer.WriteLine("comments=" + post.comments);
                writer.WriteLine("photo=" + post.photo);
                writer.Close();
            }
            catch (Exception e)
            {
                //an error occured during saving, do nothing
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
                //redirect to Main Page
                App.Current.MainPage = new MainPage();
            }
        }

        public void picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            //user has selected an item from picker
            String selection = picker.Items[picker.SelectedIndex];
            selectedPost[0] = PostsContainer.Single(i => i.title == selection);

            //bind the image list to show the selected data
            imageListView.ItemsSource = selectedPost;

            //update input fields to contain pre-filled data
            title.Text = selectedPost[0].title;
            comments.Text = selectedPost[0].comments;

        }



        public void title_TextChanged(object sender, TextChangedEventArgs e)
        {
            //update the data list for imagelist as user types so they see live changes
            selectedPost[0].title = title.Text;
            //force refresh of the imagelist 
            imageListView.ItemsSource = null;
            imageListView.ItemsSource = selectedPost;
        }

        public void comments_TextChanged(object sender, TextChangedEventArgs e)
        {
            //update the data list for imagelist as user types so they see live changes
            selectedPost[0].comments = comments.Text;
            //force refresh of the imagelist
            imageListView.ItemsSource = null;
            imageListView.ItemsSource = selectedPost;
        }


    }
}