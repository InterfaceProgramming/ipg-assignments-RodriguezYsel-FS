using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IPG_Final
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Details : ContentPage
    {
        public Details()
        {
            InitializeComponent();
        }

        public Details(Stock stock)
        {
            InitializeComponent();

            image.Source = stock.Image;
            symbol.Text = stock.Symbol;
            open.Text = "Open: $" + stock.Open;
            close.Text = "Close: $" + stock.Close;
            high.Text = "High: $" + stock.High;
            low.Text = "Low: $" + stock.Low;
            volume.Text = "Volume: " + stock.Volume;
            exchange.Text = "Exchange: " + stock.Exchange;
            date.Text = "Date: " + stock.Date;
        }

        public async void Close_Button_Clicked(object sender, EventArgs e)
        {
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}