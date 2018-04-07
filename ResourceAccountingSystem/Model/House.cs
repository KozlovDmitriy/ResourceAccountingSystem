using System;
using System.Collections.Generic;

namespace ResourceAccountingSystem.Model
{
    public partial class House
    {
        public House()
        {
            Meter = new HashSet<Meter>();
        }

        public int Id { get; set; }
        public string Zip { get; set; }
        public int HouseNumber { get; set; }
        public int StreetId { get; set; }

        public Street Street { get; set; }
        public ICollection<Meter> Meter { get; set; }
    }
}
