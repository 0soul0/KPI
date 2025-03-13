using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using ReviewWebsite.Models.Base;

namespace ReviewWebsite.Models.Db
{
    [Table("EvaluationList")] // 對應資料表名稱
    public class EvaluationList : BaseModel
    {

        [Key]
        [StringLength(15)] // 對應 NVARCHAR(15)
        public string EvaluationListId { get; set; } // 主鍵

        [StringLength(15)]
        public string EvaluationId { get; set; }

        [DisplayName("評鑑表單名")]
        public string FromName { get; set; }

        [DisplayName("年份")]
        public int Year { get; set; }

        [DisplayName("單位與中心")]
        public string Units { get; set; }

    }
}
