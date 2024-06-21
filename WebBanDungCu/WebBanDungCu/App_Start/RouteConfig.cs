﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebBanDungCu
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
               name: "Products",
               url: "sanpham",
               defaults: new { controller = "Products", action = "Index", alias = UrlParameter.Optional },
               namespaces: new[] { "WebBanDungCu.Controllers" }
           );
            routes.MapRoute(
               name: "ShoppingCart",
               url: "gio-hang",
               defaults: new { controller = "ShoppingCart", action = "Index", alias = UrlParameter.Optional },
               namespaces: new[] { "WebBanDungCu.Controllers" }
           );
            routes.MapRoute(
              name: "CheckOut",
              url: "thanh-toan",
              defaults: new { controller = "ShoppingCart", action = "CheckOut", alias = UrlParameter.Optional },
              namespaces: new[] { "WebBanDungCu.Controllers" }
          );
            routes.MapRoute(
           name: "News",
           url: "baiviet",
           defaults: new { controller = "News", action = "Index", alias = UrlParameter.Optional },
           namespaces: new[] { "WebBanDungCu.Controllers" }
       );
            routes.MapRoute(
               name: "CategoryProduct",
               url: "danh-muc-san-pham/{alias}-{id}",
               defaults: new { controller = "Products", action = "ProductCategory", id = UrlParameter.Optional },
               namespaces: new[] { "WebBanDungCu.Controllers" }
           );
            routes.MapRoute(
              name: "DetailProduct",
              url: "chi-tiet/{alias}-p{id}",
              defaults: new { controller = "Products", action = "Detail", id = UrlParameter.Optional },
              namespaces: new[] { "WebBanDungCu.Controllers" }
          );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] {"WebBanDungCu.Controllers" }
            );
        }
    }
}