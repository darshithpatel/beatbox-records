using BeatBox.DataAccess.Repository.IRepository;
using BeatBox.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeatBox.DataAccess.Repository
{
    public class ShopRepository : Repository<Shop>, IShopRepository
    {
        private ApplicationDbContext _db;

        public ShopRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        
        public void Update(Shop obj)
        {
            _db.Shops.Update(obj);
        }

       
    }
}
