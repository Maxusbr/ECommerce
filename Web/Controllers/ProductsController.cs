using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
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
            return View(new Product());
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

            var prices = await ProductManager.GetSalesProducsSummPriceAsync();
            var shipping = await ProductManager.GetSalesProducsShippingTypeAsync();
            var payment = await ProductManager.GetSalesProducsPaymentTypeAsync();
            var transact = await ProductManager.GetTransactRouteAsync();
            var model = new ChartsModel
            {
                Chart1 = GetChartCount(data, "count"),
                Chart2 = GetChartPrice(prices, "price"),
                Chart3 = GetChartShipping(shipping, "shipping"),
                Chart4 = GetChartPayment(payment, "payment"),
                Chart5 = GetChartTransact(transact, "transact")
            };
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

        private Highcharts GetChartCount(List<ProductInOrderViewModel> data, string name)
        {
            if (!data.Any()) return null;
            var height = Math.Max(400, data.Count * 60);
            var chart = new Highcharts("cart_" + name)
                        //define the type of chart 
                        .InitChart(new Chart { DefaultSeriesType = ChartTypes.Bar, Height = height, Width = null })
                        //overall Title of the chart 
                        .SetTitle(new Title { Text = "Кількість проданих товарів, шт." })
                        //small label below the main Title
                        //.SetSubtitle(new Subtitle { Text = "Accounting" })
                        //load the X values
                        .SetXAxis(new XAxis
                        {
                            Categories = data.Select(o => o.Name.Length > 34 ?
                            o.Name.Replace('\'', '`').Remove(34)+"...": o.Name.Replace('\'', '`')).ToArray(),
                            Labels = new XAxisLabels { Style = " fontSize: '16px'" },
                            Title = new XAxisTitle { Text = "" }
                        })
                        //set the Y title
                        .SetYAxis(new YAxis
                        {
                            Title = new YAxisTitle { Text = "Кількість товарів, шт.", Style = " fontSize: '18px'" },
                            Labels = new YAxisLabels { Style = " fontSize: '18px', fontWeight: 'bold'" },
                            AllowDecimals = false
                        })
                        .SetTooltip(new Tooltip
                        {
                            Enabled = true,
                            Formatter = @"function() { return '<b>'+ this.series.name +'</b><br/>'+ this.x +': <b>'+ this.y + '</b> шт.'; }"
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
                        .SetLegend(new Legend { Enabled = false })
                        //load the Y values 
                        .SetSeries(new[]
                    {
                        new Series {Name = "Кількість", Data = new Data(data.Select(o => new object[] { o.Count }).ToArray()),Color =Color.DodgerBlue   },
                            //you can add more y data to create a second line
                            // new Series { Name = "Other Name", Data = new Data(OtherData) }
                    });
            return chart;
        }

        private Highcharts GetChartPrice(List<ProductInOrderViewModel> data, string name)
        {
            if (!data.Any()) return null;
            var height = Math.Max(400, data.Count * 60);
            var chart = new Highcharts("cart_" + name)
                        .InitChart(new Chart { DefaultSeriesType = ChartTypes.Bar, Height = height, Width = null })
                        .SetTitle(new Title { Text = "Загальна вартість проданих товарів, грн." })
                        .SetXAxis(new XAxis
                        {
                            Categories = data.Select(o => o.Name.Length > 34 ?
                            o.Name.Replace('\'', '`').Remove(34) + "..." : o.Name.Replace('\'', '`')).ToArray(),
                            Labels = new XAxisLabels { Style = " fontSize: '16px'" },
                            Title = new XAxisTitle { Text = "" }
                        })
                        .SetYAxis(new YAxis
                        {
                            Title = new YAxisTitle { Text = "Вартість товарів, грн.", Style = " fontSize: '18px'" },
                            Labels = new YAxisLabels { Style = " fontSize: '18px', fontWeight: 'bold'" }
                        })
                        .SetTooltip(new Tooltip
                        {
                            Enabled = true,
                            Formatter = @"function() { return '<b>'+ this.series.name +'</b><br/>'+ this.x +': <b>'+ this.y + '</b> грн.'; }"
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
                        .SetLegend(new Legend { Enabled = false })
                        .SetSeries(new[]
                    {
                        new Series {Name = "Загальна вартість", Data =
                            new Data(data: data.Select(o => new object[] { o.Price }).ToArray()), Color =Color.Coral  },
                    });
            return chart;
        }
        private Highcharts GetChartShipping(List<ChartDataViewModel> data, string name)
        {
            if (!data.Any()) return null;
            var chart = new Highcharts("cart_" + name)
                        .InitChart(new Chart { DefaultSeriesType = ChartTypes.Bar, Height = null, Width = null })
                        .SetTitle(new Title { Text = "Кількість проданих товарів по типу доставки, шт." })
                        .SetXAxis(new XAxis
                        {
                            Categories = data.Select(o => o.Name.Replace('\'', '`')).ToArray(),
                            Labels = new XAxisLabels { Style = " fontSize: '16px'" },
                            Title = new XAxisTitle { Text = "" }
                        })
                        .SetYAxis(new YAxis
                        {
                            Title = new YAxisTitle { Text = "Кількість товарів, шт.", Style = " fontSize: '18px'" },
                            AllowDecimals = false,
                            Labels = new YAxisLabels { Style = " fontSize: '18px', fontWeight: 'bold'" }
                        })
                        .SetTooltip(new Tooltip
                        {
                            Enabled = true,
                            Formatter = @"function() { return '<b>'+ this.series.name +'</b><br/>'+ this.x +': <b>'+ this.y + '</b> шт.'; }"
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
                        .SetLegend(new Legend { Enabled = false })
                        .SetSeries(new[]
                    {
                        new Series {Name = "Кількість проданих товарів", Data =
                            new Data(data: data.Select(o => new object[] { o.Count }).ToArray()), Color =Color.DodgerBlue  },
                    });
            return chart;
        }
        private Highcharts GetChartPayment(List<ChartDataViewModel> data, string name)
        {
            if (!data.Any()) return null;
            var chart = new Highcharts("cart_" + name)
                        .InitChart(new Chart { DefaultSeriesType = ChartTypes.Bar, Height = null, Width = null })
                        .SetTitle(new Title { Text = "Кількість проданих товарів по формi оплати, шт." })
                        .SetXAxis(new XAxis
                        {
                            Categories = data.Select(o => o.Name.Replace('\'', '`')).ToArray(),
                            Labels = new XAxisLabels { Style = " fontSize: '16px'" },
                            Title = new XAxisTitle { Text = "" }
                        })
                        .SetYAxis(new YAxis
                        {
                            Title = new YAxisTitle { Text = "Кількість товарів, шт.", Style = " fontSize: '18px'" },
                            AllowDecimals = false,
                            Labels = new YAxisLabels { Style = " fontSize: '18px', fontWeight: 'bold'" }
                        })
                        .SetTooltip(new Tooltip
                        {
                            Enabled = true,
                            Formatter = @"function() { return '<b>'+ this.series.name +'</b><br/>'+ this.x +': <b>'+ this.y + '</b> шт.'; }"
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
                        .SetLegend(new Legend { Enabled = false })
                        .SetSeries(new[]
                    {
                        new Series {Name = "Кількість проданих товарів", Data =
                            new Data(data: data.Select(o => new object[] { o.Count }).ToArray()), Color =Color.Coral  },
                    });
            return chart;
        }
        private Highcharts GetChartTransact(List<ChartDataViewModel> data, string name)
        {
            if (!data.Any()) return null;
            var chart = new Highcharts("cart_" + name)
                        .InitChart(new Chart { DefaultSeriesType = ChartTypes.Spline, Height = null, Width = null })
                        .SetTitle(new Title { Text = "Загальні трансакційні витрати покупців, грн." })
                        .SetXAxis(new XAxis
                        {
                            Categories = data.Select(o => o.Count.ToString()).ToArray(),
                            Labels = new XAxisLabels { Style = " fontSize: '18px', fontWeight: 'bold'" },
                            AllowDecimals = false,
                            Title = new XAxisTitle { Text = "Номер маршруту", Style = " fontSize: '18px'" }
                        })
                        .SetYAxis(new YAxis { Title = new YAxisTitle { Text = "" }, AllowDecimals = true })
                        .SetTooltip(new Tooltip
                        {
                            Enabled = true,
                            Formatter = @"function() { return '<b>'+ this.series.name +'</b><br/>'+ this.x +': <b>'+ this.y + '</b> грн.'; }"
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
                        .SetLegend(new Legend { Enabled = false })
                        .SetSeries(new[]
                    {
                        new Series {Name = "Трансакційні витрати індівідуальні", Data =
                            new Data(data: data.Select(o => new object[] { o.Value2 }).ToArray()), Color =Color.Coral  },
                        new Series {Name = "Трансакційні витрати колективні", Data =
                            new Data(data: data.Select(o => new object[] { o.Value }).ToArray()), Color =Color.DodgerBlue  }
                    });
            return chart;
        }
    }


}
