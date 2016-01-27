using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Web.Models;

namespace Web
{
    public class ProductManager : IDisposable
    {
        public static ProductManager Create(IdentityFactoryOptions<ProductManager> options, IOwinContext context)
        {
            return new ProductManager(context.Get<ApplicationDbContext>());
        }

        private static ApplicationDbContext _db;

        public ProductManager(ApplicationDbContext applicationDbContext)
        {
            _db = applicationDbContext;
        }
        public void Dispose()
        {
            _db.Dispose();
        }

        public async Task<List<Product>> GetAllProducsAsync()
        {
            await _db.WeightCategories.ToListAsync();
            var res = await _db.Products.OrderBy(o => o.Name).ToListAsync();
            return res;
        }

        public async Task<List<ProductInOrderViewModel>> GetProducsAsync()
        {
            await _db.WeightCategories.ToListAsync();
            return (from el in await _db.Products.OrderBy(o => o.Name).ToListAsync() select new ProductInOrderViewModel(el)).ToList();
        }

        public async Task<List<ProductInOrderViewModel>> GetSalesProducsAsync()
        {
            await _db.Products.ToListAsync(); await _db.WeightCategories.ToListAsync();
            var list =await _db.OrdersDetails.Join(_db.Receipts, d => d.IdOrder, r => r.OrderId,
                        (d, r) => new {Product = d.Product, Count = d.Count, Price = d.Price}).GroupBy(arg => arg.Product.Id).Select(g =>
            new ProductInOrderViewModel
            {
                Id = g.Key,
                Price = g.Average(p => p.Price),
                Count = g.Sum(p => p.Count)
            }).ToListAsync();
            foreach (var el in list)
            {
                var product = await FindAsync(el.Id);
                if(product == null) continue;
                el.Art = product.Art;
                el.MaxCount = product.Count;
                el.Name = product.Name;
            }
            return list.OrderBy(o => o.Name).ToList();
        }

        public async Task<List<ProductInOrderViewModel>> GetProducsInOrderAsync(string orderId)
        {
            var result = await _db.OrdersDetails.Where(o => o.IdOrder.Equals(orderId)).ToListAsync();
            if (result.Any(o => o.Product == null)) await _db.Products.ToListAsync();
            await _db.WeightCategories.ToListAsync();
            return result.Select(el => new ProductInOrderViewModel(el.Product)
                { Count = el.Count, Price = el.Price }).OrderBy(o => o.Name).ToList();
        }

        public async Task<Product> FindAsync(string id)
        {
            await _db.WeightCategories.ToListAsync();
            return await _db.Products.FindAsync(id);
        }

        public async Task AddOrUpdate(Product product)
        {
            await _db.WeightCategories.ToListAsync();
            if (string.IsNullOrEmpty(product.Id))
            {
                product.Id = Guid.NewGuid().ToString();
                _db.Entry(product).State = EntityState.Added;
            }
            else
                _db.Entry(product).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        public async Task Delete(string id)
        {
            var product = await FindAsync(id);
            _db.Entry(product).State = EntityState.Deleted;
            await _db.SaveChangesAsync();
        }

        public async Task<List<WeightCategory>> GetWeightCategoriesAsync()
        {
            return await _db.WeightCategories.ToListAsync();
        }
    }
}