using BeatBox.DataAccess;
using BeatBox.DataAccess.Repository.IRepository;
using BeatBox.Models;
using BeatBox.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using BeatBox.Utility;

namespace BeatBox.Areas.Admin.Controllers
    
{
	[Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ShopController : Controller
    {
		#region UnitOfWork
		private readonly IUnitOfWork _unitOfWork;
        public ShopController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
		#endregion

		#region Index
		public IActionResult Index()
        {
            return View();
        }
		#endregion

		#region Return Upsert View
		// GET (DISPLAY DATA TO PAGE FROM SERVER)
		public IActionResult Upsert(int? id)
        {
            Shop shop = new();
        
            if (id == null || id == 0)
            {
                // if id=0 then new product will be created...
                //ViewBag.CategoryList = CategoryList;
                //ViewBag["AlbumFormatList"] = AlbumFormatList;
                return View(shop);
            }
            else
            {
                shop = _unitOfWork.Shop.GetFirstOrDefault(u => u.Id == id);
                return View(shop);

                // shop will be updated...

            }

        }
		#endregion

		#region Upsert Logic
		// POST (SENDS DATA INTO SERVER FROM CLIENT)
		[HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Shop obj, IFormFile? file)
        {
            
            if (ModelState.IsValid)
            {

                if(obj.Id == 0) 
                { 
                    _unitOfWork.Shop.Add(obj);
                    TempData["success"] = "Shop data added successfully.";

                }
                else
                {
                    _unitOfWork.Shop.Update(obj);
                    TempData["success"] = "Shop data updated successfully.";
                }
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(obj);
        }
		#endregion

		#region API CALLS, Delete Logic
		[HttpGet]
        public IActionResult GetAll()
        {
            var shopList = _unitOfWork.Shop.GetAll();
            return Json(new { data = shopList });
        }

        // POST (SENDS DATA INTO SERVER FROM CLIENT)
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.Shop.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.Shop.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Deleted successful" });
        }

        #endregion
    }

}
