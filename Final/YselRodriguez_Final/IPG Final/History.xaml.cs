using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace IPG_Final
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class History : ContentPage
    {

        public ObservableCollection<Stock> HistoricalData = new ObservableCollection<Stock>();
        public History()
        {
            InitializeComponent();

            //set the carouselview to use Observable Collection for data
            carouselView.ItemsSource = HistoricalData;
        }

        public async void Get_Historical_Button_Clicked(object sender, EventArgs e)
        {
            //validate values
            bool dirty = false;

            String symbolValue = "";
            String dataType = "";
            String dateFrom = "";
            String dateTo = "";

            
            if (StockInput.Text.Length == 0)
            {
                dirty = true;
                await DisplayAlert("Validation Failed", "Type a stock symbol", "OK");
            }
            else {
                int selectedIndex = picker.SelectedIndex;

                if (selectedIndex != -1)
                {
                    //user has selected a data type
                    dirty = false;

                    symbolValue = StockInput.Text.ToLower();

                    dataType = (string)picker.ItemsSource[selectedIndex];

                    if (String.Equals(dataType, "Single"))
                    {
                        //user has selected single day type
                        dateFrom = datePicker.Date.ToString("yyyy-MM-dd");
                        dateTo = datePicker.Date.ToString("yyyy-MM-dd");
                    }
                    else {
                        //user has selected ten day range
                        dateFrom = datePicker.Date.ToString("yyyy-MM-dd");

                        //add ten days to datefrom
                        dateTo = datePicker.Date.AddDays(10).ToString("yyyy-MM-dd");
                    }

                    //fetch the data
                    FetchData(symbolValue, dateFrom, dateTo);

                }
                else
                {
                    //user has not selected a data type
                    dirty = true;
                    await DisplayAlert("Validation Failed", "Select a data type", "OK");
                }
            }
        }

        public async void FetchData(String symbolValue, String dateFrom, String dateTo)
        {
            //URL parameters
            var parameters = $"?symbols={symbolValue}&access_key={Constants.API_KEY}&date_from={dateFrom}&date_to={dateTo}";

            try
            {
                //instantiate api client
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Constants.API_URL);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //make request asyncroniously
                HttpResponseMessage response = await client.GetAsync(parameters).ConfigureAwait(false);

                JObject json = JObject.Parse(response.Content.ReadAsStringAsync().Result);

                //dynamic dynJson = JsonConvert.DeserializeObject(json["Data"]);

                if (response.IsSuccessStatusCode)
                {
                    //reset the data collection
                    HistoricalData.Clear();

                    //parse the data into a dictionary
                    var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(json.ToString());
                    Console.WriteLine(json.ToString());

                    if (dictionary.ContainsKey("error"))
                    {
                        //there was an API error
                        JObject error_object = JObject.Parse(dictionary["error"].ToString());
                        String error_value = error_object.GetValue("code").ToString();

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            DisplayAlert("API Failed", error_value, "OK");
                        });
                    }
                    else
                    {
                        //successfully fetched API result for symbol
                        //parse the useful data into a json array
                        JArray jsonArray = JArray.Parse(dictionary["data"].ToString());

                        if (jsonArray.Count > 0)
                        {
                            foreach (JObject element in jsonArray)
                            {
                                //set the data values

                                Stock stock = new Stock();
                                stock.Symbol = element.GetValue("symbol").ToString();

                                stock.Open = element.GetValue("open").ToString();
                                stock.Close = element.GetValue("close").ToString();
                                stock.Close = element.GetValue("close").ToString();
                                stock.High = element.GetValue("high").ToString();
                                stock.Low = element.GetValue("low").ToString();
                                stock.Volume = element.GetValue("volume").ToString();
                                stock.Exchange = element.GetValue("exchange").ToString();
                                stock.Image = Constants.EOD_API_URL + stock.Symbol.ToLower() + ".png" + "?api_token=" + Constants.EOD_API_KEY;
                                stock.Date = DateTime.Parse(element.GetValue("date").ToString()).ToLongDateString();

                                //add to data list
                                HistoricalData.Add(stock);

                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    //display the data in a new page
                                    Navigation.PushAsync(new HistoricalView(HistoricalData), true);
                                });
                            }
                        }
                        else
                        {
                            //empty result set, maybe weekend?
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                DisplayAlert("No Data", "The search returned no data. Please check your" +
                                    " search parameters (There is no EOD data on weekends) and try again", "OK");
                            });

                            }
                    }
                }
                else
                {
                    //API error
                    //parse the data into a dictionary
                    var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(json.ToString());
                    Console.WriteLine(json.ToString());

                    if (dictionary.ContainsKey("error"))
                    {
                        //there was an API error
                        JObject error_object = JObject.Parse(dictionary["error"].ToString());
                        String error_value = error_object.GetValue("code").ToString();

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            DisplayAlert("API Failed", error_value, "OK");
                        });
                    }
                }
            }
            catch (WebException ex)
            {
                await DisplayAlert("API Failed", ex.Message, "OK");
            }
        }
  
    }
}