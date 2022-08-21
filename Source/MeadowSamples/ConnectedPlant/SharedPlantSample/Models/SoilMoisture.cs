using System;
using System.Collections.Generic;
using System.Text;

namespace PlantWing.Shared.Models
{
    public class SoilMoisture
    {
        public long Id { get; set; }
        public int Level { get; set; }
        public int Moisture { get; set; }
        public string Date { get; set; }
    }
}
