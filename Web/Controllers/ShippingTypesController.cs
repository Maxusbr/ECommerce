using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class ShippingTypesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ShippingTypes
        public async Task<ActionResult> Index()
        {
            return View(await db.ShippingTypes.ToListAsync());
        }

        // GET: ShippingTypes/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShippingType shippingType = await db.ShippingTypes.FindAsync(id);
            if (shippingType == null)
            {
                return HttpNotFound();
            }
            return View(shippingType);
        }

        // GET: ShippingTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ShippingTypes/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Type")] ShippingType shippingType)
        {
            if (ModelState.IsValid)
            {
                db.ShippingTypes.Add(shippingType);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(shippingType);
        }

        // GET: ShippingTypes/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShippingType shippingType = await db.ShippingTypes.FindAsync(id);
            if (shippingType == null)
            {
                return HttpNotFound();
            }
            return View(shippingType);
        }

        // POST: ShippingTypes/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Type")] ShippingType shippingType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shippingType).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(shippingType);
        }

        // GET: ShippingTypes/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShippingType shippingType = await db.ShippingTypes.FindAsync(id);
            if (shippingType == null)
            {
                return HttpNotFound();
            }
            return View(shippingType);
        }

        // POST: ShippingTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            ShippingType shippingType = await db.ShippingTypes.FindAsync(id);
            db.ShippingTypes.Remove(shippingType);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
