using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class LogisticViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Трансакційні витрати теорія")]
        public double Amount { get; set; }
        [Display(Name = "Адреса магазину")]
        public string ShopAdress { get; set; }
        public List<RouteViewModel> Routes { get; set; }
        public int CountRoute { get; set; }
    }

    public class ShippingWay
    {
        [Display(Name = "Маршрут доставки")]
        public int Id { get; set; }
        [Display(Name = "Номери замовлень")]
        public string NumbersOrders { get; set; }
        [Display(Name = "Тип доставки")]
        public string ShippingType { get; set; }
        [Display(Name = "Загальна дистанція маршруту")]
        public double Distance { get; set; }
        [Display(Name = "Трансакційні витрати на маршруті")]
        public double Amount { get; set; }
        public List<ReceiptViewModel> Receipts { get; set; }
    }
}