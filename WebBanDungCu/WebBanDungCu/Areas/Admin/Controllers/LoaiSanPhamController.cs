using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanDungCu.Models;
namespace WebBanDungCu.Areas.Admin.Controllers
{
    public class LoaiSanPhamController : Controller
    {
        QL_DCANEntities1 db = new QL_DCANEntities1();

        // GET: Admin/LoaiSanPham
        public ActionResult Index()
        {
            
            List<LOAI> list = db.LOAIs.ToList();
            return View(list);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]

        [ValidateAntiForgeryToken]
        public ActionResult Create(LOAI sp)
        {
            if (ModelState.IsValid)
            {
                
                db.LOAIs.Add(sp);
                db.SaveChanges();
                return RedirectToAction("Index", "LoaiSanPham");

            }
            return View(sp);
        }
    }
}