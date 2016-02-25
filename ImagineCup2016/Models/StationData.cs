using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImagineCup2016.Models
{
    public class StationData
    {
        public List<Programme> ProgramList { get; set; }
        public List<UserProfile> AnnouncerList { get; set; }
        public List<UserProfile> ProducerList { get; set; }
        //Time table
        public station Station { get; set; }
    }
}