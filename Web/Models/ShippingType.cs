using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Models
{
    public class ShippingType
    {
        public ShippingType()
        {
            Id = Guid.NewGuid().ToString();
        }

        [Required]
        [Display(Name = "ID")]
        public string Id { get; private set; }

        [Required]
        [Display(Name = "Тип доставки")]
        public string Type { get; set; }

        public int SortId { get; set; }

        public override string ToString()
        {
            return Type;
        }
    }
}
