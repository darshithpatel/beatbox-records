using BeatBox.DataAccess;
using BeatBox.DataAccess.Repository.IRepository;
using BeatBox.Models;
using BeatBox.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BeatBox.Areas.Admin.Controllers
    
{
	[Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
	public class CategoryController : Controller
    {
		#region UnitOfWork
		private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region CategoryList Rendering (Index)
        public IActionResult Index()
        {
            IEnumerable<Category> objCategoryList = _unitOfWork.Category.GetAll();
            return View(objCategoryList);
        }
		#endregion

		#region Return Create View
		// GET (DISPLAY DATA TO PAGE FROM SERVER)
		public IActionResult Create()
        {
            return View();
        }
		#endregion

		#region Create Logic
		// POST (SENDS DATA INTO SERVER FROM CLIENT)
		[HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("CustomError", "The Display Order cannot exactly match the Name.");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category created successfully.";
                return RedirectToAction("Index");
            }
            return View(obj);
        }
		#endregion

		#region Return Edit View
		// GET (DISPLAY DATA TO PAGE FROM SERVER)
		public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            // var categoryFromDb = _db.Categories.Find(id);
            var categoryFromDbFirst = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);
            // var categoryFromDbSingle = _db.Categories.SingleOrDefault(u => u.Id == id);
            // Three ways to find and fetch id and its corresponding data.(1.Find 2.FirstOrDefault 3.SingleOrDefault)

            if (categoryFromDbFirst == null)
            {
                return NotFound();
            }

            return View(categoryFromDbFirst);
        }
		#endregion

		#region Edit Logic
		// POST (SENDS DATA INTO SERVER FROM CLIENT)
		[HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("CustomError", "The Display Order cannot exactly match the Name.");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category updated successfully.";
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
            // var categoryFromDb = _db.Categories.Find(id);
            var categoryFromDbFirst = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);
            // var categoryFromDbSingle = _db.Categories.SingleOrDefault(u => u.Id == id);
            // Three ways to find and fetch id and its corresponding data.(1.Find 2.FirstOrDefault 3.SingleOrDefault)

            if (categoryFromDbFirst == null)
            {
                return NotFound();
            }

            return View(categoryFromDbFirst);
        }
		#endregion

		#region Delete Logic
		// POST (SENDS DATA INTO SERVER FROM CLIENT)
		[HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var obj = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            _unitOfWork.Category.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Category deleted successfully.";
            return RedirectToAction("Index");
        }
		#endregion
	}
}
