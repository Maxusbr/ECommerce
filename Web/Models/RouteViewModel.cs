using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class RouteViewModel
    {
        [Display(Name = "Маршрут")]
        public int Id { get; set; }

        public IEnumerable<ReceiptViewModel> Orders { get; set; }
        [Display(Name = "Тип доставки")]
        public string ShippingType { get; set; }
        [Display(Name = "Номери замовлень")]
        public string OrdersIds
        {
            get
            {
                var res = Orders != null && Orders.Any() ? Orders.Aggregate("", (current, el) =>
                    string.IsNullOrEmpty(current) ? current + el.Id : current + ", " + el.Id) : "";
                return res;
            }
        }
        [Display(Name = "Загальна дистанція маршруту")]
        public double TotalDistance { get; set; }

        public string OrdersAdresses
        {
            get
            {
                var res = Orders != null && Orders.Any() ? Orders.Aggregate("", (current, el) => 
                    string.IsNullOrEmpty(current) ? current + el.Adress : current + ";" + el.Adress) : "";
                return res;
            }
        }

        [Display(Name = "Число адрес на маршруті")]
        public int CountOrders => Orders != null && Orders.Any() ? Orders.Count() : 0;
        [Display(Name = "Сума")]        
        public double TotalTariff => Orders != null && Orders.Any() ? Orders.Sum(o => o.ShippingCost) : 0;
        [Display(Name = "Трансакційні витрати на маршруті")]
        public double RouteTariff { get; set; }
    }
}