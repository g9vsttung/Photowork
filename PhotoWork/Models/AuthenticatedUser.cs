//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PhotoWork.Models
{
    using System;
  
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public partial class AuthenticatedUser
    {
        [DisplayName("Email")]
        [Required(ErrorMessage = "Vui lòng nhập Email!")]
        [StringLength(50)]
        [RegularExpression(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-‌​]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$", ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        [DisplayName("Mật khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [MaxLength(15, ErrorMessage = "Tối đa 15 kí tự"), MinLength(3, ErrorMessage = "Tối thiểu 3 kí tự")]
        public string passwords { get; set; }
        public string Role { get; set; }
        [DisplayName("Số điện thoại")]
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]

        [RegularExpression(@"^(09|03|07|08|05)+([0-9]{8})\b", ErrorMessage = "Số điện thoại không hợp lệ")]
        public string phoneNumber { get; set; }
        [DisplayName("Họ và tên")]
        [Required(ErrorMessage = "Vui lòng nhập họ và tên")]
        [StringLength(50)]
        public string FullName { get; set; }
        [DisplayName("Nhập lại mật khẩu")]
        [Required(ErrorMessage = "Vui lòng xác nhận mật khẩu")]
        [MaxLength(15, ErrorMessage = "Tối đa 15 kí tự"), MinLength(3, ErrorMessage = "Tối thiểu 3 kí tự")]
        [Compare("passwords", ErrorMessage = "Xác nhận sai, vui lòng thử lại")]
        public string repasswords { get; set; }
        public bool isActive { get; set; }
        public string Avatar { get; set; }
        public Nullable<bool> isBanned { get; set; }
        public virtual Admin Admin { get; set; }
        public virtual Client Client { get; set; }
        public virtual Photographer Photographer { get; set; }
    }
}
