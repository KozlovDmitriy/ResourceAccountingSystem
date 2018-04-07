using System;
using System.Collections.Generic;

namespace ResourceAccountingSystem.Model
{
    public partial class Street
    {
        public Street()
        {
            House = new HashSet<House>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<House> House { get; set; }
    }
}
