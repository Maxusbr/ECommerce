using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Web.Models;

namespace Web.Controllers
{
    public class MarketingsController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        public AdressManager AdressManager => HttpContext.GetOwinContext().Get<AdressManager>();
        // GET: Marketings
        public async Task<ActionResult> Index()
        {
            return View(await _db.MarketEvents.ToListAsync());
        }

        // GET: AdvertisingView
        public ActionResult AdvertisingView()
        {
            return View();
        }
        // GET: Marketings/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MarketEvents marketEvents = await _db.MarketEvents.FindAsync(id);
            if (marketEvents == null)
            {
                return HttpNotFound();
            }
            marketEvents.Adress = await AdressManager.GetAdress(marketEvents.AdressId);
            return View(marketEvents);
        }

        // GET: Marketings/Create
        public ActionResult Create()
        {
            var dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var model = new MarketEvents {Id = Guid.NewGuid().ToString(), Adress = new Adress(), Date = dt};
            return View(model);
        }

        // POST: Marketings/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Date,Price")] MarketEvents marketEvents, Adress adress)
        {
            if (ModelState.IsValid)
            {
                marketEvents.Adress = await AdressManager.GetOrCreateAdress(adress);
                marketEvents.AdressId = marketEvents.Adress.Id;
                _db.MarketEvents.Add(marketEvents);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(marketEvents);
        }

        // GET: Marketings/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            var marketEvents = await _db.MarketEvents.FindAsync(id);
            if (marketEvents == null)
            {
                return HttpNotFound();
            }
            marketEvents.Adress = await AdressManager.GetAdress(marketEvents.AdressId);
            return View(marketEvents);
        }

        // POST: Marketings/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Date,Price")] MarketEvents marketEvents, Adress adress)
        {
            if (ModelState.IsValid)
            {
                var events = await _db.MarketEvents.FindAsync(marketEvents.Id);
                if(events == null) return RedirectToAction("Index");
                adress = await AdressManager.GetOrCreateAdress(adress);
                events.AdressId = adress.Id;
                events.Name = marketEvents.Name;
                events.Date = marketEvents.Date;
                events.Price = marketEvents.Price;
                _db.Entry(events).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(marketEvents);
        }

        // GET: Marketings/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MarketEvents marketEvents = await _db.MarketEvents.FindAsync(id);
            if (marketEvents == null)
            {
                return HttpNotFound();
            }
            return View(marketEvents);
        }

        // POST: Marketings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            MarketEvents marketEvents = await _db.MarketEvents.FindAsync(id);
            _db.MarketEvents.Remove(marketEvents);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
