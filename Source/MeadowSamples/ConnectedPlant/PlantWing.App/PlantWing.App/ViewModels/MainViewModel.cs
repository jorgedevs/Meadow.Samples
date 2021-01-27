using PlantWing.App.Models;
using PlantWing.App.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PlantWing.App.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<SoilMoistureModel> ClimateList { get; set; }

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

        public Command GetHumidityCommand { private set; get; }

        public MainViewModel()
        {
            ClimateList = new ObservableCollection<SoilMoistureModel>();

            IpAddress = "192.168.1.74";

            GetHumidityCommand = new Command(async (s) => await GetReadingsAsync());

            GetReadingsAsync();
        }

        async Task GetReadingsAsync()
        {
            ClimateList.Clear();

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

            ClimateList.Add(new SoilMoistureModel() { Id = 0, Moisture = 100, Date = DateTime.Now.ToString("g") });
            ClimateList.Add(new SoilMoistureModel() { Id = 1, Moisture = 98,  Date = DateTime.Now.ToString("g") });
            ClimateList.Add(new SoilMoistureModel() { Id = 2, Moisture = 77,  Date = DateTime.Now.ToString("g") });
            ClimateList.Add(new SoilMoistureModel() { Id = 3, Moisture = 56,  Date = DateTime.Now.ToString("g") });
            ClimateList.Add(new SoilMoistureModel() { Id = 4, Moisture = 45,  Date = DateTime.Now.ToString("g") });
            ClimateList.Add(new SoilMoistureModel() { Id = 5, Moisture = 26,  Date = DateTime.Now.ToString("g") });

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
