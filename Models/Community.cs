using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNET_lab4.Models
{
    public class Community
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [DisplayName("Registration Number")]
        [Required]
        public string ID { get; set; }
        [StringLength(50)]
        [MinLength(3)]
        [Required]
        public string Title { get; set; }
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal Budget { get; set; }

        public ICollection<CommunityMembership> Membership { get; set; }

        public ICollection<CommunityAdvert> Adverts { get; set; }

    }
}
