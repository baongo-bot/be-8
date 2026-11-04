using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _25dh110017_Mystore.Models.ViewModel
{
    public class CartItem
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public decimal ProductPrice { get; set; }
        public int Quantity { get; set; }

        // Thành tiền của từng món
        public decimal TotalPrice
        {
            get { return Quantity * ProductPrice; }
        }
    }
}