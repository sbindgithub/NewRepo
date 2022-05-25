using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
//using System.Web.WebPages.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWOrk;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductController(IUnitOfWork unitOfWOrk, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWOrk = unitOfWOrk;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            //IEnumerable<Product> objProductList = _unitOfWOrk.Product.GetAll();
            return View();
        }

        //GET
        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                Product = new(),
                CategoryList = _unitOfWOrk.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                CoverTypeList = _unitOfWOrk.CoverType.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
            };
            if (id == null || id == 0)
            {
                //create product
                //ViewBag.CategoryList = CategoryList;
                //ViewData["CoverTypeList"] = CoverTypeList;
                return View(productVM);
            }
            else
            {
                //update product
                productVM.Product = _unitOfWOrk.Product.GetFirstOrDefault(u => u.Id == id);
                return View(productVM);
            }
           
            return View(productVM);
        }

        //POST
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
                    var uploads=Path.Combine(wwwRootPath, @"images\products");
                    var extension=Path.GetExtension(file.FileName);

                    if (obj.Product.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var filestreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(filestreams);
                    }
                    obj.Product.ImageUrl = @"\images\products\" + fileName + extension;
                }
                if (obj.Product.Id == 0)
                {
                    _unitOfWOrk.Product.Add(obj.Product);
                }
                else
                {
                    _unitOfWOrk.Product.Update(obj.Product);
                }
                _unitOfWOrk.Save();
                TempData["success"] = "Product created successfully";
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
            var CoverTypeFromDbFirst = _unitOfWOrk.CoverType.GetFirstOrDefault(c => c.Id == id);
            if (CoverTypeFromDbFirst == null)
            {
                return NotFound();
            }

            return View(CoverTypeFromDbFirst);
        }

        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj= _unitOfWOrk.CoverType.GetFirstOrDefault(c => c.Id == id);


            if (obj==null)
            {
                return NotFound();
            }
            _unitOfWOrk.CoverType.Remove(obj);
            _unitOfWOrk.Save();
            TempData["success"] = "CoverType deleted successfully";
            return RedirectToAction("Index");
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var productList = _unitOfWOrk.Product.GetAll(includeProperties:"Category,CoverType");
            return Json(new { data = productList });
        }
        #endregion
    }
}
