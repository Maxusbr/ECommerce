using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Models
{
    public class Adress
    {

        [Display(Name = "ID")]
        public string Id { get; set; }
        [Required]
        [Display(Name = "Місто")]
        public string Sity { get; set; }

        [Display(Name = "Вулиця")]
        public string Street { get; set; }

        [Display(Name = "Будинок")]
        public string House { get; set; }
        public string FullAdress
        {
            get
            {
                var res = !string.IsNullOrEmpty(Sity) ? Sity : "";
                if (!string.IsNullOrEmpty(Street)) res += ", " + Street;
                if (!string.IsNullOrEmpty(House)) res += ", " + House;
                return res;
            }
        }

        public override string ToString()
        {
            return FullAdress;
        }
    }
}
