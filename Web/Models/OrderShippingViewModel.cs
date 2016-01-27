using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class OrderShippingViewModel
    {
        public OrderShippingViewModel() { }

        public OrderShippingViewModel(Order order)
        {
            Id = order.Id;
            UserId = order.UserId;
            AdresShipping = order.AdresShipping.FullAdress;
            Distance = order.Distance;

        }

        [Display(Name = "ID")]
        public string Id { get; set; }
        [Display(Name = "Замовлення")]
        public int NumberOrder { get; set; }
        public string UserId { get; set; }
        public string UserPhone { get; set; }


        [Display(Name = "Адреса доставки")]
        public string AdresShipping { get; set; }
        [Display(Name = "Відстань доставки")]
        public double Distance { get; set; }
        [Display(Name = "Категорія урбанізації")]
        public string UrbanCategory { get; set; }
        [Display(Name = "Вагова категорія")]
        public string WCategory { get; set; }
        [Display(Name = "Тарифний коефіціент")]
        public double Tariff { get; set; }
    }
}