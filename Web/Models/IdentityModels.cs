using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Web.Models
{
    // Чтобы добавить данные профиля для пользователя, можно добавить дополнительные свойства в класс ApplicationUser. Дополнительные сведения см. по адресу: http://go.microsoft.com/fwlink/?LinkID=317594.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Обратите внимание, что authenticationType должен совпадать с типом, определенным в CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Здесь добавьте утверждения пользователя
            return userIdentity;
        }

        [MaxLength(128)]
        [Display(Name = "Прізвище")]
        public string Surname { get; set; }

        [MaxLength(128)]
        [Display(Name = "Ім'я")]
        public string Name { get; set; }

        [MaxLength(128)]
        [Display(Name = "По батькові")]
        public string MiddleName { get; set; }

        public string FIO => $"{Surname} {Name} {MiddleName}";

        public string[] RolesIds { get; set;}

        public IEnumerable<string> RolesNames { get; set;}

        public string AdressId { get; set;}
        [ForeignKey("AdressId")]
        public Adress Adress { get; set;}

        public override string ToString()
        {
            return string.IsNullOrEmpty(Name) ? UserName: FIO;
        }
    }

    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base() { Id = Guid.NewGuid().ToString(); }
        public ApplicationRole(string name) : base(name) { Id = Guid.NewGuid().ToString();}
        public string Description { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            
        }

        static ApplicationDbContext()
        {
            //Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<ApplicationRole> IdentityRoles { get; set; }

        public DbSet<MarketEvents> MarketEvents { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<PaymentType> PaymentTypes { get; set; }

        public DbSet<ShippingType> ShippingTypes { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrdersDetails { get; set; }

        public DbSet<Receipt> Receipts { get; set; }

        public DbSet<Shop> Shops { get; set; }

        public DbSet<Adress> Adresses { get; set; }

        public DbSet<UserShop> UserShop { get; set; }

        public DbSet<WeightCategory> WeightCategories { get; set; }

        public DbSet<TariffCoefficient> TariffCoefficients { get; set; }

        public DbSet<AverangeValue> AverangeValues { get; set; }
    }
}