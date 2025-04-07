using ReviewWebsite.Models.Db;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using ReviewWebsite.Models.Base;

namespace ReviewWebsite.Models.ViewModel
{
    public class EvaluationViewModel : BaseModel
    {

        public string EvaluationId { get; set; }

        public string Data { get; set; } 

        public string FromName { get; set; }

        public int Year { get; set; }

        public string Units { get; set; }

        public String UpdateTime { get; set; }
    }
}
