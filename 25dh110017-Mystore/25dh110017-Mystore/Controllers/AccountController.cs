using _25dh110017_Mystore.Models;
using _25dh110017_Mystore.Models.ViewModel;
using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace _25dh110017_Mystore.Controllers
{
    public class AccountController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // 1. ĐĂNG KÝ (GET)
        public ActionResult Register()
        {
            return View();
        }

        // 1. ĐĂNG KÝ (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem Username đã tồn tại chưa
                var checkUser = db.Users.FirstOrDefault(u => u.Username == model.Username);
                if (checkUser != null)
                {
                    ModelState.AddModelError("", "Tên đăng nhập này đã tồn tại.");
                    return View(model);
                }

                try
                {
                    // 1. Gán UserRole = "C" (1 ký tự để tránh lỗi Maximum Length = 1)
                    User newUser = new User
                    {
                        Username = model.Username,
                        Password = model.Password,
                        UserRole = "C" // "C": Customer, "A": Admin
                    };
                    db.Users.Add(newUser);

                    // 2. Gán CustomerPhone an toàn (tránh lỗi Required field)
                    Customer newCustomer = new Customer
                    {
                        Username = model.Username,
                        CustomerName = model.CustomerName,
                        CustomerEmail = model.Email,
                        CustomerPhone = string.IsNullOrEmpty(model.Phone) ? "0000000000" : model.Phone
                    };
                    db.Customers.Add(newCustomer);

                    db.SaveChanges();

                    // Đăng ký thành công -> Chuyển hướng sang trang đăng nhập
                    TempData["SuccessMessage"] = "Đăng ký tài khoản thành công! Vui lòng đăng nhập.";
                    return RedirectToAction("Login");
                }
                catch (DbEntityValidationException ex)
                {
                    // Bắt chi tiết lỗi Validation để hiển thị thông báo rõ ràng ra View
                    foreach (var entityErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in entityErrors.ValidationErrors)
                        {
                            ModelState.AddModelError("", $"Lỗi trường '{validationError.PropertyName}': {validationError.ErrorMessage}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Không thể tạo tài khoản: " + ex.Message);
                }
            }
            return View(model);
        }

        // 2. ĐĂNG NHẬP (GET)
        public ActionResult Login()
        {
            return View();
        }

        // 2. ĐĂNG NHẬP (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);
                if (user != null)
                {
                    Session["Username"] = user.Username;
                    FormsAuthentication.SetAuthCookie(user.Username, false);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không chính xác.");
            }
            return View(model);
        }

        // 3. ĐĂNG XUẤT
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
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