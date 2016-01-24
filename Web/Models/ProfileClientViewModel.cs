using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class ProfileClientViewModel
    {
        [Display(Name = "Ідентифікатор")]
        public string Id { get; set; }
        [Display(Name = "Клієнт")]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        [Display(Name = "Товари")]
        public IEnumerable<ProductViewModel> Products { get; set; }

        [Display(Name = "Загальна вартість куплених товарів")]
        public double Summ { get; set; }
    }

    
}