using PlantWing.App.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PlantWing.App.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public static int HIGH = 1;
        public static int MEDIUM = 2;
        public static int LOW = 3;

        public ObservableCollection<SoilMoisture> SoilMoistureList { get; set; }

        string ipAddress;
        public string IpAddress
        {
            get => ipAddress;
            set { ipAddress = value; OnPropertyChanged(nameof(IpAddress)); }
        }

        bool isRefreshing;
        public bool IsRefreshing
        {
            get => isRefreshing;
            set { isRefreshing = value; OnPropertyChanged(nameof(IsRefreshing)); }
        }

        public Command GetSoilMoisturesCommand { private set; get; }

        public MainViewModel()
        {
            SoilMoistureList = new ObservableCollection<SoilMoisture>();

            IpAddress = "192.168.1.74";

            GetSoilMoisturesCommand = new Command(async (s) => await GetSoilMoistureReadings());

            GetSoilMoistureReadings();
        }

        async Task GetSoilMoistureReadings()
        {
            SoilMoistureList.Clear();

            //var response = await NetworkManager.GetAsync(IpAddress);

            //if (response != null)
            //{
            //    string json = await response.Content.ReadAsStringAsync();
            //    var values = (List<ClimateReading>)System.Text.Json.JsonSerializer.Deserialize(json, typeof(List<ClimateReading>));

            //    foreach (ClimateReading value in values)
            //    {
            //        ClimateList.Add(new SoilMoistureModel() { Id = value.ID, Moisture = value.TempC });
            //    }
            //}

            SoilMoistureList.Add(new SoilMoisture() { Id = 0, Moisture = 100, Level = HIGH, Date = DateTime.Now.ToString("g") });
            SoilMoistureList.Add(new SoilMoisture() { Id = 1, Moisture = 98,  Level = HIGH, Date = DateTime.Now.ToString("g") });
            SoilMoistureList.Add(new SoilMoisture() { Id = 2, Moisture = 77,  Level = MEDIUM, Date = DateTime.Now.ToString("g") });
            SoilMoistureList.Add(new SoilMoisture() { Id = 3, Moisture = 56,  Level = MEDIUM, Date = DateTime.Now.ToString("g") });
            SoilMoistureList.Add(new SoilMoisture() { Id = 4, Moisture = 45,  Level = LOW, Date = DateTime.Now.ToString("g") });
            SoilMoistureList.Add(new SoilMoisture() { Id = 5, Moisture = 26,  Level = LOW, Date = DateTime.Now.ToString("g") });

            IsRefreshing = false;
        }

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion
    }
}
