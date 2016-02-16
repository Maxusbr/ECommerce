using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class ReceiptViewModel
    {
        public ReceiptViewModel(){ }

        public ReceiptViewModel(Receipt rec)
        {
            Id = rec.Id.ToString("D6"); 
            Adress = rec.Order.AdresShipping.FullAdress;
            Distance = rec.Order.Distance;
            PhoneNumber = rec.Order.User.PhoneNumber;
            ShippingCost = rec.ShippingCost;
            ReceiptStatus = rec.Status.ToString();
            ShippingType = rec.Order.ShippingType;
        }
        [Display(Name = "Тип доставки")]
        public ShippingType ShippingType { get; set; }

        public string Id { get; set; }
        [Display(Name = "Вартість доставки, грн.")]
        public double ShippingCost { get; set; }

        public string PhoneNumber { get; set; }
        [Display(Name = "Адреса доставки")]
        public string Adress { get; set; }

        public string ReceiptStatus { get; set; }
        public double Distance { get; set; }

        public IEnumerable<ProductInOrderViewModel> Products { get; set; }
        public TariffModel TariffModel { get; set; }
        [Display(Name = "Клієнт")]
        public string User { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'.'MM'.'yyyy}")]
        [Display(Name = "Дата квитанції")]
        public DateTime Date { get; set; }
        [Display(Name = "Форма оплати")]
        public PaymentType PaymentType { get; set; }
    }
}