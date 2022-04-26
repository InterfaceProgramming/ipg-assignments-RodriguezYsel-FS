using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
//Ysel Rodriguez
//DEV2500
//TermC202204
//CE07:JSON Data & APIs
namespace CE07
{
    public partial class MainPage : ContentPage
    {
        //declare variables
        String API_KEY = "4d1fda74527f1ae3eae4069100dc058d";
        String API_URL = "http://api.marketstack.com/v1/eod";
        List<String> symbolsList = new List<String>() { "AAPL", "AMZN", "FB", "GOOGL", "NFLX" };
        List<Data> dataList = new List<Data>();
        int index = 0;

        public MainPage()
        {
            InitializeComponent();

            //at this point, there is no data, disable next and previous buttons
            next.IsEnabled = false;
            previous.IsEnabled = false;
        }

        public async void FetchData(String parameters) {
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
                    dataList.Add(data);
                }

                    //display the first item, and enable next button
                    DisplayItem(0);

                Device.BeginInvokeOnMainThread(async () =>
                {
                    //enable next button since we have data
                    next.IsEnabled = true;
                });
            }

            else
            {
                Console.WriteLine(response.StatusCode.ToString() + " - " + (string)(json.GetValue("error")["code"]));

                Device.BeginInvokeOnMainThread(async () =>
                {
                    //API error
                    await DisplayAlert("API Result", (response.StatusCode.ToString() + " - " + (string)(json.GetValue("error")["code"])), "OK");
                });
            }
        }

        public void DisplayItem(int index) {
            
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
        public void Get_Button_Clicked(object sender, EventArgs e)
        {
            //URL parameters
            var parameters = $"?symbols=AAPL,AMZN,FB,GOOGL,NFLX &access_key={API_KEY}&limit=5";

            //fetch data
            FetchData(parameters);

        }

        public void Next_Button_Clicked(object sender, EventArgs e)
        {
            if (dataList.Count > index + 2)
            {
                //display next item
                DisplayItem(index + 1);

                //since we have moved to next item, enable previous button
                previous.IsEnabled = true;
                index++;

            }
            else
            {
                //this is the last item
                DisplayItem(index + 1);

                //since we have moved to next item, enable previous button
                previous.IsEnabled = true;
                index = index + 1;

                //disable next button since this is the last item
                next.IsEnabled = false;
            }
        }
        
        public void Previous_Button_Clicked(object sender, EventArgs e)
        {
            if (index == 1)
            {
                //we are about to show the first item, disable previous button
                DisplayItem(0);
                previous.IsEnabled = false;
            }
            else { 
                //display previous item
                DisplayItem(index - 1);
                index--;
            }
        }      
    }
}
