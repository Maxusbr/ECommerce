using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class CreateOrderViewModel
    {
        public CreateOrderViewModel() { }

        public CreateOrderViewModel(Order order)
        {
            Id = order.Id;
            UserId = order.UserId;
            ShopId = order.ShopId;
            Date = order.Date;
            AdresShipping = order.AdresShipping;
            ShippingTypeId = order.ShippingTypesId;
            PaymentTypesId = order.PaymentTypesId;
        }

        [Display(Name = "ID")]
        public string Id { get; set; }
        public string UserId { get; set; }
        [Display(Name = "Клієнт")]
        public string User { get; set; }
        public string ShopId { get; set; }
        public string ShopAdress { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}")]
        [Display(Name = "Дата замовлення")]
        public DateTime Date { get; set; }

        [Display(Name = "Адреса доставки")]
        public Adress AdresShipping { get; set; }

        [Required]
        [Display(Name = "Тип доставки")]
        public string ShippingTypeId { get; set; }
        [Display(Name = "Тип доставки")]
        public string ShippingType { get; set; }
        public List<ShippingType> ShippingTypes { get; set; }

        [Display(Name = "Форма оплати")]
        public string PaymentTypesId { get; set; }
        [Display(Name = "Форма оплати")]
        public string PaymentType { get; set; }

        public List<PaymentType> PaymentTypes { get; set; }
        public List<ProductInOrderViewModel> Products { get; set; }
        [Display(Name = "Користувачі")]
        public List<ApplicationUser> Users { get; set; }
        public bool ReceiptExist { get; set; }
        [Display(Name = "Дистанція доставки")]
        public double Distance { get; set; }
        public Order GetOrder => new Order
        {
            Id = Id,
            Date = Date,
            PaymentTypesId = PaymentTypesId,
            ShippingTypesId = ShippingTypeId,
            ShopId = ShopId,
            UserId = UserId,
            Distance = Distance
        };
        [Display(Name = "Вартість доставки")]
        public double ShippingCost { get; set; }
        public string ReceiptStatus { get; set; }
    }
}