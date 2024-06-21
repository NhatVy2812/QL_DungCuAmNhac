using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebBanDungCu.Models;
namespace WebBanDungCu.Areas.Admin.Controllers
{
    public class NewsController : Controller
    {
        QL_DCANEntities1 db = new QL_DCANEntities1();

        // GET: Admin/News
        public ActionResult Index(int? page)
        {
            //var items = db.NEWS.OrderByDescending(x => x.ID).ToList();
            IEnumerable<NEWS> items = db.NEWS.OrderByDescending(x => x.ID);
            var pageSize = 3;
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
            return View();
        }
        [HttpPost]
        public ActionResult Create(NEWS sp)
        {
            if (ModelState.IsValid)
            {
                db.NEWS.Add(sp);
                db.SaveChanges();
                return RedirectToAction("Index", "News");

            }
            return View(sp);
        }
        public ActionResult Edit(int id)
        {
            var item = db.NEWS.Find(id);
            return View(item);
        }
        [HttpPost]
        public ActionResult Edit(NEWS sp)
        {
            if (ModelState.IsValid)
            {
                db.NEWS.Attach(sp);
                db.Entry(sp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "News");
            }
            return View(sp);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.NEWS.Find(id);
            if(item !=null)
            {
                db.NEWS.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
        public ActionResult IsActive(int id)
        {
            var item = db.NEWS.Find(id);
            if (item != null)
            {
                item.IsActive = !item.IsActive;
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true , isActive = item.IsActive});
            }
            return Json(new { success = false });
        }
    }
}