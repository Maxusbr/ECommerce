using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Attributes;

namespace Web.Models
{
    public class Product
    {
        [Required]
        [Display(Name = "ID")]
        public string Id { get; set; }

        [Display(Name = "Артикул")]
        public string Art { get; set; }

        [Required]
        [Display(Name = "Найменування товару")]
        public string Name { get; set; }

        [Display(Name = "Вага")]
        public double Weight { get; set; }

        [Display(Name = "Вагова категорія")]
        public int WCategoryId { get; set; }
        [ForeignKey("WCategoryId")]
        public WeightCategory WCategory { get; set; }

        [Display(Name = "Ціна")]
        public double Price { get; set; }

        [Display(Name = "Кількість на складі")]
        public int Count { get; set; }
    }

    public class ProductInOrderViewModel
    {
        public ProductInOrderViewModel() { }

        public ProductInOrderViewModel(Product product)
        {
            Id = product.Id;
            Art = product.Art;
            Name = product.Name;
            Weight = product.Weight;
            WCategoryId = product.WCategoryId;
            Price = product.Price;
            MaxCount = product.Count;
        }

        [Required]
        [Display(Name = "ID")]
        public string Id { get; set; }

        [Display(Name = "Артикул")]
        public string Art { get; set; }

        [Required]
        [Display(Name = "Найменування товару")]
        public string Name { get; set; }

        [Display(Name = "Вага")]
        public double Weight { get; set; }

        [Display(Name = "Вагова категорія")]
        public int WCategoryId { get; set; }
        public List<WeightCategory> WCategories { get; set; }

        [Display(Name = "Ціна")]
        public double Price { get; set; }

        [Display(Name = "Кількість")]
        [NumericLessThan("MaxCount", "має бути меньше ніж Кількість на складі", AllowEquality = true)]
        public int Count { get; set; }

        [Display(Name = "Кількість на складі")]
        public int MaxCount { get; set; }

        [Display(Name = "Загальна вартість")]
        public double TotalPrice => Price * Count;
        public bool ReadOnly { get; set; }
    }
}
