using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers
{
    [Area("Admin")]
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWOrk;

        public CoverTypeController(IUnitOfWork unitOfWOrk)
        {
            _unitOfWOrk = unitOfWOrk;

        }
        public IActionResult Index()
        {
            IEnumerable<CoverType> objCoverTypeList = _unitOfWOrk.CoverType.GetAll();
            return View(objCoverTypeList);
        }

        //GET
        public IActionResult Create()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CoverType obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWOrk.CoverType.Add(obj);
                _unitOfWOrk.Save();
                TempData["success"] = "CoverType created successfully";
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
            var CoverTypeFromDbFirst = _unitOfWOrk.CoverType.GetFirstOrDefault(c => c.Id == id);
            if(CoverTypeFromDbFirst == null)
            {
                return NotFound();
            }

            return View(CoverTypeFromDbFirst);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CoverType obj)
        {
           
            if (ModelState.IsValid)
            {
                _unitOfWOrk.CoverType.Update(obj);
                _unitOfWOrk.Save();
                TempData["success"] = "CoverType updated successfully";
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
    }
}
