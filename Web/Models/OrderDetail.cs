﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Models
{
    public class OrderDetail
    {
        [Key, Column(Order = 0)]
        [ForeignKey("Order")]
        public string IdOrder { get; set; }
        [Required]
        public Order Order { get; set; }

        [Key, Column(Order = 1)]
        [ForeignKey("Product")]
        public string IdProduct { get; set; }
        [Required]
        public Product Product { get; set; }

        [Display(Name = "Кількість")]
        public int Count { get; set; }

        [Display(Name = "Ціна")]
        public double Price { get; set; }
    }
}
