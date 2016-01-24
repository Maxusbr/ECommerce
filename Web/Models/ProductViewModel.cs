using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public int WCategoryId { get; set; }

        [Display(Name = "Кількість")]
        public int Count { get; set; }

        [Display(Name = "Ціна")]
        public double Price { get; set; }
        public string ShippingTypeId { get; set; }
        [Display(Name = "Тип доставки")]
        [ForeignKey("ShippingTypeId")]
        public ShippingType ShippingType { get; set; }
        public string PaymentTypeId { get; set; }
        [Display(Name = "Форма оплати")]
        [ForeignKey("PaymentTypeId")]
        public PaymentType PaymentType { get; set; }
    }
}