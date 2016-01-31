using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using Microsoft.AspNet.Identity.Owin;
using Web.Models;

namespace Web.Controllers
{
    public class ProductsController : Controller
    {
        public ProductsController() { }

        public ProductsController(ProductManager productManager)
        {
            ProductManager = productManager;
        }
        //private readonly ApplicationDbContext _db = new ApplicationDbContext();
        private ProductManager _productManager;
        public ProductManager ProductManager
        {
            get { return _productManager ?? HttpContext.GetOwinContext().Get<ProductManager>(); }
            private set { _productManager = value; }
        }

        // GET: Products
        public async Task<ActionResult> Index()
        {
            return View(await ProductManager.GetAllProducsAsync());
        }

        // GET: Products/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await ProductManager.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        [Authorize(Roles = "Менеджер")]
        public async Task<ActionResult> Create()
        {
            ViewBag.WCategory = await ProductManager.GetWeightCategoriesAsync();
            return View();
        }

        // POST: Products/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Менеджер")]
        public async Task<ActionResult> Create([Bind(Include = "Id,Art,Name,Weight,WCategoryId,Price,Count")] Product product)
        {
            if (ModelState.IsValid)
            {
                //_db.Products.Add(product);
                //await _db.SaveChangesAsync();
                product.WCategoryId = product.Weight < 10 ? 1 : product.Weight < 100 ? 2 : 3;
                await ProductManager.AddOrUpdate(product);
                return RedirectToAction("Index");
            }
            ViewBag.WCategory = await ProductManager.GetWeightCategoriesAsync();
            return View(product);
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "Менеджер")]
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await ProductManager.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.WCategory = await ProductManager.GetWeightCategoriesAsync();
            return View(product);
        }

        // POST: Products/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Менеджер")]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Art,Name,Weight,WCategoryId,Price,Count")] Product product)
        {
            if (ModelState.IsValid)
            {
                await ProductManager.AddOrUpdate(product);
                return RedirectToAction("Index");
            }
            ViewBag.WCategory = await ProductManager.GetWeightCategoriesAsync();
            return View(product);
        }

        // GET: Products/Delete/5
        [Authorize(Roles = "Менеджер")]
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await ProductManager.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Менеджер")]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            //Product product = await ProductManager.FindAsync(id);
            //_db.Products.Remove(product);
            //await _db.SaveChangesAsync();
            await ProductManager.Delete(id);
            return RedirectToAction("Index");
        }
        public async Task<ActionResult> SalesProducts()
        {
            var data = await ProductManager.GetSalesProducsAsync();
            return View(data);
        }

        public async Task<ActionResult> GraphView()
        {
            var data = await ProductManager.GetSalesProducsAsync();
            var model = new ChartsModel {Chart1 = GetChart(data, "count"),
                Chart2 = GetChart(data, "price"),Chart3 = GetChart(data, "shipping"), Chart4 = GetChart(data, "payment")};
            return View(model);
        }
        public ActionResult CountSalesProduct()
        {
            return View();
        }
        public ActionResult TotalPriceSalesProduct()
        {
            return View();
        }

        //public async Task<FileContentResult> GetChart()
        //{
        //    return File(await GetChartsProducts(), "image/png");
        //}


        //private async Task<byte[]> GetChartsProducts()
        //{
        //    if(Data == null) Data = await ProductManager.GetSalesProducsAsync();
        //    var chart = new Chart(300, 400, ChartTheme.Green);
        //    chart.AddTitle("Загальна вартість");
        //    chart.AddSeries("Counts", xValue: Data, xField: "Name", yValues: Data, yFields: "Count");
        //    //chart.AddSeries("TotalPrice", xValue: Data, xField: "Name", yValues: Data, yFields: "TotalPrice");
        //    return chart.GetBytes();
        //}
        //private static readonly ApplicationDbContext _db = new ApplicationDbContext();

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        _db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        private Highcharts GetChart(List<ProductInOrderViewModel> data, string name)
        {
            
            var chart = new Highcharts("cart_" + name)
                        //define the type of chart 
                        .InitChart(new Chart { DefaultSeriesType = ChartTypes.Bar })
                        //overall Title of the chart 
                        .SetTitle(new Title { Text = "Кількість проданих товарів" })
                        //small label below the main Title
                        .SetSubtitle(new Subtitle { Text = "Accounting" })
                        //load the X values
                        .SetXAxis(new XAxis { Categories = data.Select(o => o.Name).ToArray() })
                        //set the Y title
                        .SetYAxis(new YAxis { Title = new YAxisTitle { Text = "Number of Transactions" } })
                        .SetTooltip(new Tooltip
                        {
                            Enabled = true,
                            Formatter = @"function() { return '<b>'+ this.series.name +'</b><br/>'+ this.x +': '+ this.y; }"
                        })
                        .SetPlotOptions(new PlotOptions
                        {
                            Line = new PlotOptionsLine
                            {
                                DataLabels = new PlotOptionsLineDataLabels
                                {
                                    Enabled = true
                                },
                                EnableMouseTracking = false
                            }
                        })
                        //load the Y values 
                        .SetSeries(new[]
                    {
                        new Series {Name = "Hour", Data = new Data(data.Select(o => new object[] { o.Count }).ToArray())},
                            //you can add more y data to create a second line
                            // new Series { Name = "Other Name", Data = new Data(OtherData) }
                    });
            return chart;
        }
    }

    
}
