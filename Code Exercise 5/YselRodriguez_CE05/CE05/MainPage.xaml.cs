using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CE05
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<Contact> ContactsList = new ObservableCollection<Contact>();

        InputPage inputPage;
        public MainPage()
        {
            InitializeComponent();

            inputPage = new InputPage();

            //hide delete image if there's no data
            if (ContactsList.Count < 1)
            {
                deleteImage.IsVisible = false;
            }

            collectionView.ItemsSource = ContactsList;

            //send a heartbeat to Input Page so it knows what object to later redirect to, for data persistence between navigations
            Xamarin.Forms.MessagingCenter.Send<MainPage>(this, "ping");

            //listen for message on save event from Input Page
            MessagingCenter.Subscribe<InputPage, Contact>(this, "save", (sender, data) =>
            {
                // received message from Input Page
                // new object has been created
                //save data
                
                ContactsList.Add(data);

                //hide delete image if there's no data
                if (ContactsList.Count < 1)
                {
                    deleteImage.IsVisible = false;
                }
                else
                {
                    deleteImage.IsVisible = true;
                }

            });
        }

        public void OnAddImageTapped(object sender, EventArgs args)
        {
            //redirect to input page for user to enter data
            App.Current.MainPage = inputPage;
        }

        public async void OnDeleteImageTapped(object sender, EventArgs args)
        {
            bool answer = await DisplayAlert("Delete", "Are you sure you want to delete selected contacts?", "YES", "NO");

            if (answer == true)
            {
                foreach (Contact contact in collectionView.SelectedItems)
                {
                    ContactsList.Remove(contact);
                }
            }
        }
    }
}
