using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWOrk;

        public CategoryController(IUnitOfWork unitOfWOrk)
        {
            _unitOfWOrk = unitOfWOrk;

        }
        public IActionResult Index()
        {
            IEnumerable<Category> objCategoryList= _unitOfWOrk.Category.GetAll();
            return View(objCategoryList);
        }

        //GET
        public IActionResult Create()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DsiplayOrder.ToString())
                ModelState.AddModelError("CustomErrorName", "The DisplayOrder cannot exactly match the name");
            if (ModelState.IsValid)
            {
                _unitOfWOrk.Category.Add(obj);
                _unitOfWOrk.Save();
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index");
            }else
                return View(obj);
        }

        //GET
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            //var categoryFromDb = _db.Categories.Find(id);
            var categoryFromDbFirst = _unitOfWOrk.Category.GetFirstOrDefault(c => c.Id == id);
            //var categoryFromDbSingle = _db.Categories.SingleOrDefault(c => c.Id == id);
            if(categoryFromDbFirst == null)
            {
                return NotFound();
            }

            return View(categoryFromDbFirst);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (obj.Name == obj.DsiplayOrder.ToString())
                ModelState.AddModelError("CustomErrorName", "The DisplayOrder cannot exactly match the name");
            if (ModelState.IsValid)
            {
                _unitOfWOrk.Category.Update(obj);
                _unitOfWOrk.Save();
                TempData["success"] = "Category updated successfully";
                return RedirectToAction("Index");
            }
            else
                return View(obj);
        }

        //GET
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            //var categoryFromDb = _db.Categories.Find(id);
            var categoryFromDbFirst = _unitOfWOrk.Category.GetFirstOrDefault(c => c.Id == id);
            //var categoryFromDbSingle = _db.Categories.SingleOrDefault(c => c.Id == id);
            if (categoryFromDbFirst == null)
            {
                return NotFound();
            }

            return View(categoryFromDbFirst);
        }

        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj= _unitOfWOrk.Category.GetFirstOrDefault(c => c.Id == id);


            if (obj==null)
            {
                return NotFound();
            }
            _unitOfWOrk.Category.Remove(obj);
            _unitOfWOrk.Save();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
