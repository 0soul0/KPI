using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ReviewWebsite.Models.Db
{
    [Table("Evaluations")] // 對應資料表名稱
    public class Evaluations
    {

        [Key]
        [StringLength(15)] // 對應 NVARCHAR(15)
        public String EvaluationId { get; set; } // 主鍵
  
        [StringLength(15)]
        public String FormHeadId { get; set; }

        [DisplayName("評分")]
        public int Rating { get; set; }

        [DisplayName("更新時間")]
        [DataType(DataType.DateTime)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdateTime { get; set; }

        [DisplayName("建立時間")]
        [DataType(DataType.DateTime)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreateTime { get; set; }

        [NotMapped]
        [DisplayName("表單名稱")]
        [StringLength(15)]
        public String Name { get; set; }

        [NotMapped]
        [DisplayName("年份")]
        [Range(maximum: 200, minimum: 100)]
        public int Year { get; set; }
    }
}
