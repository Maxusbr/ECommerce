using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Models
{
    public class MarketEvents
    {
        public MarketEvents()
        {
            Id = Guid.NewGuid().ToString();
        }

        [Required]
        [Display(Name = "ID")]
        public string Id { get; private set; }

        [Required]
        [Display(Name = "Найменування заходу")]
        public string Name { get; set; }

        public Adress Adress { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}")]
        [Display(Name = "Дата проведення")]
        public DateTime Date { get; set; }

        [Display(Name = "Вартість")]
        public double Price { get; set; }
    }
}
