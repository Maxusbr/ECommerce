using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Web.Models;

namespace Web
{
    public class AdressManager : IDisposable
    {
        public static AdressManager Create(IdentityFactoryOptions<AdressManager> options, IOwinContext context)
        {
            return new AdressManager(context.Get<ApplicationDbContext>());
        }

        private static ApplicationDbContext _db;

        public AdressManager(ApplicationDbContext applicationDbContext)
        {
            _db = applicationDbContext;
        }

        internal async Task UpdateAsync(ApplicationUser user, Adress adress)
        {
            adress = await GetOrCreateAdress(adress);
            var useradress = await _db.UserAdress.FirstOrDefaultAsync(o => o.IdUser.Equals(user.Id));
            if (useradress != null)
            {
                useradress.IdAdress = adress.Id;
                _db.Entry(useradress).State = EntityState.Modified;
            }
            else
            {
                useradress = new UserAdress { IdUser = user.Id, IdAdress = adress.Id };
                _db.Entry(useradress).State = EntityState.Added;
            }
            await _db.SaveChangesAsync();
        }

        public async Task<Adress> GetAdress(string id = "")
        {
            var adress = await _db.Adresses.FindAsync(id);
            return adress ?? new Adress { Sity = "Киев" };
        }

        public async Task<Adress> GetAdressUserId(string userId = "")
        {
            var useradress = await _db.UserAdress.FirstOrDefaultAsync(o => o.IdUser.Equals(userId));
            var adress = useradress != null ? await _db.Adresses.FirstOrDefaultAsync(o => o.Id.Equals(useradress.IdAdress)) : null;
            return adress;
        }

        public async Task<Adress> GetOrCreateAdress(string sity, string street, string house)
        {
            var adress = await _db.Adresses.FirstOrDefaultAsync(o => o.Sity.Equals(sity) && o.Street.Equals(street) && o.House.Equals(house));
            if (adress != null) return adress;
            adress = new Adress
            {
                Id = Guid.NewGuid().ToString(),
                Sity = sity,
                Street = street,
                House = house
            };
            _db.Entry(adress).State = EntityState.Added;
            await _db.SaveChangesAsync();

            return adress;
        }

        public async Task<Adress> GetOrCreateAdress(Adress model)
        {
            var adress = await _db.Adresses.FirstOrDefaultAsync(o => o.Sity.Equals(model.Sity) &&
                o.Street.Equals(model.Street) && o.House.Equals(model.House));
            if (adress != null)
            {
                _db.Entry(adress).State = EntityState.Unchanged;
                return adress;
            }
            adress = model;
            adress.Id = Guid.NewGuid().ToString();
            _db.Entry(adress).State = EntityState.Added;
            await _db.SaveChangesAsync();

            return adress;
        }

        public async Task<Shop> GetShop(string id = "")
        {
            if (string.IsNullOrEmpty(id))
                return await _db.Shops.FirstOrDefaultAsync();
            return await _db.Shops.FindAsync(id);
        }

        public Shop CreateShop(Adress adress)
        {
            var shop = new Shop { Adress = adress };
            if (adress != null) shop.AdressId = adress.Id;
            return shop;
        }

        public async Task<Shop> GetShopByUserId(string userId)
        {
            var userShop = await _db.UserShop.FirstOrDefaultAsync();
            return userShop != null ? await _db.Shops.FindAsync(userShop.IdShop) : null;
            //var shop = userShop == null ? await CreateShop() :
            //    await _db.Shops.FindAsync(userShop.IdShop);
            //if(!string.IsNullOrEmpty(shop.AdressId))
            //    shop.Adress = await _db.Adresses.FindAsync(shop.AdressId);
            //return shop;
        }

        public async Task CreateOrUpdate(Shop shop, string userId)
        {
            if (!string.IsNullOrEmpty(shop.Id))
            {
                _db.Entry(shop).State = EntityState.Modified;
            }
            else
            {
                shop.Id = Guid.NewGuid().ToString();
                var userShop = new UserShop { IdShop = shop.Id, IdUser = shop.Id };
                _db.Entry(shop).State = EntityState.Added;
                _db.Entry(userShop).State = EntityState.Added;
            }
            await _db.SaveChangesAsync();
        }

        public void Dispose()
        {
            _db.Dispose();
        }


    }
}