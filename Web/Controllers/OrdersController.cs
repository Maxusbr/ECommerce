using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Web.Models;
using WebGrease.Css.Extensions;

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
                var rec = await OrderManager.GetReceiptAsync(el.Id);
                order.ReceiptExist = rec != null;
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
            var rec = await OrderManager.GetReceiptAsync(id);
            model.ReceiptExist = rec != null;
            return View(model);
        }

        // GET: Orders/Create
        [Authorize]
        public async Task<ActionResult> Create(string userId = "")
        {
            var dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var order = new CreateOrderViewModel { Date = dt };
            if (User.IsInRole("Менеджер"))
            {
                await InitOrder(order);
                if (!string.IsNullOrEmpty(userId)) order.UserId = userId;
            }
            else await InitOrder(order, User.Identity.GetUserId());

            return View(order);
        }

        // POST: Orders/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Create(CreateOrderViewModel model, Adress adress)
        {
            ValidateAdress(model.AdresShipping ?? adress);
            ValidateProduct(model.Products);
            if (ModelState.IsValid)
            {
                if (model.AdresShipping == null) model.AdresShipping = adress;
                await CreateOrder(model);
                return RedirectToAction("Index", "Products");
            }

            await InitListTypes(model);
            return View(model);
        }

        private void ValidateAdress(Adress model)
        {
            if (model == null)
                ModelState.AddModelError("AdresShipping", "Необхідно вказати адресу");
            if (string.IsNullOrEmpty(model?.Sity))
                ModelState.AddModelError("AdresShipping", "Необхідно вказати місто");
            if (string.IsNullOrEmpty(model?.Street))
                ModelState.AddModelError("AdresShipping", "Необхідно вказати вулицю");
            if (string.IsNullOrEmpty(model?.House))
                ModelState.AddModelError("AdresShipping", "Необхідно вказати номер будинка");
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
        public async Task<ActionResult> Edit(CreateOrderViewModel model, Adress adress)
        {
            if (model.AdresShipping == null) model.AdresShipping = adress;
            if (ModelState.IsValid)
            {
                await ModifyOrder(model);
                return RedirectToAction("Index");
            }
            if (string.IsNullOrEmpty(model.AdresShipping?.FullAdress))
                ModelState.AddModelError("AdresShipping", "Необхідно вказати адресу");
            await InitListTypes(model);
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

        public ActionResult AdressView(string userId)
        {
            var db = new ApplicationDbContext();
            var useradress = db.Users.Find(userId);
            db.Adresses.ToList();
            return PartialView(useradress?.Adress ?? new Adress());
        }

        private async Task InitListTypes(CreateOrderViewModel order)
        {
            order.ShippingTypes = await OrderManager.GetShippingTypesListAsync();
            order.PaymentTypes = await OrderManager.GetPaymentTypesListAsync();
            if (User.IsInRole("Менеджер")) await InitUsers(order);
        }
        private async Task InitOrder(CreateOrderViewModel order, string userid = "")
        {
            order.ShippingTypes = await OrderManager.GetShippingTypesListAsync();
            order.PaymentTypes = await OrderManager.GetPaymentTypesListAsync();

            await AdressManager.GetListAdressAsync();
            var shop = await AdressManager.GetShop();
            if (shop != null)
            {
                order.ShopId = shop.Id;
                order.ShopAdress = shop.Adress.FullAdress;
            }
            order.Products = await ProductManager.GetProducsAsync();
            if (!string.IsNullOrEmpty(userid))
            {
                var user = await UserManager.FindByIdAsync(userid);
                order.UserId = userid;
                order.AdresShipping = user.Adress ?? new Adress();
            }
            else
                await InitUsers(order);
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
            var shop = await AdressManager.GetShop();
            if (shop != null)
            {
                order.ShopId = shop.Id;
                order.ShopAdress = shop.Adress.FullAdress;
            }
            var useradress = await AdressManager.GetAdress(model.AdressId);
            if (useradress != null)
            {
                order.AdresShipping = useradress;
            }
            return order;
        }

        private async Task InitUsers(CreateOrderViewModel order)
        {
            var users = new List<ApplicationUser>();
            foreach (var user in await UserManager.Users.ToListAsync())
            {
                if (await UserManager.IsInRoleAsync(user.Id, "Користувач"))
                    users.Add(user);
            }
            order.Users = users;
            //order.NewUsers = new RegisterViewModel { Adress = new Adress() };
        }


        private async Task<Order> GetOrder(Order order, CreateOrderViewModel model)
        {
            order.User = await UserManager.FindByIdAsync(model.UserId);
            order.ShippingType = await OrderManager.GetShippingTypeByIdAsync(model.ShippingTypeId);
            order.PaymentType = await OrderManager.GetPaymentTypeByIdAsync(model.PaymentTypesId);
            order.Shop = await AdressManager.GetShop(model.ShopId);
            order.AdresShipping = await AdressManager.GetOrCreateAdress(model.AdresShipping);
            order.AdressId = order.AdresShipping.Id;
            order.Distance = model.Distance;
            return order;
        }

        private void ValidateProduct(IEnumerable<ProductInOrderViewModel> products)
        {
            if (products.Any(o => o.Count > 0)) return;
            ModelState.AddModelError("Products", "Виберіть товари для замовлення");
        }

        private async Task CreateOrder(CreateOrderViewModel model)
        {
            var order = await GetOrder(model.GetOrder, model);
            var products = model.Products.Where(o => o.Count > 0).ToList();

            order = await OrderManager.CreateOrUpdate(order);
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

        public ActionResult Register(string returnurl)
        {
            return RedirectToAction("Register", "Account", new { returnurl = returnurl });
        }

        public async Task<ActionResult> CreateReceipt(string id, string returnUrl)
        {
            var order = await OrderManager.FindAsync(id);
            var tariffModel = await GetTariffModel(order);
            var rec = new Receipt
            {
                Date = DateTime.Now,
                OrderId = order.Id,
                Order = order,
                ShippingCost = tariffModel.Tariff,
                Status = ReceiptStatus.Created
            };
            await CalculateProductCount(order.Id);
            if (!ModelState.IsValid)
                return RedirectToAction("Edit", order.Id);
            await OrderManager.CreateReceipt(rec);
            return RedirectToAction("DetailReceipt", new { id = rec.Id });
        }

        private async Task CalculateProductCount(string id)
        {
            foreach (var el in await ProductManager.GetProducsInOrderAsync(id))
            {
                var product = await ProductManager.FindAsync(el.Id);
                if (product.Count < el.Count)
                {
                    ModelState.AddModelError("", "Виберіть товари для замовлення");
                    return;
                }
                product.Count -= el.Count;
                await ProductManager.AddOrUpdate(product);
            }
        }

        public async Task<ActionResult> DetailReceipt(int id)
        {
            var rec = await OrderManager.GetReceiptByIdAsync(id);
            if (rec == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var order = rec.Order;
            var tariffModel = await GetTariffModel(order);
            var shippingCost = tariffModel.Tariff;

            var model = await InitOrder(order, order.UserId);
            model.Id = rec.Id.ToString("D6");
            model.ShippingType = (await OrderManager.GetShippingTypeByIdAsync(model.ShippingTypeId)).ToString();
            model.PaymentType = (await OrderManager.GetPaymentTypeByIdAsync(model.PaymentTypesId)).ToString();
            model.User = (await UserManager.FindByIdAsync(order.UserId)).ToString();
            model.Products.ForEach(o => o.ReadOnly = true);
            model.ShippingCost = shippingCost;
            model.Date = rec.Date;
            return View(model);
        }

        private async Task<TariffModel> GetTariffModel(Order order)
        {
            var result = new TariffModel { OrderId = order.Id };
            var products = await ProductManager.GetProducsInOrderAsync(order.Id);
            if (!products.Any()) return result;
            var wCat = products.Max(o => o.WCategoryId);
            var wCategory = await ProductManager.GetWeightCategoriesByIdAsync(wCat);
            result.WCategory = wCategory?.Name;
            result.UrbanCategory = order.Distance < 19 ? 1 : order.Distance < 40 ? 2 : 3;
            result.TariffKoefficient = await OrderManager.GetTariff(wCat, result.UrbanCategory);
            return result;
        }

        public async Task<ActionResult> ReceiptsView()
        {
            var result = new List<CreateOrderViewModel>();
            foreach (var el in await OrderManager.GetReceiptsListAsync())
            {
                var order = new CreateOrderViewModel(el.Order)
                {
                    Id = el.Id.ToString("D6"),
                    ShippingCost = el.ShippingCost,
                    User = (await UserManager.FindByIdAsync(el.Order.UserId)).ToString(),
                    Products = await ProductManager.GetProducsInOrderAsync(el.Order.Id),
                    ReceiptStatus = el.Status.ToString()
                };

                result.Add(order);
            }
            return View(result);
        }

        [HttpGet]
        public async Task<ActionResult> Logistic()
        {
            var shop = await AdressManager.GetShop();
            var basereceipts = (await OrderManager.GetReceiptsListAsync());

            var receipts = new List<ReceiptViewModel>();
            foreach (var rec in basereceipts.Where(o => o.Status == ReceiptStatus.Created))
            {
                receipts.Add(new ReceiptViewModel(rec)
                {
                    Products = await ProductManager.GetProducsInOrderAsync(rec.Order.Id),
                    TariffModel = await GetTariffModel(rec.Order),
                });
            }

            var model = new LogisticViewModel { ShopAdress = shop.Adress.FullAdress, Routes = new List<RouteViewModel>() };
            var routeId = 1;
            if (receipts.Any())
                foreach (var tariffcoeff in await OrderManager.GetTariffCoefficients())
                {
                    var recs = receipts.Where(o => o.ShippingType.SortId < 2 && o.TariffModel.TariffKoefficient == tariffcoeff);
                    if (recs.Any())
                    {
                        model.Routes.Add(new RouteViewModel
                        {
                            Id = routeId++,
                            UrbanId = tariffcoeff.UrbanCategoryId,
                            Orders = recs,
                            ShippingType = "за адресою",
                            SummOrderTariff = recs.Sum(o => o.ShippingCost),
                            OrderDistance = 2 * recs.Sum(o => o.Distance)
                        });
                    }
                    recs = receipts.Where(o => o.ShippingType.SortId == 2 && o.TariffModel.TariffKoefficient == tariffcoeff);
                    if (recs.Any())
                    {
                        model.Routes.Add(new RouteViewModel
                        {
                            Id = routeId++,
                            UrbanId = tariffcoeff.UrbanCategoryId,
                            Orders = recs,
                            ShippingType = "до пункту видачі",
                            SummOrderTariff = recs.Sum(o => o.ShippingCost),
                            OrderDistance = 2 * recs.Sum(o => o.Distance)
                        });
                    }
                }
            model.CountRoute = model.Routes.Count;
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Logistic(LogisticViewModel model)
        {
            return View(model);
        }

        public async Task<JsonResult> Calqulate(LogisticViewModel model)
        {
            return await OrderManager.CalqulateTariff(model.Routes);
            //return new JsonResult
            //{
            //    Data = new
            //    {
            //        data = res.Select(o => o.ToString(CultureInfo.CurrentCulture)),
            //        result = (345.8).ToString(CultureInfo.CurrentCulture)
            //    }
            //};
        }
    }
}
