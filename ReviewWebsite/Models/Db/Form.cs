using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using ReviewWebsite.Models.Base;

namespace ReviewWebsite.Models.Db
{
    [Table("Form")]
    public class Form : BaseModel
    {
        [Key]
        [StringLength(15)]
        public String FormId { get; set; }

        public String Data { get; set; }

    }
}
