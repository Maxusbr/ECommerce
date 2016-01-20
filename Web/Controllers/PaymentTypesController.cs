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
    public class PaymentTypesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PaymentTypes
        public async Task<ActionResult> Index()
        {
            return View(await db.PaymentTypes.ToListAsync());
        }

        // GET: PaymentTypes/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentType paymentType = await db.PaymentTypes.FindAsync(id);
            if (paymentType == null)
            {
                return HttpNotFound();
            }
            return View(paymentType);
        }

        // GET: PaymentTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PaymentTypes/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Type")] PaymentType paymentType)
        {
            if (ModelState.IsValid)
            {
                db.PaymentTypes.Add(paymentType);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(paymentType);
        }

        // GET: PaymentTypes/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentType paymentType = await db.PaymentTypes.FindAsync(id);
            if (paymentType == null)
            {
                return HttpNotFound();
            }
            return View(paymentType);
        }

        // POST: PaymentTypes/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Type")] PaymentType paymentType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(paymentType).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(paymentType);
        }

        // GET: PaymentTypes/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentType paymentType = await db.PaymentTypes.FindAsync(id);
            if (paymentType == null)
            {
                return HttpNotFound();
            }
            return View(paymentType);
        }

        // POST: PaymentTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            PaymentType paymentType = await db.PaymentTypes.FindAsync(id);
            db.PaymentTypes.Remove(paymentType);
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
