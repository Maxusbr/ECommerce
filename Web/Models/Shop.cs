using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Models
{
    public class Shop
    {
        [Required]
        [Display(Name = "ID")]
        public string Id { get; set; }

        [Required]
        [Display(Name = "Назва магазину")]
        public string Name { get; set; }

        public string AdressId { get; set; }
        [ForeignKey("AdressId")]
        public Adress Adress { get; set; }
    }
}
