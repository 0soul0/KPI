using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ReviewWebsite.Models.Db
{
    [Table("User")] // 對應資料表名稱
    public class User
    {
        [Key]
        [StringLength(15)] // 對應 NVARCHAR(15)
        public required string UserId { get; set; } // 主鍵

        [DisplayName("姓名")]
        [StringLength(30)] // 對應 NVARCHAR(30)
        public string? Name { get; set; }

        [DisplayName("電子信箱")]
        [StringLength(30)] // 對應 NVARCHAR(30)
        public string? Email { get; set; }

        [StringLength(20)] // 對應 NVARCHAR(20)
        public string? Password { get; set; }

        [StringLength(10)] // 對應 NVARCHAR(10)
        public string? Identification { get; set; }

        [DisplayName("生日")]
        [StringLength(10)] // 對應 NVARCHAR(10)
        public string? Birthday { get; set; }
        [NotMapped]
        public string? FormattedBirthday
        {
            get
            {
                if (DateTime.TryParse(Birthday, out DateTime date))
                {
                    return date.ToString("yyyy-MM-dd");
                }
                return Birthday ?? ""; // 如果是 null，則回傳空字串
            }
        }

        [StringLength(50)] // 對應 NVARCHAR(50)
        public string? Location { get; set; }

        [DisplayName("手機")]
        [StringLength(10)] // 對應 NVARCHAR(10)
        public string? Telephone { get; set; }

        [DisplayName("權限")]
        [StringLength(5)] // 對應 NVARCHAR(5)
        public string? AccessRight { get; set; }


    }
}
