using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _25dh110017_Mystore.Models.ViewModel
{
    public class ProductSearchVM
    {
        // 1. Tiêu chí tìm kiếm theo từ khóa (tên, mô tả)
        public string SearchTerm { get; set; }

        // 2. Tiêu chí tìm kiếm theo khoảng giá
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        // 3. Thứ tự sắp xếp (price_asc, price_desc, name_asc, name_desc...)
        public string SortOrder { get; set; }

        // 4. Các thuộc tính Phân trang (Pagination)
        public int PageNumber { get; set; } = 1; // Trang hiện tại (mặc định là 1)
        public int PageSize { get; set; } = 5;   // Số sản phẩm hiển thị trên 1 trang (mặc định là 5)
        public int TotalCount { get; set; }      // Tổng số sản phẩm tìm thấy

        // Tổng số trang (tự động tính dựa trên tổng số sản phẩm và kích thước trang)
        public int PageCount => (int)Math.Ceiling((double)TotalCount / (PageSize > 0 ? PageSize : 1));

        // 5. Danh sách sản phẩm kết quả
        public List<Product> Products { get; set; }
    }
}