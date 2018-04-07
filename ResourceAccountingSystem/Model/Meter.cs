using System;
using System.Collections.Generic;

namespace ResourceAccountingSystem.Model
{
    public partial class Meter
    {
        public Meter()
        {
            MeterReading = new HashSet<MeterReading>();
        }

        public string SerialNumber { get; set; }
        public int HouseId { get; set; }

        public House House { get; set; }
        public ICollection<MeterReading> MeterReading { get; set; }
    }
}
