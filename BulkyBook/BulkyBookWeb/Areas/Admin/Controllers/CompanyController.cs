using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
//using System.Web.WebPages.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Controllers
{
    [Area("Admin")]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWOrk;
        public CompanyController(IUnitOfWork unitOfWOrk)
        {
            _unitOfWOrk = unitOfWOrk;
        }
        public IActionResult Index()
        {
            return View();
        }

        //GET
        public IActionResult Upsert(int? id)
        {
            Company company = new();
         
            if (id == null || id == 0)
            {
                return View(company);
            }
            else
            {
                company = _unitOfWOrk.Company.GetFirstOrDefault(u => u.Id == id);
                return View(company);
            }
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company obj)
        {
           
            if (ModelState.IsValid)
            {
                if (obj.Id == 0)
                {
                    _unitOfWOrk.Company.Add(obj);
                    TempData["success"] = "Company Create successfully";
                }
                else
                {
                    _unitOfWOrk.Company.Update(obj);
                    TempData["success"] = "Company updated successfully";
                }
                _unitOfWOrk.Save();
               
                return RedirectToAction("Index");
            }
            return View(obj);
        }

      

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var companyList = _unitOfWOrk.Company.GetAll();
            return Json(new { data = companyList });
        }

        //POST
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWOrk.Company.GetFirstOrDefault(c => c.Id == id);
            
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWOrk.Company.Remove(obj);
            _unitOfWOrk.Save();
            return Json(new { success = true, message = "Delete successful" });
        }
        #endregion
    }
}
