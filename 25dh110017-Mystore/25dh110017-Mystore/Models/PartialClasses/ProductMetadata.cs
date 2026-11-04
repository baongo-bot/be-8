using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _25dh110017_Mystore.Models.PartialClasses
{
    [MetadataType(typeof(ProductMetadata))]
    public partial class Product
    {
    }
    public class ProductMetadata
    {
        [Display(Name = "Mã sản phẩm")]
        public int ProductID { get; set; }

        [Display(Name = "Tên sản phẩm")]
        [Required(ErrorMessage = "{0} không được để trống.")]
        [StringLength(150, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
        public string ProductName { get; set; }

        [Display(Name = "Mô tả sản phẩm")]
        [DataType(DataType.MultilineText)]
        public string ProductDescription { get; set; }

        [Display(Name = "Giá bán")]
        [Required(ErrorMessage = "{0} không được để trống.")]
        [Range(0, double.MaxValue, ErrorMessage = "{0} phải lớn hơn hoặc bằng 0.")]
        [DisplayFormat(DataFormatString = "{0:N0} VNĐ")]
        public decimal ProductPrice { get; set; }

        [Display(Name = "Hình ảnh")]
        public string ProductImage { get; set; }

        [Display(Name = "Danh mục sản phẩm")]
        [Required(ErrorMessage = "Vui lòng chọn {0}.")]
        public int CategoryID { get; set; }
    }
}