using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class TransactRouteReceipt
    {
        [Key, Column(Order = 0)]
        public int RouteId { get; set; }
        [ForeignKey("RouteId")]
        public TransactRoute Route { get; set; }
        [Key, Column(Order = 1)]
        public int ReceiptId { get; set; }
        [ForeignKey("ReceiptId")]
        public Receipt Receipt { get; set; }
    }
}