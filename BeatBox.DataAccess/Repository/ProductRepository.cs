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
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        #region Updating Data (Mapping entities for database)
        public void Update(Product obj)
        {
            var objFromDb = _db.Product.FirstOrDefault(u=>u.Id ==  obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Artist = obj.Artist;
                objFromDb.ISRC = obj.ISRC;
                objFromDb.Title= obj.Title;
                objFromDb.Description = obj.Description;
                objFromDb.Price = obj.Price;
                objFromDb.Price50 = obj.Price50;
                objFromDb.Price100 = obj.Price100;
                objFromDb.CategoryId = obj.CategoryId;
                objFromDb.AlbumFormatId = obj.AlbumFormatId;

                if(objFromDb.ImageUrl != null)
                {
                    objFromDb.ImageUrl = obj.ImageUrl;
                }
            }
        }

		#endregion

	}
}
