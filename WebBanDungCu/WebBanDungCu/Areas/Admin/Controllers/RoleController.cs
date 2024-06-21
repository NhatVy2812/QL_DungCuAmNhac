using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanDungCu.Models;
using WebBanDungCu.Migrations;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace WebBanDungCu.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        // GET: Admin/Role
        //QL_DCANEntities db = new QL_DCANEntities();
        private ApplicationDbContext db1 = new ApplicationDbContext();
        public ActionResult Index()
        {
            var items = db1.Roles.ToList();
            return View(items);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(IdentityRole model)
        {
            if (ModelState.IsValid)
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db1));
                roleManager.Create(model);
                return RedirectToAction("Index", "Role");
                //db.SANPHAMs.Add(sp);
                //db.SaveChanges();
                //return RedirectToAction("ShowProduct", "Admin");

            }
            return View(model);
        }
        public ActionResult Edit(int id)
        {
            var item = db1.Roles.Find(id);
            return View(item);
        }
        [HttpPost]
        public ActionResult Edit(IdentityRole model)
        {
            if (ModelState.IsValid)
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db1));
                roleManager.Update(model);
                return RedirectToAction("Index", "Role");
                //db.SANPHAMs.Add(sp);
                //db.SaveChanges();
                //return RedirectToAction("ShowProduct", "Admin");

            }
            return View(model);
        }
    }
}