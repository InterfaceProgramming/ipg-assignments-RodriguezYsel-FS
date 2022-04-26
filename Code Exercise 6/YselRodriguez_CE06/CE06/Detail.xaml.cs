using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CE06
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Detail : ContentPage
    {
        public Detail()
        {
            InitializeComponent();

            //listen for message from Main Page
            MessagingCenter.Subscribe<MainPage, Animal>(this, "display", (sender, data) =>
            {
                //display the name of the animal and their description
                Console.WriteLine("yes");
                nameLabel.Text = data.Name;
                descriptionLabel.Text = data.Description;
            });
        }

    }
}