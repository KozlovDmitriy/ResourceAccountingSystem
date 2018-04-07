using System;
using System.Collections.Generic;

namespace ResourceAccountingSystem.Model
{
    public partial class MeterReading
    {
        public int Id { get; set; }
        public string MeterSerialNumber { get; set; }
        public int Value { get; set; }
        public DateTime ReadingDateTime { get; set; }

        public Meter MeterSerialNumberNavigation { get; set; }
    }
}
