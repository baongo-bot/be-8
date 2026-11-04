using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _25dh110017_Mystore.Models;

namespace _25dh110017_Mystore.Models.Cart
{
    public class CartService
    {
        private readonly HttpSessionStateBase session;
        private const string CartSessionKey = "CartSession";

        public CartService(HttpSessionStateBase session)
        {
            this.session = session;
        }

        // Lấy đối tượng Cart từ Session (tự tạo mới nếu chưa có)
        public Cart GetCart()
        {
            if (session == null)
            {
                return new Cart();
            }

            Cart cart = session[CartSessionKey] as Cart;
            if (cart == null)
            {
                cart = new Cart();
                session[CartSessionKey] = cart;
            }
            return cart;
        }

        // Thêm sản phẩm vào giỏ
        public void AddItem(Product product, int quantity)
        {
            var cart = GetCart();
            cart.AddItem(product, quantity);
            SaveCart(cart);
        }

        // Cập nhật số lượng sản phẩm
        public void UpdateQuantity(int productId, int quantity)
        {
            var cart = GetCart();
            cart.UpdateQuantity(productId, quantity);
            SaveCart(cart);
        }

        // Xóa sản phẩm khỏi giỏ
        public void RemoveLine(int productId)
        {
            var cart = GetCart();
            cart.RemoveLine(productId);
            SaveCart(cart);
        }

        // Xóa sạch giỏ hàng
        public void ClearCart()
        {
            if (session != null)
            {
                session[CartSessionKey] = null;
            }
        }

        // Hàm hỗ trợ cập nhật lại Session
        private void SaveCart(Cart cart)
        {
            if (session != null)
            {
                session[CartSessionKey] = cart;
            }
        }
    }
}