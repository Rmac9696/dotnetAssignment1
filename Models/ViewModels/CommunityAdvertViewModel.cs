﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNET_lab4.Models.ViewModels
{
    public class CommunityAdvertViewModel
    {
        public Community Community { get; set; }

        public IEnumerable<Advertisement> Advertisements { get; set; }

    }
}
