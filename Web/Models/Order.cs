using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Web.Models
{
    public class Order
    {
        [Required]
        [Display(Name = "ID")]
        public string Id { get; set; }

        [ForeignKey("Shop")]
        public string ShopId { get; set; }
        [Required]
        public Shop Shop { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}")]
        [Display(Name = "Дата замовлення")]
        public DateTime Date { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        [Required]
        [Display(Name = "Клієнт")]
        public ApplicationUser User { get; set; }

        [ForeignKey("AdresShipping")]
        public string AdressId { get; set; }

        [Display(Name = "Адреса доставки")]
        [Required(ErrorMessage = "Необхідно вказати адресу")]
        public Adress AdresShipping { get; set; }

        [ForeignKey("ShippingType")]
        public string ShippingTypesId { get; set; }
        [Required]
        [Display(Name = "Тип доставки")]
        public ShippingType ShippingType { get; set; }

        [ForeignKey("PaymentType")]
        public string PaymentTypesId { get; set; }

        [Display(Name = "Форма оплати")]
        public PaymentType PaymentType { get; set; }

    }

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

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}")]
        [Display(Name = "Дата замовлення")]
        public DateTime Date { get; set; }

        [Display(Name = "Адреса доставки")]
        [Required(ErrorMessage = "Необхідно вказати адресу")]
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

        public Order GetOrder => new Order
        {
            Id = Id,
            Date = Date,
            PaymentTypesId = PaymentTypesId,
            ShippingTypesId = ShippingTypeId,
            ShopId = ShopId,
            UserId = UserId
        };
    }
}
