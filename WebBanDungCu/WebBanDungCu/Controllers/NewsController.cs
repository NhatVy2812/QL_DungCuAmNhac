using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
namespace WebBanDungCu.Controllers
{
    public class NewsController : Controller
    {
        // GET: News
        QL_DCANEntities1 db = new QL_DCANEntities1();
        public ActionResult Index(int? page)
        {
            var pageSize = 2;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<NEWS> items = db.NEWS.OrderByDescending(x => x.ID);
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }
        public ActionResult Detail(int id)
        {
            var item = db.NEWS.Find(id);
            return View(item);
        }
        public ActionResult Partial_News_Home()
        {
            var items = db.NEWS.Take(3).ToList();
            return PartialView(items);
        }
    }
}