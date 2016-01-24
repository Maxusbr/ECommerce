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
    public class ApplicationUsersController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public AdressManager AdressManager => HttpContext.GetOwinContext().GetUserManager<AdressManager>();
        public OrderManager OrderManager => HttpContext.GetOwinContext().Get<OrderManager>();

        // GET: ApplicationUsers
        public async Task<ActionResult> Index()
        {
            var users = new List<ApplicationUser>();
            foreach (var user in await UserManager.Users.ToListAsync())
            {
                if (await UserManager.IsInRoleAsync(user.Id, "Користувач"))
                    users.Add(user);
            }
            return View(users);
        }


        // GET: ApplicationUsers/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = await UserManager.FindByIdAsync(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(await GetRegisterModel(applicationUser));
        }



        // POST: ApplicationUsers/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Surname,Name,MiddleName,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                await UpdateApplicationUser(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: ApplicationUsers/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = await UserManager.FindByIdAsync(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: ApplicationUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            ApplicationUser applicationUser = await UserManager.FindByIdAsync(id);
            var result = await UserManager.DeleteAsync(applicationUser);
            return RedirectToAction("Index");
        }

        // GET: ApplicationUsers/ProfileClient/5
        public async Task<ActionResult> ProfileClient(string id)
        {
            var user = new ProfileClientViewModel
            {
                Id = id,
                User = await UserManager.FindByIdAsync(id),
                Products = await OrderManager.GetUserProducts(id)
            };
            user.Summ = user.Products.Sum(o => o.Count*o.Price);
            return View(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        private async Task<RegisterViewModel> GetRegisterModel(ApplicationUser user)
        {
            var adress = await AdressManager.GetAdressUserIdAsync(user.Id) ?? new Adress { Sity = "Київ" };
            var result = new RegisterViewModel
            {
                Id = user.Id,
                Adress = adress,
                Email = user.Email,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                MiddleName = user.MiddleName,
                Name = user.Name,
                Surname = user.Surname
            };
            return result;
        }

        private async Task UpdateApplicationUser(RegisterViewModel model)
        {
            var user = await UserManager.FindByIdAsync(model.Id);
            user.MiddleName = model.MiddleName;
            user.Name = model.Name;
            user.Surname = model.Surname;
            user.PhoneNumber = model.PhoneNumber;
            await AdressManager.UpdateAsync(user, model.Adress);
            var result = await UserManager.UpdateAsync(user);
        }

    }
}
