using ReviewWebsite.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReviewWebsite.Models.Db
{
    [Table("Evaluation")] // 對應資料表名稱
    public class Evaluation : BaseModel
    {

        [Key]
        [StringLength(15)]
        public string EvaluationId { get; set; }

        public string Data { get; set; }

    }
}
