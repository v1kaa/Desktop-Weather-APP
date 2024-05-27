using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Security.RightsManagement;
using System.Windows.Media.Animation;
using System.Net;
using Newtonsoft.Json;

namespace weather
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        string key = "41464aa5246387013b738c9dc010df85";
        private string defaultText = "city";
        public MainWindow()
        {
            InitializeComponent();
            SetDateAndTime();
            
            input_textbox.Text= defaultText;
        }

        private void SetDateAndTime()
        {
            DateTime currentDate = DateTime.Now;
            string formattedDate = currentDate.ToString("dddd dd MMMM", CultureInfo.InvariantCulture);
            date_label.Content = formattedDate;
            DateTime currentTime = DateTime.Now;
            string formattedTime = currentTime.ToString("HH:mm");
            time_label.Content = formattedTime;
        }

        void getWeather(string city)
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={key}";
                    var json = client.DownloadString(url);
                    WeatherInfo.root Info = JsonConvert.DeserializeObject<WeatherInfo.root>(json);
                
                //using bitmap for displaying pictures
                string localImagePath = "photo\\" + Info.weather[0].icon + "_t.png";
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(localImagePath, UriKind.Relative); // Use UriKind.Relative
                bitmap.EndInit();
                image.Source = bitmap;

                wind_speed_label.Content = "wind speed: "+Info.wind.speed;
                pressure_label.Content = "pressure: " + Info.main.pressure;
                humidity_label.Content="humidity: " + Info.main.humidity;
                main_label.Content = "main: "+Info.weather[0].main;
                description_label.Content = Info.weather[0].description;
                sunrise_label.Content = "Sunrise at: " + DateTimeOffset.FromUnixTimeSeconds(Info.sys.sunrise).LocalDateTime.ToString("HH:mm");
                sunset_label.Content="Sunset at: " + DateTimeOffset.FromUnixTimeSeconds(Info.sys.sunset).LocalDateTime.ToString("HH:mm");
                double temperatureInKelvin = Info.main.temp;
                double temperatureInCelsius = temperatureInKelvin - 273.15;

                temperature_label.Content = "Temperature: " + temperatureInCelsius.ToString("0.##") + "°C";

                }
                catch (Exception ex) { MessageBox.Show(ex.Message + "or You write uncorrect city, Try again"); }

            }


        }
        class WeatherInfo
        {
            public class weather
            {
                public string main { get; set; }
                public string description { get; set; }
                public string icon { get; set; }
            }
            public class main
            {
                public double temp { get; set; }
                public double pressure { get; set; }
                public double humidity { get; set; }
            }
            public class wind
            {
                public double speed { get; set; }
            }
            public class sys
            {
                public long sunrise { get; set; }
                public long sunset { get; set; }
            }
            public class root
            {
                public List<weather> weather { get; set; }
                public main main { get; set; }
                public wind wind { get; set; }
                public sys sys { get; set; }

            }
        }



        private void search_Click(object sender, EventArgs e)
        {
            getWeather(input_textbox.Text);
            city_label.Content=input_textbox.Text;
            input_textbox.Text = defaultText;
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (input_textbox.Text == defaultText)
            {
                input_textbox.Text = string.Empty;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(input_textbox.Text))
            {
                input_textbox.Text = defaultText;
            }

        }
    }
}