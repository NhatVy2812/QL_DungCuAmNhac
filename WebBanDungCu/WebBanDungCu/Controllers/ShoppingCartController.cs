using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanDungCu.Models;

namespace WebBanDungCu.Controllers
{
    public class ShoppingCartController : Controller
    {
        // GET: ShoppingCart
        QL_DCANEntities1 db = new QL_DCANEntities1();
        public ActionResult Index()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if(cart != null)
            {
                return View(cart.Items);
            }
            return View();
        }
        public ActionResult CheckOut()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                ViewBag.CheckCart = cart;
            }
            return View();
            
        }
        public ActionResult CheckOutSuccess()
        {
            return View();
        }
        public ActionResult Partial_Item_ThanhToan()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                return PartialView(cart.Items);
            }
            return PartialView();
        }
        public ActionResult Partial_CheckOut()
        {
            return PartialView();
        }
        public ActionResult Partial_Item_Cart()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                return PartialView(cart.Items);
            }
            return PartialView();
        }
        public ActionResult ShowCount()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if(cart != null)
            {
                return Json(new {Count = cart.Items.Count }, JsonRequestBehavior.AllowGet);

            }
            return Json(new {Count = 0 },JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AddToCart(int id, int quantity)
        {
            var code = new { Success = false, msg = "", code = -1, Count = 0 };
            var db = new QL_DCANEntities1();
            var checkProduct = db.SANPHAMs.FirstOrDefault(x => x.ID == id);
            if(checkProduct !=null)
            {
                ShoppingCart cart = (ShoppingCart)Session["Cart"];
                if(cart == null)
                {
                    cart = new ShoppingCart();
                }
              
                    ShoppingCartItem item = new ShoppingCartItem
                    {
                        ProductId = checkProduct.ID,
                        ProductName = checkProduct.TENSP,
                        CateName = checkProduct.LOAI.TENLOAI,
                        Quantity = quantity
                    };
                    item.ProductImg = checkProduct.IMG;
                    item.Price = (decimal)checkProduct.GIA;
                    item.TotalPrice = item.Quantity * item.Price;
                    cart.AddToCart(item, quantity);
                        Session["cart"] = cart;
                        code = new { Success = true, msg = "Thêm Sản Phẩm vào Giỏ Hàng Thành Công", code = 1, Count = cart.Items.Count() };
            }
            return Json(code);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var code = new { Success = false, msg = "", code = -1, Count = 0 };
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if(cart!=null)
            {
                var checkProduct = cart.Items.FirstOrDefault(x => x.ProductId == id);
                if(checkProduct != null)
                {
                    cart.Remove(id);
                    code = new { Success = true, msg = "", code = 1, Count = cart.Items.Count };

                }

            }
            return Json(code);
        }
        [HttpPost]
        public ActionResult DeleteAll()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                cart.ClearCart();
                return Json(new { Success = true });
            }
            return Json(new { Success = false });
        }
        [HttpPost]
        public ActionResult Update(int id, int quantity)
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                cart.UpdateQuantity(id, quantity);
                return Json(new { Success = true });
            }
            return Json(new { Success = false });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOut(OrderViewModel req)
        {
            var code = new { Success = false, Code = -1 };
            if (ModelState.IsValid)
            {
                ShoppingCart cart = (ShoppingCart)Session["Cart"];
                if (cart != null)
                {
                    HOADON order = new HOADON();
                    order.TENKH = req.CustomerName;
                    order.SDT = req.Phone;
                    order.DIACHI = req.Address;
                    order.EMAIL = req.Email;
                    cart.Items.ForEach(x => order.CHITIETHDs.Add(new CHITIETHD
                    {
                        MASP = x.ProductId,
                        SOLUONG = x.Quantity,
                        THANHTIEN = x.Price
                    }));
                    order.TONGTIEN = cart.Items.Sum(x => (x.Price * x.Quantity));
                    order.TYPEPAYMENT = req.TypePayment;
                   
                    Random rd = new Random();
                    order.CODE = "DH" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);
                    //order.E = req.CustomerName;
                    db.HOADONs.Add(order);
                    db.SaveChanges();
                    //send mail cho khachs hang
                    var strSanPham = "";
                    var thanhtien = decimal.Zero;
                    var TongTien = decimal.Zero;
                    foreach (var sp in cart.Items)
                    {
                        strSanPham += "<tr>";
                        strSanPham += "<td>" + sp.ProductName + "</td>";
                        strSanPham += "<td>" + sp.Quantity + "</td>";
                        strSanPham += "<td>" + WebBanDungCu.Common.FormatNumber(sp.TotalPrice, 0) + "</td>";
                        strSanPham += "</tr>";
                        thanhtien += sp.Price * sp.Quantity;
                    }
                    TongTien = thanhtien;
                    string contentCustomer = System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/send2.html"));
                   contentCustomer = contentCustomer.Replace("{{MaDon}}", order.CODE);
                    contentCustomer = contentCustomer.Replace("{{SanPham}}", strSanPham);
                    contentCustomer = contentCustomer.Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"));
                    contentCustomer = contentCustomer.Replace("{{TenKhachHang}}", order.TENKH);
                    contentCustomer = contentCustomer.Replace("{{Phone}}", order.SDT);
                    contentCustomer = contentCustomer.Replace("{{Email}}", req.Email);
                    contentCustomer = contentCustomer.Replace("{{DiaChiNhanHang}}", order.DIACHI);
                    contentCustomer = contentCustomer.Replace("{{ThanhTien}}", WebBanDungCu.Common.FormatNumber(thanhtien, 0));
                    contentCustomer = contentCustomer.Replace("{{TongTien}}", WebBanDungCu.Common.FormatNumber(TongTien, 0));
                   WebBanDungCu.Common.SendMail("ShopOnline", "Đơn hàng #" + order.CODE, contentCustomer.ToString(), req.Email);

                    string contentAdmin = System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/send1.html"));
                    contentAdmin = contentAdmin.Replace("{{MaDon}}", order.CODE);
                    contentAdmin = contentAdmin.Replace("{{SanPham}}", strSanPham);
                    contentAdmin = contentAdmin.Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"));
                    contentAdmin = contentAdmin.Replace("{{TenKhachHang}}", order.TENKH);
                    contentAdmin = contentAdmin.Replace("{{Phone}}", order.DIACHI);
                    contentAdmin = contentAdmin.Replace("{{Email}}", req.Email);
                    contentAdmin = contentAdmin.Replace("{{DiaChiNhanHang}}", order.DIACHI);
                    contentAdmin = contentAdmin.Replace("{{ThanhTien}}", WebBanDungCu.Common.FormatNumber(thanhtien, 0));
                    contentAdmin = contentAdmin.Replace("{{TongTien}}", WebBanDungCu.Common.FormatNumber(TongTien, 0));
                   WebBanDungCu.Common.SendMail("ShopOnline", "Đơn hàng mới #" + order.CODE, contentAdmin.ToString(), ConfigurationManager.AppSettings["EmailAdmin"]);
                    cart.ClearCart();
                    return RedirectToAction("CheckOutSuccess");
                }
            }
            return Json(code);
        }

    }
}