using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
//Ysel Rodriguez
//DEV2500
//TermC202204
//4.2 Final Project: Stock App Project
namespace IPG_Final
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
