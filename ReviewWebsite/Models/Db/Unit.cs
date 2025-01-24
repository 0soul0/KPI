using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReviewWebsite.Models.Db
{
    [Table("Unit")]
    public class Unit
    {

        [Key]
        [StringLength(15)] // 對應 NVARCHAR(15)
        public required String UnitId { get; set; } // 主鍵

        [DisplayName("單位")]
        [StringLength(30)] // 對應 NVARCHAR(30)
        public required String Name { get; set; }
    }
}
