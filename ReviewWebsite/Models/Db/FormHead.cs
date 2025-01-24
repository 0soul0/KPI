using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ReviewWebsite.Models.Db
{
    public class FormHead
    {   


        public FormHead(String FormHeadId) {
            this.FormHeadId = FormHeadId;
        }

        [Key]
        [StringLength(15)]
        public String FormHeadId { get; set; }

        [DisplayName("表單名稱")]
        [StringLength(15)]
        public String Name { get; set; }

        [DisplayName("年份")]
        [Range(maximum:200,minimum:100)]
        public int Year { get; set; }

        [DisplayName("更新時間")]
        [DataType(DataType.DateTime)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdateTime { get; set; }

        [DisplayName("建立時間")]
        [DataType(DataType.DateTime)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreateTime { get; set; }
    }
}
