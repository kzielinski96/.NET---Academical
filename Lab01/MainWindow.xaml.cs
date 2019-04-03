using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Lab01
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // private fields necessary in getting data from the internet 
        private static WebClient client = new WebClient();
        private static Uri remoteUri1 = new Uri("https://raw.githubusercontent.com/dwyl/english-words/master/words.txt");
        private static String htmlText = client.DownloadString(remoteUri1);
        private static String[] words = htmlText.Split('\n');
        private static Random random = new Random();


        ObservableCollection<Person> people = new ObservableCollection<Person>
        {
            new Person {Name = "P1", Age = 1},
            new Person {Name = "P2", Age = 2}
        };

        public ObservableCollection<Person> Items
        {
            get => people;
        }

        public MainWindow()
        {
            InitializeComponent();
//            GetDataTask();
            GetWeatherData();
            DataContext = this;
        }


        private void AddImage_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog op = new Microsoft.Win32.OpenFileDialog();

            op.DefaultExt = ".png";
            op.Filter =
                "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";

            if (op.ShowDialog() == true)
            {
                image.Source = new BitmapImage(new Uri(op.FileName));
            }
        }

        private void AgeTextBox_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(ageTextBox.Text, "[^0-9]+[^.[0-9]+]?"))
            {
                MessageBox.Show("Please enter digits only.");
                ageTextBox.Text = ageTextBox.Text.Remove(ageTextBox.Text.Length - 1);
            }
        }


        private void AddNewPersonButton_Click(object sender, RoutedEventArgs e)
        {
            people.Add(new Person
                {Age = double.Parse(ageTextBox.Text), Name = nameTextBox.Text, PersonImage = image.Source as BitmapImage});
        }


        private void ListBox_MouseDoubleClick(object sender, EventArgs e)
        {
            Person person = ListBox.SelectedItem as Person;
            nameTextBox.Text = person.Name;
            ageTextBox.Text = person.Age.ToString();
            image.Source = person.PersonImage;
        }

        private async void GetDataTask()
        {
            int i = 10;

            await Task.Run(() =>
            {
                while (true)
                {
                    Uri remoteUri = new Uri("https://source.unsplash.com/random/400x3" + i.ToString());
                    client.DownloadFile(remoteUri, "C:/Users/Korni/Desktop/platformy/.NET---Academical/img/" + i.ToString() + ".jpg");
                    String name = words[random.Next(0, words.Length)];
                    int age = random.Next(0, 50);

                    Dispatcher.Invoke(() => { 
                        people.Add(new Person{ Age = age, Name = name, PersonImage = new BitmapImage(new Uri("C:/Users/Korni/Desktop/platformy/.NET---Academical/img/" + i.ToString() + ".jpg"))});
                    });

                    i++;
                    Thread.Sleep(5000);

                }
            });
        }

        private async void GetWeatherData()
        {
            string apiKey = "747fe0e760b5276cc648effd30d3a2a8";
            string apiBaseUrl = "https://api.openweathermap.org/data/2.5/weather";



            await Task.Run(() =>
            {
                using (var client = new WebClient())
                {
                    while (true)
                    {
                        double lat = random.Next(35, 71);
                        double lon = random.Next(-9, 68);
                        string apiCall = apiBaseUrl + "?lat=" + lat + "&lon=" + lon + "&apikey=" + apiKey +
                                         "&mode=json&units=metric";
//                        String apiCall =
//                            "https://api.openweathermap.org/data/2.5/weather?lat=51&lon=17&apikey=747fe0e760b5276cc648effd30d3a2a8&mode=json&units=metric";

                        String jsonString = client.DownloadString(apiCall);
                        var jsonObject = JsonConvert.DeserializeObject<JObject>(jsonString);
                        String name = jsonObject["name"].ToString();
                        String temp = jsonObject["main"]["temp"].ToString();

                        if (name == "")
                        {
                            name = "Ocean or Sea";
                        }

                            Dispatcher.Invoke(() =>
                            {
                                people.Add(new Person {Age = double.Parse(temp), Name = name});
                            });

                        Thread.Sleep(5000);
                    }
                }
            });


        }
    }
}