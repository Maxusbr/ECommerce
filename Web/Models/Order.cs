using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Web.Models
{
    public class Order
    {
        [Required]
        [Display(Name = "ID")]
        public string Id { get; set; }


        [Required]
        public string ShopId { get; set; }
        [ForeignKey("ShopId")]
        public Shop Shop { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}")]
        [Display(Name = "Дата замовлення")]
        public DateTime Date { get; set; }

        [Required]
        public string UserId { get; set; }

        [Display(Name = "Клієнт")]
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public string AdressId { get; set; }

        [Display(Name = "Адреса доставки")]
        [Required(ErrorMessage = "Необхідно вказати адресу")]
        [ForeignKey("AdressId")]
        public Adress AdresShipping { get; set; }


        public string ShippingTypesId { get; set; }
        [Required]
        [Display(Name = "Тип доставки")]
        [ForeignKey("ShippingTypesId")]
        public ShippingType ShippingType { get; set; }


        public string PaymentTypesId { get; set; }

        [Display(Name = "Форма оплати")]
        [ForeignKey("PaymentTypesId")]
        public PaymentType PaymentType { get; set; }

        public double Distance { get; set; }

        public bool ClosedOrder { get; set; }
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

        public int NumberOrder { get; set; }
    }

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
        public string UserId { get; set; }
        public string UserPhone { get; set; }


        [Display(Name = "Адреса доставки")]
        public string AdresShipping { get; set; }
        [Display(Name = "Відстань доставки")]
        public double Distance { get; set; }
    }
}
