using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNET_lab4.Models
{
    public class CommunityAdvert
    {
        public int AdvertisementID{ get; set; }

        public string CommunityID { get; set; }

        public Community Community { get; set; }

        public Advertisement Advertisement {get; set;}
    }
}
