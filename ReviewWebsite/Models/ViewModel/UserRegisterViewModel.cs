using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ReviewWebsite.Models
{
  
    public class UserRegisterViewModel
    {
        [DisplayName("姓名")]
        [Required(ErrorMessage = "請輸入姓名")]
        [StringLength(30, ErrorMessage = "姓名不能超過30個字符")] 
        public string? Name { get; set; }

        [DisplayName("電子信箱")]
        [Required(ErrorMessage = "請輸入電子信箱")]
        [StringLength(30, ErrorMessage = "電子信箱不能超過30個字符")]
        [EmailAddress(ErrorMessage = "電子郵件格式無效")]
        public required string Email { get; set; }

        [DisplayName("密碼")]
        [Required(ErrorMessage = "請輸入密碼")]
        [StringLength(20, ErrorMessage = "密碼不能超過30個字符")]
        public required string Password { get; set; }

        [DisplayName("確定密碼")]
        [Required(ErrorMessage = "請輸入確定密碼")]
        [StringLength(20, ErrorMessage = "確定密碼不能超過30個字符")]
        public required string CheckPassword { get; set; }

        [DisplayName("單位")]
        [Required(ErrorMessage = "請選擇單位")]
        [StringLength(15, ErrorMessage = "單位不能超過15個字符")]
        public required string Unit { get; set; }

    }
}
