using MobilePlantSample.Models;
using PlantWing.Shared.Network;
using PlantWing.Shared.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MobilePlantSample.ViewModels
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

            var response = await NetworkManager.GetAsync(IpAddress);

            if (response != null)
            {
                string json = await response.Content.ReadAsStringAsync();
                var values = (List<SoilMoistureEntity>)System.Text.Json.JsonSerializer.Deserialize(json, typeof(List<SoilMoistureEntity>));

                foreach (var value in values)
                {
                    var model = new SoilMoisture();
                    model.Date = value.date;
                    model.Id = value.id;
                    model.Moisture = value.value * 100;
                    model.Level = model.Moisture > 75 ? HIGH : model.Moisture > 25 ? MEDIUM : LOW;

                    SoilMoistureList.Add(model);
                }
            }

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
