using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Web.Models;

namespace Web.Controllers
{
    public class OrdersController : Controller
    {
        public OrderManager OrderManager => HttpContext.GetOwinContext().Get<OrderManager>();

        public AdressManager AdressManager => HttpContext.GetOwinContext().Get<AdressManager>();

        public ProductManager ProductManager => HttpContext.GetOwinContext().Get<ProductManager>();

        public ApplicationUserManager UserManager => HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

        // GET: Orders
        [Authorize(Roles = "Менеджер")]
        public async Task<ActionResult> Index()
        {
            var result = new List<CreateOrderViewModel>();
            foreach (var el in await OrderManager.GetOrdersAsync())
            {
                var order = new CreateOrderViewModel(el)
                {
                    User = (await UserManager.FindByIdAsync(el.UserId)).ToString(),
                    Products = await ProductManager.GetProducsInOrderAsync(el.Id)
                };
                result.Add(order);
            }
            return View(result);
        }

        // GET: Orders/Details/5
        [Authorize(Roles = "Менеджер")]
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var order = await OrderManager.FindAsync(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            var model = await InitOrder(order, order.UserId);
            model.ShippingType = (await OrderManager.GetShippingTypeByIdAsync(model.ShippingTypeId)).ToString();
            model.PaymentType = (await OrderManager.GetPaymentTypeByIdAsync(model.PaymentTypesId)).ToString();
            model.User = (await UserManager.FindByIdAsync(order.UserId)).ToString();
            model.Products.ForEach(o => o.ReadOnly = true);

            return View(model);
        }

        // GET: Orders/Create
        [Authorize]
        public async Task<ActionResult> Create()
        {
            var dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var order = new CreateOrderViewModel { Date = dt };
            await InitOrder(order, User.Identity.GetUserId());

            return View(order);
        }

        // POST: Orders/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Create(CreateOrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                await CreateOrder(model);
                return RedirectToAction("Index", "Products");
            }
            if (string.IsNullOrEmpty(model.AdresShipping?.FullAdress))
                ModelState.AddModelError("AdresShipping", "Необхідно вказати адресу");
            

            await InitShippingTypes(model);
            return View(model);
        }

        // GET: Orders/Edit/5
        [Authorize(Roles = "Менеджер")]
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var order = await OrderManager.FindAsync(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            var model = await InitOrder(order, order.UserId);
            return View(model);
        }

        // POST: Orders/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Менеджер")]
        public async Task<ActionResult> Edit(CreateOrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                await ModifyOrder(model);
                return RedirectToAction("Index");
            }
            if (string.IsNullOrEmpty(model.AdresShipping?.FullAdress))
                ModelState.AddModelError("AdresShipping", "Необхідно вказати адресу");
            await InitShippingTypes(model);
            return View(model);
        }

        // GET: Orders/Delete/5
        [Authorize(Roles = "Менеджер")]
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = await OrderManager.FindAsync(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Менеджер")]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            await OrderManager.Delete(id);
            return RedirectToAction("Index");
        }

        private async Task InitShippingTypes(CreateOrderViewModel order)
        {
            order.ShippingTypes = await OrderManager.GetShippingTypesListAsync();
            order.PaymentTypes = await OrderManager.GetPaymentTypesListAsync();
        }
        private async Task InitOrder(CreateOrderViewModel order, string userid)
        {
            order.ShippingTypes = await OrderManager.GetShippingTypesListAsync();
            order.PaymentTypes = await OrderManager.GetPaymentTypesListAsync();
            var shop = await AdressManager.GetShop();
            if (shop != null) order.ShopId = shop.Id;
            order.Products = await ProductManager.GetProducsAsync();
            order.UserId = userid;
            var useradress = await AdressManager.GetAdressUserId(userid);
            if (useradress != null)
            {
                order.AdresShipping = useradress;
            }
        }
        private async Task<CreateOrderViewModel> InitOrder(Order model, string userid)
        {
            var order = new CreateOrderViewModel(model)
            {
                ShippingTypes = await OrderManager.GetShippingTypesListAsync(),
                PaymentTypes = await OrderManager.GetPaymentTypesListAsync(),
                Products = await ProductManager.GetProducsInOrderAsync(model.Id),
                UserId = userid
            };
            var useradress = await AdressManager.GetAdress(model.AdressId);
            if (useradress != null)
            {
                order.AdresShipping = useradress;
            }
            return order;
        }
        private async Task<Order> GetOrder(Order order,  CreateOrderViewModel model)
        {
            order.User = await UserManager.FindByIdAsync(model.UserId);
            order.ShippingType = await OrderManager.GetShippingTypeByIdAsync(model.ShippingTypeId);
            order.PaymentType = await OrderManager.GetPaymentTypeByIdAsync(model.PaymentTypesId);
            order.Shop = await AdressManager.GetShop(model.ShopId);
            order.AdresShipping = await AdressManager.GetOrCreateAdress(model.AdresShipping);
            order.AdressId = order.AdresShipping.Id;
            return order;
        }

        private async Task CreateOrder(CreateOrderViewModel model)
        {
            var order = await GetOrder(model.GetOrder, model);
            order = await OrderManager.CreateOrUpdate(order);
            var products = model.Products.Where(o => o.Count > 0).ToList();
            if (products.Any())
            {
                foreach (var el in products)
                {
                    var product = await ProductManager.FindAsync(el.Id);
                    if (product == null) continue;
                    var detail = new OrderDetail
                    {
                        IdOrder = order.Id,
                        Product = product,
                        Order = order,
                        IdProduct = product.Id,
                        Count = el.Count,
                        Price = product.Price
                    };
                    await OrderManager.CreateOrUpdateDetails(detail);
                }
                await OrderManager.Save();
            }
        }

        private async Task ModifyOrder(CreateOrderViewModel model)
        {
            var order = await GetOrder(await OrderManager.FindAsync(model.Id), model);
            order = await OrderManager.CreateOrUpdate(order);
            foreach (var el in model.Products)
            {
                var detail = await OrderManager.DetailsFindAsync(order.Id, el.Id);
                if (detail == null) continue;
                detail.Count = el.Count;

                await OrderManager.CreateOrUpdateDetails(detail);
            }
            await OrderManager.Save();
        }

    }
}
