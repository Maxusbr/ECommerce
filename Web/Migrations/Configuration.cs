using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Web.Models;

namespace Web.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            context.WeightCategories.AddOrUpdate(
            p => p.Name,
                  new WeightCategory { Id = 1, Name = "A" },
                  new WeightCategory { Id = 2, Name = "B" },
                  new WeightCategory { Id = 3, Name = "C" }
                );
            //context.TariffCoefficients.AddOrUpdate(
            //p => p.UrbanCategoryId,
            //      new TariffCoefficient { UrbanCategoryId = 1, WeightCategoryId = 1, ShippingCost = 35, Tariff = 1 },
            //      new TariffCoefficient { UrbanCategoryId = 1, WeightCategoryId = 2, ShippingCost = 50, Tariff = 2 },
            //      new TariffCoefficient { UrbanCategoryId = 1, WeightCategoryId = 3, ShippingCost = 55, Tariff = 4 },
            //      new TariffCoefficient { UrbanCategoryId = 2, WeightCategoryId = 1, ShippingCost = 35, Tariff = 2 },
            //      new TariffCoefficient { UrbanCategoryId = 2, WeightCategoryId = 2, ShippingCost = 50, Tariff = 4 },
            //      new TariffCoefficient { UrbanCategoryId = 2, WeightCategoryId = 3, ShippingCost = 55, Tariff = 10 },
            //      new TariffCoefficient { UrbanCategoryId = 3, WeightCategoryId = 1, ShippingCost = 35, Tariff = 7 },
            //      new TariffCoefficient { UrbanCategoryId = 3, WeightCategoryId = 2, ShippingCost = 50, Tariff = 14 },
            //      new TariffCoefficient { UrbanCategoryId = 3, WeightCategoryId = 3, ShippingCost = 55, Tariff = 20 }
            //    );
            //context.Products.AddOrUpdate(
            //p => p.Id,
            //      new Product {Id = Guid.NewGuid().ToString(), Name = "Продукт №1", Count = 150, Art = "Art#1", Price = 150, WCategoryId = 2, Weight = 10},
            //      new Product { Id = Guid.NewGuid().ToString(), Name = "Продукт №2", Count = 100, Art = "Art#2", Price = 50, WCategoryId = 1, Weight = 5},
            //      new Product { Id = Guid.NewGuid().ToString(), Name = "Продукт №3", Count = 50, Art = "Art#3", Price = 20, WCategoryId = 3, Weight = 20 }
            //    );
            //context.PaymentTypes.AddOrUpdate(p => p,
            //      new PaymentType {Type = "готівкою", SortId = 1},
            //      new PaymentType { Type = "пластиковою карткою", SortId = 2 },
            //      new PaymentType { Type = "банківський переказ", SortId = 3 }
            //    );
            //context.ShippingTypes.AddOrUpdate(p => p,
            //      new ShippingType { Type = "кур'єром", SortId = 1 },
            //      new ShippingType { Type = "додому", SortId = 2 },
            //      new ShippingType { Type = "у відділення пошти", SortId = 3 }
            //    );
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            //context.AverangeValues.AddOrUpdate(
            //p => p.UrbanCategoryId,
            //      new AverangeValue { UrbanCategoryId = 1, Dhd = 21, Drp = 15, Cshd = 84, Csrp = 16, Tw = 35},
            //      new AverangeValue { UrbanCategoryId = 2, Dhd = 45, Drp = 35, Cshd = 52, Csrp = 48, Tw = 50 },
            //      new AverangeValue { UrbanCategoryId = 3, Dhd = 79, Drp = 79, Cshd = 36, Csrp = 64, Tw = 55 }
            //    );
        }
    }
}
