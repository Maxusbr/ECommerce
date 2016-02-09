using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Web.Models;
using Microsoft.AspNet.Identity.Owin;

namespace Web.Controllers
{
    [Authorize]
    public class ApplicationRolesController : Controller
    {
        private ApplicationRoleManager _roleManager;
        private ApplicationUserManager _userManager;

        public ApplicationRolesController() { }

        public ApplicationRolesController(ApplicationRoleManager roleManager)
        {
            RoleManager = roleManager;
        }
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

        // GET: ApplicationRoles
        [Authorize(Roles = "Адміністратор")]
        public async Task<ActionResult> Index()
        {
            return View(await UserManager.Users.ToListAsync());
        }

        // GET: ApplicationRoles/Details/5
        [Authorize(Roles = "Адміністратор")]
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var applicationRole = await UserManager.FindByIdAsync(id);
            if (applicationRole == null)
            {
                return HttpNotFound();
            }
            return View(applicationRole);
        }

        // GET: ApplicationRoles/Create
        [Authorize]
        public ActionResult Create()
        {
            var role = new ApplicationRole();
            return View(role);
        }

        // POST: ApplicationRoles/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Description")] ApplicationRole applicationRole)
        {
            if (ModelState.IsValid)
            {
                await RoleManager.CreateAsync(applicationRole);
                return RedirectToAction("Index");
            }

            return View(applicationRole);
        }

        // GET: ApplicationRoles/Edit
        [Authorize]
        public async Task<ActionResult> Edit(string id)
        {
            ViewBag.Roles = await RoleManager.Roles.ToListAsync();
            var users = string.IsNullOrEmpty(id) ? await UserManager.Users.ToListAsync() :
                await UserManager.Users.Where(o => o.Id.Equals(id)).ToListAsync();
            foreach (var user in users)
                user.RolesIds = user.Roles.Select(o => o.RoleId).ToArray();
            return View(users);
        }


        // POST: ApplicationRoles/Edit
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Edit(IEnumerable<ApplicationUser> applicationUsers)
        {
            if (ModelState.IsValid)
            {
                var roles = await RoleManager.Roles.ToArrayAsync();
                foreach (var user in applicationUsers)
                {
                    if (user.RolesIds == null || !user.RolesIds.Any())
                    {
                        await UserManager.RemoveFromRolesAsync(user.Id, roles.Select(o => o.Name).ToArray());
                        continue;
                    }
                    foreach (var role in roles)
                    {
                        var exist = user.RolesIds.FirstOrDefault(x => x == role.Id);
                        if (exist != null)
                            await UserManager.AddToRoleAsync(user.Id, role.Name);
                        else
                            await UserManager.RemoveFromRolesAsync(user.Id, role.Name);
                    }
                }
            }
            return RedirectToAction("Edit");
        }

        // GET: ApplicationRoles/Delete/5
        [Authorize(Roles = "Адміністратор")]
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var applicationRole = await RoleManager.FindByIdAsync(id);
            if (applicationRole == null)
            {
                return HttpNotFound();
            }
            return View(applicationRole);
        }

        // POST: ApplicationRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Адміністратор")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            var applicationRole = await RoleManager.FindByIdAsync(id);
            await RoleManager.DeleteAsync(applicationRole);
            return RedirectToAction("Index");
        }

    }
}
