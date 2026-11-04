using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _25dh110017_Mystore.Models.PartialClasses
{
    [MetadataType(typeof(UserMetadata))]
    public partial class User
    {
    }
    public class UserMetadata
    {
        [Display(Name = "Tên đăng nhập")]
        [Required(ErrorMessage = "{0} không được để trống.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "{0} phải từ {2} đến {1} ký tự.")]
        public string Username { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "{0} không được để trống.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "{0} phải có ít nhất {2} ký tự.")]
        public string Password { get; set; }

        [Display(Name = "Quyền người dùng")]
        public string UserRole { get; set; }
    }
}