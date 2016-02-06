using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagineCup2016.Models
{
    public class Station
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Frequency { get; set; }
        public byte[] Logo { get; set; }
        public bool Approved { get; set; }

    }
}
