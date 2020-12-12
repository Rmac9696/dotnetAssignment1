using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNET_lab4.Models.ViewModels
{
    public class StudentMembershipsViewModel
    {

        public Student student { get; set; }

        public IEnumerable<Community> communities { get; set; }

    }
}
