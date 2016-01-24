using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class TariffCoefficient
    {
        [Key, Column(Order = 0)]
        public int UrbanCategoryId { get; set; }

        [Key, Column(Order = 1)]
        public int WeightCategoryId { get; set; }

        [ForeignKey("WeightCategoryId")]
        public WeightCategory WeightCategory { get; set; }

        public double ShippingCost { get; set; }
        public int Tariff { get; set; }
    }
}