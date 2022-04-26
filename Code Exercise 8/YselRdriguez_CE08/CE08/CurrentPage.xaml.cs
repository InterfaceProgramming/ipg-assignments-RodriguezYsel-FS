using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
//Ysel Rodriguez
//DEV2500
//TermC202204
//CE08: TabbedPage & More API
namespace CE08
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CurrentPage : ContentPage
    {
        //declare variables
        String API_KEY = "4d1fda74527f1ae3eae4069100dc058d";
        String API_URL = "http://api.marketstack.com/v1/eod"; 
        List<Data> dataList = new List<Data>();
        public CurrentPage()
        {
            InitializeComponent();


            //if data list is zero do not display reset page
            if (dataList.Count == 0) { 
                reset.IsVisible = false;
            }

            MessagingCenter.Subscribe<Settings, String>(this, "get", (sender, data) =>
            {
                // received message from Settings Page
                
                //URL parameters
                var parameters = $"?symbols={data}&access_key={API_KEY}&limit=1";

                //fetch data
                FetchData(parameters);

            });
        }

        public async void FetchData(String parameters)
        {
            //instantiate api client
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(API_URL);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //make request asyncroniously
            HttpResponseMessage response = await client.GetAsync(parameters).ConfigureAwait(false);

            JObject json = JObject.Parse(response.Content.ReadAsStringAsync().Result);

            //dynamic dynJson = JsonConvert.DeserializeObject(json["Data"]);

            if (response.IsSuccessStatusCode)
            {
                //parse the data into a dictionary
                var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(json.ToString());

                //parse the useful data into a json array
                JArray jsonArray = JArray.Parse(dictionary["data"].ToString());

                foreach (JObject element in jsonArray)
                {
                    //create new data object
                    Data data = new Data();

                    //set the data values
                    data.Symbol = element.GetValue("symbol").ToString();
                    data.Open = element.GetValue("open").ToString();
                    data.Close = element.GetValue("close").ToString();
                    data.High = element.GetValue("high").ToString();
                    data.Low = element.GetValue("low").ToString();
                    data.Volume = element.GetValue("volume").ToString();
                    data.Exchange = element.GetValue("exchange").ToString();
                    data.Date = DateTime.Parse(element.GetValue("date").ToString()).ToLongDateString();

                    //add to data list
                    dataList.Clear();
                    dataList.Add(data);
                }

                //display the first item, and enable next button
                DisplayItem(0);
            }
        }

        public void DisplayItem(int index)
        {

            Device.BeginInvokeOnMainThread(() =>
            {
                //display the symbol image
                image.Source = ImageSource.FromFile(dataList[index].Symbol.ToLower() + ".png");

                //display the symbol in uppercase
                symbol.Text = dataList[index].Symbol.ToUpper();

                //display other data values
                open.Text = "Open: $" + dataList[index].Open;
                close.Text = "Close: $" + dataList[index].Close;
                high.Text = "High: $" + dataList[index].High;
                low.Text = "Low: $" + dataList[index].Low;
                volume.Text = "Volume: " + dataList[index].Volume;
                exchange.Text = "Exchange: " + dataList[index].Exchange;
                date.Text = "Date: " + dataList[index].Date;

            });
        }

        public void Reset_Button_Clicked(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                //clear displayed data
                image.Source = "";
                symbol.Text = "";
                open.Text = "";
                close.Text = "";
                high.Text = "";
                low.Text = "";
                volume.Text = "";
                exchange.Text = "";
                date.Text = "";

                //remove reset page button
                reset.IsVisible = false;

            });
        }
    }
}