using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class TariffModel
    {
        [Display(Name = "OrderId")]
        public string OrderId { get; set; }
        [Display(Name = "Категорія урбанізації")]
        public int UrbanCategory { get; set; }
        [Display(Name = "Вагова категорія")]
        public string WCategory { get; set; }
        [Display(Name = "Тарифний коефіціент")]
        public double Tariff => TariffKoefficient?.ShippingCost * TariffKoefficient?.Tariff ?? 0;

        public TariffCoefficient TariffKoefficient { get; set; }
    }
}