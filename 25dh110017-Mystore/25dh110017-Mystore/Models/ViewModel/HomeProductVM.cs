using System.Collections.Generic;
using PagedList;
using _25dh110017_Mystore.Models; // Khai báo dùng Model Product

namespace _25dh110017_Mystore.Models.ViewModel
{
    public class HomeProductVM
    {
        public string SearchTerm { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 6;

        public List<Product> FeaturedProducts { get; set; } = new List<Product>();
        public IPagedList<Product> NewProducts { get; set; }
    }
}