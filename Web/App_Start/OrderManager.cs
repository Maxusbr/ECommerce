using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Web.Models;

namespace Web
{
    public class OrderManager : IDisposable
    {
        public static OrderManager Create(IdentityFactoryOptions<OrderManager> options, IOwinContext context)
        {
            return new OrderManager(context.Get<ApplicationDbContext>());
        }

        private static ApplicationDbContext _db;

        public OrderManager(ApplicationDbContext applicationDbContext)
        {
            _db = applicationDbContext;
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            var result = await _db.Orders.ToListAsync();
            if (result.Any(o => o.AdresShipping == null)) await _db.Adresses.ToListAsync();
            if (result.Any(o => o.ShippingType == null)) await _db.ShippingTypes.ToListAsync();
            return result;
        }

        public async Task<Order> FindAsync(string id)
        {
            var result = await _db.Orders.FindAsync(id);
            if (result == null) return null;
            if (result.AdresShipping == null) await _db.Adresses.ToListAsync();
            if (result.ShippingType == null) await _db.ShippingTypes.ToListAsync();
            if (result.User == null) await _db.Users.ToListAsync();
            return result;
        }

        public async Task<Order> CreateOrUpdate(Order order)
        {
            var res = await FindAsync(order.Id);
            if (res == null)
            {
                if (string.IsNullOrEmpty(order.Id)) order.Id = Guid.NewGuid().ToString();
                _db.Entry(order).State = EntityState.Added;
            }
            else
                _db.Entry(order).State = EntityState.Modified;

            return order;
            //await _db.SaveChangesAsync();
        }
        public async Task<OrderDetail> DetailsFindAsync(string orderId, string productId)
        {
            var result = await _db.OrdersDetails.FindAsync(orderId, productId);
            if (result.Product == null) await _db.Products.ToListAsync();
            if (result.Order == null) await _db.Orders.ToListAsync();
            return result;
        }

        public async Task CreateOrUpdateDetails(OrderDetail detail)
        {
            var result = await _db.OrdersDetails.FindAsync(detail.IdOrder, detail.IdProduct);
            _db.Entry(detail).State = result == null ? detail.Count > 0 ? EntityState.Added : EntityState.Deleted : EntityState.Modified;
        }

        public async Task Delete(string id)
        {
            var order = await FindAsync(id);
            _db.Entry(order).State = EntityState.Deleted;
            await Save();
        }

        public async Task<List<ShippingType>> GetShippingTypesListAsync()
        {
            return await _db.ShippingTypes.OrderBy(o => o.SortId).ToListAsync();
        }

        public async Task<List<PaymentType>> GetPaymentTypesListAsync()
        {
            return await _db.PaymentTypes.OrderBy(o => o.SortId).ToListAsync();
        }


        public async Task<ShippingType> GetShippingTypeByIdAsync(string id)
        {
            return await _db.ShippingTypes.FindAsync(id);
        }

        public async Task<PaymentType> GetPaymentTypeByIdAsync(string id)
        {
            return await _db.PaymentTypes.FindAsync(id);
        }

        public async Task<IEnumerable<ProductViewModel>> GetUserProducts(string id)
        {
            var detail = await _db.OrdersDetails.ToListAsync();
            await _db.Products.ToListAsync();
            await _db.PaymentTypes.ToListAsync();
            await _db.ShippingTypes.ToListAsync();
            var orders = await _db.Orders.Where(o => o.UserId.Equals(id)).ToListAsync();
            var result = orders.Join(detail, e => e.Id, e => e.IdOrder, (order, orderDetail) => new ProductViewModel
            {
                Art = orderDetail.Product.Art,
                Name = orderDetail.Product.Name,
                Id = orderDetail.Product.Id,
                Count = orderDetail.Count,
                Price = orderDetail.Price,
                WCategory = orderDetail.Product.WCategory,
                ShippingType = order.ShippingType,
                PaymentType = order.PaymentType
            });
            return result;
        }

        public async Task Save()
        {
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception EX_NAME)
            {
                Debug.WriteLine(EX_NAME);
            }
        }

        public void Dispose()
        {
            _db.Dispose();
        }


    }
}