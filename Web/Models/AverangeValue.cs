using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class AverangeValue
    {
        [Key]
        public int UrbanCategoryId { get; set; }
        public int Dhd { get; set; }
        public int Drp { get; set; }
        public int Cshd { get; set; }
        public int Csrp { get; set; }
        public int Tw { get; set; }
    }
}