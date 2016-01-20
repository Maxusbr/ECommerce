using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Models
{
    public class Receipt
    {
        public Receipt()
        {
            Id = Guid.NewGuid().ToString();
        }

        [Required]
        [Display(Name = "ID")]
        public string Id { get; private set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}")]
        [Display(Name = "Дата квитанції")]
        public DateTime Date { get; set; }

        [Required]
        public Order Order { get; set; }
    }
}
