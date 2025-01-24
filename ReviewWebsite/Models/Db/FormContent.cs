using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.IdentityModel.Tokens;

namespace ReviewWebsite.Models.Db
{
    public class FormContent
    {   

        public FormContent() { }

        [Key]
        [StringLength(15)]
        public required String FormContentId { get; set; }

        [StringLength(15)]
        public required String FormHeadId { get; set; }
        public required int RowIndex { get; set; }

        [StringLength(15)]
        public String? CategoryName { get; set; }

        [DisplayName("查核指標")]
        public String? CheckIndicators { get; set; }

        [DisplayName("查核標準")]
        public String? CheckStandards { get; set; }

        [DisplayName("應檢附資料")]
        public String? AttachedInfo { get; set; }

        [DisplayName("評核基準")]
        public String? Assessment { get; set; }

        [DisplayName("KPI連動")]
        public String? KPILinkage { get; set; }

        private string _type=""; 
        [NotMapped]
        public string Type
        {
            get
            {
                if (!_type.IsNullOrEmpty()) { 
                    return _type;
                }
                return CategoryName.IsNullOrEmpty() ? "item" : "title";
            }
            set
            {
                _type = value; 
            }
        }

    }
}
