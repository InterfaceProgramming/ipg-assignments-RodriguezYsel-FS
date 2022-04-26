using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CE08
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Settings : ContentPage
    {
        public Settings()
        {
            InitializeComponent();

            //attach events to buttons
            if (apple != null)
            {
                apple.Clicked += Apple_Button_Clicked;

            }
            if (facebook != null)
            {
                facebook.Clicked += Facebook_Button_Clicked;

            }
            if (amazon != null)
            {
                amazon.Clicked += Amazon_Button_Clicked;

            }
            if (google != null)
            {
                google.Clicked += Alphabet_Button_Clicked;

            }
            if (netflix != null)
            {
                netflix.Clicked += Netflix_Button_Clicked;

            }
            if (att != null)
            {
                att.Clicked += Att_Button_Clicked;

            }
        }

        public void Apple_Button_Clicked(object sender, EventArgs e)
        {
            //send selection to Current Page
            MessagingCenter.Send<Settings, String>(this, "get", "AAPL");

            //save historical data
            HistoryData historyData = new HistoryData();
            historyData.Symbol = "AAPL";
            historyData.Image = "aapl.png";
            historyData.Date = DateTime.Now.ToString();

            //send history to History page
            MessagingCenter.Send<Settings, HistoryData>(this, "history", historyData);
        }

        public void Amazon_Button_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send<Settings, String>(this, "get", "AMZN");

            //save historical data
            HistoryData historyData = new HistoryData();
            historyData.Symbol = "AMZN";
            historyData.Image = "amzn.png";
            historyData.Date = DateTime.Now.ToString();

            //send history to History page
            MessagingCenter.Send<Settings, HistoryData>(this, "history", historyData);
        }

        public void Facebook_Button_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send<Settings, String>(this, "get", "FB");

            //save historical data
            HistoryData historyData = new HistoryData();
            historyData.Symbol = "FB";
            historyData.Image = "fb.png";
            historyData.Date = DateTime.Now.ToString();

            //send history to History page
            MessagingCenter.Send<Settings, HistoryData>(this, "history", historyData);
        }

        public void Alphabet_Button_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send<Settings, String>(this, "get", "GOOGL");

            //save historical data
            HistoryData historyData = new HistoryData();
            historyData.Symbol = "GOOGL";
            historyData.Image = "googl.png";
            historyData.Date = DateTime.Now.ToString();

            //send history to History page
            MessagingCenter.Send<Settings, HistoryData>(this, "history", historyData);
        }

        public void Netflix_Button_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send<Settings, String>(this, "get", "NFLX");

            //save historical data
            HistoryData historyData = new HistoryData();
            historyData.Symbol = "NFLX";
            historyData.Image = "nflx.png";
            historyData.Date = DateTime.Now.ToString();

            //send history to History page
            MessagingCenter.Send<Settings, HistoryData>(this, "history", historyData);
        }

        public void Att_Button_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send<Settings, String>(this, "get", "T");

            //save historical data
            HistoryData historyData = new HistoryData();
            historyData.Symbol = "T";
            historyData.Image = "t.png";
            historyData.Date = DateTime.Now.ToString();

            //send history to History page
            MessagingCenter.Send<Settings, HistoryData>(this, "history", historyData);
        }


    }
}