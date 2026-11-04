using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _25dh110017_Mystore.Models.PartialClasses
{
    [MetadataType(typeof(OrderMetadata))]
    public partial class Order
    {
    }
    public class OrderMetadata
    {
        [Display(Name = "Mã đơn hàng")]
        public int OrderID { get; set; }

        [Display(Name = "Mã khách hàng")]
        public int CustomerID { get; set; }

        [Display(Name = "Ngày đặt hàng")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime OrderDate { get; set; }

        [Display(Name = "Tổng tiền")]
        [DisplayFormat(DataFormatString = "{0:N0} VNĐ")]
        public decimal TotalAmount { get; set; }

        [Display(Name = "Trạng thái thanh toán")]
        public string PaymentStatus { get; set; }

        [Display(Name = "Phương thức thanh toán")]
        [Required(ErrorMessage = "Vui lòng chọn {0}.")]
        public string PaymentMethod { get; set; }

        [Display(Name = "Phương thức giao hàng")]
        [Required(ErrorMessage = "Vui lòng chọn {0}.")]
        public string ShippingMethod { get; set; }

        [Display(Name = "Địa chỉ nhận hàng")]
        [Required(ErrorMessage = "{0} không được để trống.")]
        [StringLength(255, ErrorMessage = "{0} quá dài.")]
        public string ShippingAddress { get; set; }
    }
}