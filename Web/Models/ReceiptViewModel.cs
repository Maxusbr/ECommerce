using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class ReceiptViewModel
    {
        public ReceiptViewModel(){ }

        public ReceiptViewModel(Receipt rec)
        {
            Id = rec.Id;
            Adress = rec.Order.AdresShipping.FullAdress;
            Distance = rec.Order.Distance;
            PhoneNumber = rec.Order.User.PhoneNumber;
            ShippingCost = rec.ShippingCost;
            ReceiptStatus = rec.Status.ToString();
            ShippingType = rec.Order.ShippingType;
        }

        public ShippingType ShippingType { get; set; }

        public int Id { get; set; }

        public double ShippingCost { get; set; }

        public string PhoneNumber { get; set; }

        public string Adress { get; set; }

        public string ReceiptStatus { get; set; }
        public double Distance { get; set; }

        public IEnumerable<ProductInOrderViewModel> Products { get; set; }
        public TariffModel TariffModel { get; set; }
    }
}