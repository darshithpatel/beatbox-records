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
    public class AlbumFormatRepository : Repository<AlbumFormat>, IAlbumFormatRepository
    {
        private ApplicationDbContext _db;

        public AlbumFormatRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        
        public void Update(AlbumFormat obj)
        {
            _db.AlbumFormats.Update(obj);
        }

       
    }
}
