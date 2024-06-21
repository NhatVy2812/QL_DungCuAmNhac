using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebBanDungCu.Controllers
{
    public class MenuController : Controller
    {
        // GET: Menu
        QL_DCANEntities1 db = new QL_DCANEntities1();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MenuTop()
        {
            var items = db.MENUs.OrderBy(x => x.POSITION).ToList();
            return PartialView("MenuTop", items);
        }
        public ActionResult MenuArrivals()
        {
          var  items = db.LOAIs.ToList();
            return PartialView("MenuArrivals", items);
        }
        public ActionResult MenuLeft(int? id)
        {
            if (id != null)
            {
                ViewBag.CateId = id;
            }
            var items = db.LOAIs.ToList();
            return PartialView("MenuLeft", items);
        }
    }
}