using System;

namespace PlantWing.App.Models
{
    public class SoilMoisture
    {
        public long Id { get; set; }
        public int Level { get; set; }
        public int Moisture { get; set; }
        public string Date { get; set; }
    }
}
