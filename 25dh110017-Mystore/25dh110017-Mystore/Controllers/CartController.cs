using _25dh110017_Mystore.Models;
using _25dh110017_Mystore.Models.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _25dh110017_Mystore.Controllers
{
    public class CartController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // Khởi tạo CartService với Session hiện tại
        private CartService GetCartService()
        {
            return new CartService(Session);
        }

        // GET: Cart - Trang hiển thị danh sách giỏ hàng
        public ActionResult Index()
        {
            var cart = GetCartService().GetCart();
            return View(cart);
        }

        // GET/POST: Cart/AddToCart - Thêm sản phẩm vào giỏ hàng
        public ActionResult AddToCart(int? id, int? productID, int quantity = 1)
        {
            int pId = id ?? productID ?? 0;
            if (pId > 0)
            {
                var product = db.Products.Find(pId);
                if (product != null)
                {
                    GetCartService().AddItem(product, quantity);
                }
            }

            // Thêm xong tự quay lại trang người dùng vừa đứng
            if (Request.UrlReferrer != null)
            {
                return Redirect(Request.UrlReferrer.ToString());
            }
            return RedirectToAction("Index");
        }

        // POST: Cart/UpdateQuantity - Cập nhật số lượng sản phẩm
        [HttpPost]
        public ActionResult UpdateQuantity(int productID, int quantity)
        {
            GetCartService().UpdateQuantity(productID, quantity);
            return RedirectToAction("Index");
        }

        // GET/POST: Cart/RemoveFromCart - Xóa sản phẩm khỏi giỏ hàng
        public ActionResult RemoveFromCart(int? id, int? productID)
        {
            int pId = id ?? productID ?? 0;
            if (pId > 0)
            {
                GetCartService().RemoveLine(pId);
            }

            return RedirectToAction("Index");
        }

        // Partial View hiển thị tổng số lượng sản phẩm trên Menu/Header
        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = GetCartService().GetCart();
            return PartialView("_PVCartSummary", cart);
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