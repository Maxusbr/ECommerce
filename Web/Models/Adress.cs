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
        [Required]
        [Display(Name = "ID")]
        public string Id { get; set; }
        [Required]
        [Display(Name = "Місто")]
        public string Sity { get; set; }
        [Required]
        [Display(Name = "Вулиця")]
        public string Street { get; set; }
        [Required]
        [Display(Name = "Будинок")]
        public string House { get; set; }
        public string FullAdress => !string.IsNullOrEmpty(Sity) ? $"{Sity}, {Street}, {House}": "";

        public override string ToString()
        {
            return FullAdress;
        }
    }
}
