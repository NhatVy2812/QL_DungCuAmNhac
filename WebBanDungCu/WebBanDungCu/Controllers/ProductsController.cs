using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace WebBanDungCu.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Products
        QL_DCANEntities1 db = new QL_DCANEntities1();
        public ActionResult Index()
        {
            var item = db.SANPHAMs.ToList();
            //if(id!=null)
            //{
            //    item = item.Where(x => x.MALOAI == id).ToList();
            //}
            return View(item);
        }
        public ActionResult ProductCategory(int id)
        {
            var item = db.SANPHAMs.ToList();
            if (id > 0)
            {
                item = item.Where(x => x.MALOAI == id).ToList();
            }
            var cate = db.LOAIs.Find(id);
            if (cate != null)
            {
                ViewBag.CateName = cate.TENLOAI;
            }
            ViewBag.CateId = id;
            return View(item);
        }
        public ActionResult Partial_ItemByCateId()
        {
            var items = db.SANPHAMs.Where(x => x.IsActive).ToList();
            return PartialView(items);
        }
        public ActionResult Detail(int id)
        {
            var item = db.SANPHAMs.Find(id);
            return View(item);
        }
    }
}