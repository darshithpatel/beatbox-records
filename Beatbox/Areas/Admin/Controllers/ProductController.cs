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
using BeatBox.Utility;
using Microsoft.AspNetCore.Authorization;

namespace BeatBox.Areas.Admin.Controllers

{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
		#region UnitOfWork
		private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
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
            ProductVM productVM = new()
            {
                Product = new(),
                CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                AlbumFormatList = _unitOfWork.AlbumFormat.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
            };


            if (id == null || id == 0)
            {
                // if id=0 then new product will be created...
                //ViewBag.CategoryList = CategoryList;
                //ViewBag["AlbumFormatList"] = AlbumFormatList;
                return View(productVM);
            }
            else
            {
                productVM.Product = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
                return View(productVM);

                // product will be updated...

            }

        }
		#endregion

		#region Upsert Logic with Photo Upload
		// POST (SENDS DATA INTO SERVER FROM CLIENT)
		[HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj, IFormFile? file)
        {

            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;

                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\products");
                    var extension = Path.GetExtension(file.FileName);

                    if (obj.Product.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    obj.Product.ImageUrl = @"\images\products\" + fileName + extension;
                }

                if (obj.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(obj.Product);
                    TempData["success"] = "Album added successfully.";
                }
                else
                {
                    _unitOfWork.Product.Update(obj.Product);
                    TempData["success"] = "Album edited successfully.";
                }
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        #endregion

        #region Three ways to fetch data from id       
        // var ProductFromDb = _db.Categories.Find(id);
        // var ProductFromDbFirst = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
        // var ProductFromDbSingle = _db.Categories.SingleOrDefault(u => u.Id == id);
        // Three ways to find and fetch id and its corresponding data.(1.Find 2.FirstOrDefault 3.SingleOrDefault)
        #endregion

        #region API CALLS, Delete Logic (With Deleting Photo from Storage)
        [HttpGet]
        public IActionResult GetAll()
        {
            var productList = _unitOfWork.Product.GetAll(includeProperties: "Category,AlbumFormat");
            return Json(new { data = productList });
        }

        // POST (SENDS DATA INTO SERVER FROM CLIENT)
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Deleted successfully." });
        }

        #endregion
    }

}
