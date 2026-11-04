using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using _25dh110017_Mystore.Models;
using _25dh110017_Mystore.Models.ViewModel;

namespace _25dh110017_Mystore.Controllers
{
    public class HomeController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // GET: Home/Index
        public ActionResult Index(string searchTerm, int? page)
        {
            var model = new HomeProductVM();
            var products = db.Products.AsQueryable();

            // Tìm kiếm sản phẩm theo từ khóa (đã khắc phục lỗi Null và phân biệt hoa/thường)
            if (!string.IsNullOrEmpty(searchTerm))
            {
                model.SearchTerm = searchTerm;
                string searchLower = searchTerm.Trim().ToLower();

                products = products.Where(p => (p.ProductName != null && p.ProductName.ToLower().Contains(searchLower)) ||
                                               (p.ProductDescription != null && p.ProductDescription.ToLower().Contains(searchLower)) ||
                                               (p.Category != null && p.Category.CategoryName != null && p.Category.CategoryName.ToLower().Contains(searchLower)));
            }

            int pageNumber = page ?? 1;
            int pageSize = 6;

            // 1. Sản phẩm nổi bật (10 sản phẩm bán chạy hoặc mới nhất)
            model.FeaturedProducts = products
                .OrderByDescending(p => p.OrderDetails.Count())
                .ThenByDescending(p => p.ProductID)
                .Take(10)
                .ToList();

            // 2. Sản phẩm mới nhất (Sắp xếp giảm dần theo ProductID và phân trang)
            model.NewProducts = products
                .OrderByDescending(p => p.ProductID)
                .ToPagedList(pageNumber, pageSize);

            return View(model);
        }

        // GET: Home/ProductDetail/5
        public ActionResult ProductDetail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }

        // GET: Home/About
        public ActionResult About()
        {
            ViewBag.Message = "Trang giới thiệu thông tin nhà sách BookStore.";
            return View();
        }

        // GET: Home/Contact
        public ActionResult Contact()
        {
            ViewBag.Message = "Trang liên hệ hỗ trợ khách hàng.";
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}