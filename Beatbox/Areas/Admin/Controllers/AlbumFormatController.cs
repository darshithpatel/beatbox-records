using BeatBox.DataAccess;
using BeatBox.DataAccess.Repository.IRepository;
using BeatBox.Models;
using BeatBox.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeatBox.Areas.Admin.Controllers
    
{
	[Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class AlbumFormatController : Controller
    {
		#region UnitOfWork
		private readonly IUnitOfWork _unitOfWork;
        public AlbumFormatController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region AlbumFormatList Rendering (Index Method)
        public IActionResult Index()
        {
            IEnumerable<AlbumFormat> objAlbumFormatList = _unitOfWork.AlbumFormat.GetAll();
            return View(objAlbumFormatList);
        }
        #endregion

        #region Return Create View (Add AlbumFormat)
        // GET (DISPLAY DATA TO PAGE FROM SERVER)
        public IActionResult Create()
        {
            return View();
        }
        #endregion

        #region Create Logic (Logic of Adding AlbumFormat)
        // POST (SENDS DATA INTO SERVER FROM CLIENT)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AlbumFormat obj)
        {
            
            if (ModelState.IsValid)
            {
                _unitOfWork.AlbumFormat.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Format added successfully.";
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        #endregion

        #region Return Edit View (Update AlbumFormat)
        // GET (DISPLAY DATA TO PAGE FROM SERVER)
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            // var AlbumFormatFromDb = _db.Categories.Find(id);
            var AlbumFormatFromDbFirst = _unitOfWork.AlbumFormat.GetFirstOrDefault(u => u.Id == id);
            // var AlbumFormatFromDbSingle = _db.Categories.SingleOrDefault(u => u.Id == id);
            // Three ways to find and fetch id and its corresponding data.(1.Find 2.FirstOrDefault 3.SingleOrDefault)

            if (AlbumFormatFromDbFirst == null)
            {
                return NotFound();
            }

            return View(AlbumFormatFromDbFirst);
        }
        #endregion

        #region Edit Logic (Logic of Updating AlbumFormat)
        // POST (SENDS DATA INTO SERVER FROM CLIENT)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(AlbumFormat obj)
        {
            
            if (ModelState.IsValid)
            {
                _unitOfWork.AlbumFormat.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Format updated successfully.";
                return RedirectToAction("Index");
            }
            return View(obj);
        }
		#endregion

		#region Return Delete View
		// GET (DISPLAY DATA TO PAGE FROM SERVER)
		public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            // var AlbumFormatFromDb = _db.Categories.Find(id);
            var AlbumFormatFromDbFirst = _unitOfWork.AlbumFormat.GetFirstOrDefault(u => u.Id == id);
            // var AlbumFormatFromDbSingle = _db.Categories.SingleOrDefault(u => u.Id == id);
            // Three ways to find and fetch id and its corresponding data.(1.Find 2.FirstOrDefault 3.SingleOrDefault)

            if (AlbumFormatFromDbFirst == null)
            {
                return NotFound();
            }

            return View(AlbumFormatFromDbFirst);
        }
		#endregion

		#region Delete Logic
		// POST (SENDS DATA INTO SERVER FROM CLIENT)
		[HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var obj = _unitOfWork.AlbumFormat.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            _unitOfWork.AlbumFormat.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Format deleted successfully.";
            return RedirectToAction("Index");
        }
		#endregion
	}
}
