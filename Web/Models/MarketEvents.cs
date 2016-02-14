using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Models
{
    public class MarketEvents
    {
        [Required]
        [Display(Name = "ID")]
        public string Id { get; set; }

        [Required]
        [Display(Name = "Найменування заходу")]
        public string Name { get; set; }
        public string AdressId { get; set; }
        [ForeignKey("AdressId")]
        [Display(Name = "Адреса проведення")]
        public Adress Adress { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}")]
        [Display(Name = "Дата проведення")]
        public DateTime Date { get; set; }

        [Display(Name = "Вартість, грн.")]
        public double Price { get; set; }
    }
}
