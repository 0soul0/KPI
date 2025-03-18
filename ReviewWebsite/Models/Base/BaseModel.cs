using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ReviewWebsite.Models.Base
{
    public class BaseModel
    {

        [DisplayName("更新時間")]
        //[DataType(DataType.DateTime)]
        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdateTime { get; set; } = DateTime.Now; // DateTime.UtcNow;

        [DisplayName("建立時間")]
        [DataType(DataType.DateTime)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreateTime { get; set; }

        [NotMapped]
        public string FormattedUpdateTime
        {
            get
            {
                return UpdateTime.ToString("yyyy-MM-dd");
            }
        }

        [NotMapped]
        public string FormattedCreateTime
        {
            get
            {
                return UpdateTime.ToString("yyyy-MM-dd");
            }
        }
    }
}
