using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
// Sử dụng Alias để định nghĩa rõ ràng Class Cart, tránh đụng độ với Namespace Cart
using CartClass = _25dh110017_Mystore.Models.Cart.Cart;

namespace _25dh110017_Mystore.Models.ViewModel
{
    public class CheckoutVM
    {
        [Required(ErrorMessage = "Vui lòng nhập họ tên người nhận")]
        [Display(Name = "Họ và tên")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [Display(Name = "Số điện thoại")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ giao hàng")]
        [Display(Name = "Địa chỉ nhận hàng")]
        public string DeliveryAddress { get; set; }

        [Display(Name = "Ghi chú đơn hàng")]
        public string OrderNote { get; set; }

        // Thông tin giỏ hàng dùng Alias CartClass đã định nghĩa ở trên
        public CartClass Cart { get; set; }
    }
}