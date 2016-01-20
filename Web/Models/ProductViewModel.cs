using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class ProductViewModel
    {
        [Required]
        [Display(Name = "ID")]
        public string Id { get; set; }

        [Display(Name = "Артикул")]
        public string Art { get; set; }

        [Display(Name = "Найменування товару")]
        public string Name { get; set; }

        [Display(Name = "Вагова категорія")]
        public string WCategory { get; set; }

        [Display(Name = "Кількість")]
        public int Count { get; set; }

        [Display(Name = "Ціна")]
        public double Price { get; set; }

        [Display(Name = "Тип доставки")]
        public ShippingType ShippingType { get; set; }

        [Display(Name = "Форма оплати")]
        public PaymentType PaymentType { get; set; }
    }
}