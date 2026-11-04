using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;
using _25dh110017_Mystore.Models;
using _25dh110017_Mystore.Models.Cart;
using _25dh110017_Mystore.Models.ViewModel;

namespace _25dh110017_Mystore.Controllers
{
    public class OrderController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        private CartService GetCartService()
        {
            return new CartService(Session);
        }

        // 1. TRANG ĐẶT HÀNG (GET: Order/Checkout)
        public ActionResult Checkout()
        {
            var cart = GetCartService().GetCart();
            if (cart == null || !cart.Items.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            var model = new CheckoutVM
            {
                Cart = cart
            };

            // Nếu người dùng đã đăng nhập, tự động lấy thông tin điền sẵn
            if (Session["Username"] != null)
            {
                string username = Session["Username"].ToString();
                var customer = db.Customers.FirstOrDefault(c => c.Username == username);
                if (customer != null)
                {
                    model.CustomerName = customer.CustomerName;
                    model.Phone = customer.CustomerPhone;
                    model.DeliveryAddress = customer.CustomerAddress;
                }
            }

            // DÙNG CÁCH NÀY ĐỂ ÉP HỆ THỐNG ĐỌC ĐÚNG FILE, KHÔNG BAO GIỜ BỊ LỖI NỮA
            return View("Checkout");
        }

        // 2. XỬ LÝ ĐẶT HÀNG (POST: Order/Checkout)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Checkout(CheckoutVM model)
        {
            var cart = GetCartService().GetCart();
            model.Cart = cart;

            if (cart == null || !cart.Items.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            if (ModelState.IsValid)
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        // Tạo đối tượng Đơn hàng
                        Order order = new Order
                        {
                            OrderDate = DateTime.Now,
                            TotalAmount = cart.TotalValue()
                        };

                        // Kiểm tra nếu Model Order trong CSDL của bạn có các trường địa chỉ/trạng thái thì gán giá trị
                        // (Dùng để tránh lỗi biên dịch nếu tên cột trong CSDL của bạn khác biệt)
                        var orderType = typeof(Order);

                        if (orderType.GetProperty("DeliveryAddress") != null)
                        {
                            orderType.GetProperty("DeliveryAddress").SetValue(order, model.DeliveryAddress);
                        }
                        else if (orderType.GetProperty("ShippingAddress") != null)
                        {
                            orderType.GetProperty("ShippingAddress").SetValue(order, model.DeliveryAddress);
                        }

                        if (orderType.GetProperty("OrderStatus") != null)
                        {
                            orderType.GetProperty("OrderStatus").SetValue(order, "Pending");
                        }

                        // Gán thông tin khách hàng nếu đã đăng nhập
                        if (Session["Username"] != null)
                        {
                            string username = Session["Username"].ToString();
                            var customer = db.Customers.FirstOrDefault(c => c.Username == username);
                            if (customer != null)
                            {
                                order.CustomerID = customer.CustomerID;
                            }
                        }

                        db.Orders.Add(order);
                        db.SaveChanges(); // Lưu để sinh tự động OrderID

                        // Lưu chi tiết từng sản phẩm trong đơn hàng
                        foreach (var item in cart.Items)
                        {
                            OrderDetail detail = new OrderDetail
                            {
                                OrderID = order.OrderID,
                                ProductID = item.product.ProductID,
                                Quantity = item.quantity,
                                UnitPrice = item.product.ProductPrice
                            };
                            db.OrderDetails.Add(detail);
                        }

                        db.SaveChanges();
                        transaction.Commit();

                        // Đặt hàng thành công -> Xóa giỏ hàng
                        GetCartService().ClearCart();

                        return RedirectToAction("OrderSuccess", new { id = order.OrderID });
                    }
                    catch (DbEntityValidationException ex)
                    {
                        transaction.Rollback();
                        foreach (var entityErrors in ex.EntityValidationErrors)
                        {
                            foreach (var validationError in entityErrors.ValidationErrors)
                            {
                                ModelState.AddModelError("", $"Lỗi CSDL [{validationError.PropertyName}]: {validationError.ErrorMessage}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        ModelState.AddModelError("", "Đã xảy ra lỗi khi tạo đơn hàng: " + ex.Message);
                    }
                }
            }

            return View(model);
        }

        // 3. TRANG XÁC NHẬN ĐƠN HÀNG THÀNH CÔNG (GET: Order/OrderSuccess/5)
        public ActionResult OrderSuccess(int id)
        {
            var order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }

            return View(order);
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
