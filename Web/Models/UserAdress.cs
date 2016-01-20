using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Models
{
    public class UserAdress
    {
        [ForeignKey("User")]
        [Key, Column(Order = 0)]
        public string IdUser { get; set; }
        public ApplicationUser User { get; set; }

        [ForeignKey("Adress")]
        [Key, Column(Order = 1)]
        public string IdAdress { get; set; }
        public Adress Adress { get; set; }
    }
}
