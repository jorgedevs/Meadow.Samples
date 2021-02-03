using System;

namespace PlantWing.App.Models
{
    public class SoilMoisture
    {
        public long Id { get; set; }
        public int Level { get; set; }
        public decimal Moisture { get; set; }
        public string Date { get; set; }
    }
}
