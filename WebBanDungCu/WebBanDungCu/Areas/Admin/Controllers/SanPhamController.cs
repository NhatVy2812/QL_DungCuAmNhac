using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;
using System.Data.Entity;
using WebBanDungCu.Models;
namespace WebBanDungCu.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SanPhamController : Controller
    {
       
        // GET: Admin/SanPham
        QL_DCANEntities1 db = new QL_DCANEntities1();
        //public ActionResult Index(int? page)
        //{

        //    var pageSize = 5;
        //    if(page ==null)
        //    {
        //        page = 1;
        //    }
        //    var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
        //    var items = db.SANPHAMs.OrderBy(x => x.MASP).ToPagedList(pageIndex, pageSize);
        //    //List<SANPHAM> list = db.SANPHAMs.ToList();
        //    return View(items);
        //}
        public ActionResult Index(int? page)
        {
            IEnumerable<SANPHAM> items = db.SANPHAMs.OrderByDescending(x => x.ID);
            var pageSize = 5;
            if (page == null)
            {
                page = 1;
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            //List<SANPHAM> list = db.SANPHAMs.ToList();
            return View(items);
        }
        public ActionResult Create()
        {
            ViewBag.LoaiSanPham = new SelectList(db.LOAIs.ToList(), "MALOAI", "TENLOAI");
           

            return View();
        }
        [HttpPost]
        public ActionResult Create(SANPHAM sp)
        {
            if (ModelState.IsValid)
            {
                db.SANPHAMs.Add(sp);
                db.SaveChanges();
                return RedirectToAction("Index", "SanPham");

            }
            return View(sp);
        }
        public ActionResult Edit(int masp)
        {
            SANPHAM sp = db.SANPHAMs.Single(d => d.ID == masp);
            if (sp == null)
            {
                return HttpNotFound();
            }
            return View(sp);
        }
        [HttpPost]
        public ActionResult Edit(SANPHAM sp)
        {
            if (ModelState.IsValid)
            {
                db.SANPHAMs.Attach(sp);
                db.Entry(sp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "SanPham");
            }
            return View(sp);
        }
        public ActionResult Delete(int masp)
        {
            var department = db.SANPHAMs.Find(masp);
            if (department == null)
            {
                return HttpNotFound();
            }
            //bool checkKhoaNgoai = db.SANPHAMs.Any(e => e.MASP == masp);
            //if (checkKhoaNgoai)
            //{
            //    ViewBag.ErrorMessage = "Có khoá ngoại tham chiếu không thể xoá";
            //    return View("Error");
            //}
            db.SANPHAMs.Remove(department);
            db.SaveChanges();
            return RedirectToAction("Index", "SanPham");
        }
        public ActionResult IsActive(int id)
        {
            var item = db.SANPHAMs.Find(id);
            if (item != null)
            {
                item.IsActive = !item.IsActive;
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, isActive = item.IsActive });
            }
            return Json(new { success = false });
        }
    }
}