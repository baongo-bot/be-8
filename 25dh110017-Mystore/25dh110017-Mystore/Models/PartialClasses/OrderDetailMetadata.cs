using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _25dh110017_Mystore.Models.PartialClasses
{
    [MetadataType(typeof(OrderDetailMetadata))]
    public partial class OrderDetail
    {
    }
    public class OrderDetailMetadata
    {
        [Display(Name = "Mã chi tiết")]
        public int ID { get; set; }

        [Display(Name = "Mã đơn hàng")]
        public int OrderID { get; set; }

        [Display(Name = "Mã sản phẩm")]
        public int ProductID { get; set; }

        [Display(Name = "Số lượng")]
        [Required(ErrorMessage = "{0} không được để trống.")]
        [Range(1, 1000, ErrorMessage = "{0} phải nằm trong khoảng từ {1} đến {2}.")]
        public int Quantity { get; set; }

        [Display(Name = "Đơn giá")]
        [DisplayFormat(DataFormatString = "{0:N0} VNĐ")]
        public decimal UnitPrice { get; set; }

        [Display(Name = "Thành tiền")]
        [DisplayFormat(DataFormatString = "{0:N0} VNĐ")]
        public decimal? TotalPrice { get; set; }
    }
}