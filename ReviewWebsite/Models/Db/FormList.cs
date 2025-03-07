using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using ReviewWebsite.Models.Base;

namespace ReviewWebsite.Models.Db
{
    [Table("FormList")]
    public class FormList: BaseModel
    {

        [Key]
        [StringLength(15)]
        public String FormListId { get; set; }

        [StringLength(15)]
        public String FormId { get; set; }

        [DisplayName("表單名稱")]
        [StringLength(15)]
        public String Name { get; set; }

        [DisplayName("年份")]
        public int Year { get; set; }

    }
}
