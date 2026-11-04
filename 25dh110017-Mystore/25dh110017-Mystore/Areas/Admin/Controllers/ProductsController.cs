using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _25dh110017_Mystore.Models;
using _25dh110017_Mystore.Models.ViewModel;

namespace _25dh110017_Mystore.Areas.Admin.Controllers
{
    public class ProductsController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // GET: Admin/Products1 (Tích hợp Tìm kiếm & Sắp xếp)
        public ActionResult Index(string searchTerm, decimal? minPrice, decimal? maxPrice, string sortOrder)
        {
            var model = new ProductSearchVM
            {
                SearchTerm = searchTerm,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                SortOrder = sortOrder
            };

            var products = db.Products.Include(p => p.Category).AsQueryable();

            // 1. Tìm kiếm theo từ khóa (Tên sản phẩm hoặc Mô tả)
            if (!string.IsNullOrEmpty(searchTerm))
            {
                products = products.Where(p => p.ProductName.Contains(searchTerm) || p.ProductDescription.Contains(searchTerm));
            }

            // 2. Lọc theo khoảng giá
            if (minPrice.HasValue)
            {
                products = products.Where(p => p.ProductPrice >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                products = products.Where(p => p.ProductPrice <= maxPrice.Value);
            }

            // 3. Sắp xếp kết quả
            switch (sortOrder)
            {
                case "price_asc":
                    products = products.OrderBy(p => p.ProductPrice);
                    break;
                case "price_desc":
                    products = products.OrderByDescending(p => p.ProductPrice);
                    break;
                case "name_desc":
                    products = products.OrderByDescending(p => p.ProductName);
                    break;
                default:
                    products = products.OrderBy(p => p.ProductName);
                    break;
            }

            model.Products = products.ToList();
            return View(model);
        }

        // GET: Admin/Products/Details/5
        public ActionResult Details(int? id)
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

        // GET: Admin/Products1/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            return View();
        }

        // POST: Admin/Products1/Create (Tích hợp Upload hình ảnh)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,CategoryID,ProductName,ProductPrice,ProductDescription")] Product product, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid)
            {
                // Xử lý upload ảnh
                if (uploadImage != null && uploadImage.ContentLength > 0)
                {
                    string fileName = Path.GetFileNameWithoutExtension(uploadImage.FileName);
                    string extension = Path.GetExtension(uploadImage.FileName);
                    fileName = fileName + "_" + DateTime.Now.ToString("yymmddssfff") + extension;

                    string path = Path.Combine(Server.MapPath("~/Content/images/"), fileName);
                    product.ProductImage = "~/Content/images/" + fileName;
                    uploadImage.SaveAs(path);
                }

                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // GET: Admin/Products1/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // POST: Admin/Products1/Edit/5 (Tích hợp Cập nhật hình ảnh)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,CategoryID,ProductName,ProductPrice,ProductImage,ProductDescription")] Product product, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid)
            {
                // Nếu người dùng chọn ảnh mới để thay thế
                if (uploadImage != null && uploadImage.ContentLength > 0)
                {
                    string fileName = Path.GetFileNameWithoutExtension(uploadImage.FileName);
                    string extension = Path.GetExtension(uploadImage.FileName);
                    fileName = fileName + "_" + DateTime.Now.ToString("yymmddssfff") + extension;

                    string path = Path.Combine(Server.MapPath("~/Content/images/"), fileName);
                    product.ProductImage = "~/Content/images/" + fileName;
                    uploadImage.SaveAs(path);
                }

                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // GET: Admin/Products/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
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