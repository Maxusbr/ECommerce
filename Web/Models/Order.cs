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

    
   
}
