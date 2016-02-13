using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ImagineCup2016.Models
{
    public class Station
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Frequency { get; set; }
        public HttpPostedFileBase Logo { get; set; }
        public bool Approved { get; set; }

    }
}
