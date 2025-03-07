using ReviewWebsite.Models.Db;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using ReviewWebsite.Models.ViewModel.Request;

namespace ReviewWebsite.Models.ViewModel
{
    public class EvaluationCreateOrEditViewModel(): CreateFromRequest
    {

        public List<Unit> Units { get; set; }

        public List<FormList> FormList { get; set; }

    }
}
