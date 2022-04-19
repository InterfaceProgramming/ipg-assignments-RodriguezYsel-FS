using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CE05
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InputPage : ContentPage
    {
        MainPage mainPage;

        public InputPage()
        {
            InitializeComponent();

            //listen for ping message from Main Page
            MessagingCenter.Subscribe<MainPage>(this, "ping", sender =>
            {
                mainPage = sender;
            }
            );
        }

        public void OnSaveButtonClicked(object sender, EventArgs args)
        {
            //create a new contact instance
            Contact contact = new Contact();

            // set data
            contact.name = name.Text;
            contact.phone = phone.Text; 
            contact.email = email.Text;

            //send the data to Main Page
            Xamarin.Forms.MessagingCenter.Send<InputPage, Contact>(this, "save", contact);

            //reset fields
            name.Text = "";
            phone.Text = "";
            email.Text = "";
            
            //redirect to Main Page
            App.Current.MainPage = mainPage;

        }
    }
}