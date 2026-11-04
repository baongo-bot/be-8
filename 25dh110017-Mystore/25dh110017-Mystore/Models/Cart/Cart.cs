using _25dh110017_Mystore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _25dh110017_Mystore.Models.Cart
{
    public class Cart
    {
        private List<CartItem> items = new List<CartItem>();

        // Cho phép truy cập danh sách mặt hàng dưới dạng IEnumerable hoặc List
        public IEnumerable<CartItem> Items => items;

        // Thêm sản phẩm vào giỏ
        public void AddItem(Product product, int quantity)
        {
            if (product == null) return;

            var item = items.FirstOrDefault(p => p.product.ProductID == product.ProductID);
            if (item == null)
            {
                items.Add(new CartItem { product = product, quantity = quantity });
            }
            else
            {
                item.quantity += quantity;
            }
        }

        // Cập nhật số lượng sản phẩm
        public void UpdateQuantity(int productId, int quantity)
        {
            var item = items.FirstOrDefault(p => p.product.ProductID == productId);
            if (item != null)
            {
                if (quantity > 0)
                {
                    item.quantity = quantity;
                }
                else
                {
                    items.Remove(item);
                }
            }
        }

        // Xóa sản phẩm khỏi giỏ hàng
        public void RemoveLine(int productId)
        {
            items.RemoveAll(l => l.product != null && l.product.ProductID == productId);
        }

        // Tính tổng tiền toàn giỏ hàng
        public decimal TotalValue()
        {
            return items.Sum(e => e.TotalPrice);
        }

        // Tính tổng số lượng tất cả sản phẩm
        public int TotalQuantity()
        {
            return items.Sum(e => e.quantity);
        }

        // Xóa sạch giỏ hàng
        public void Clear()
        {
            items.Clear();
        }
    }
}