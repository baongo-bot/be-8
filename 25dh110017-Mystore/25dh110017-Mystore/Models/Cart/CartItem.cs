using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _25dh110017_Mystore.Models.Cart
{
    public class CartItem
    {
        public Product product { get; set; }
        public int quantity { get; set; }

        // Thành tiền của từng mặt hàng
        public decimal TotalPrice => product != null ? product.ProductPrice * quantity : 0;
    }
}