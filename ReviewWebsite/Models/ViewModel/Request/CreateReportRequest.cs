using Microsoft.AspNetCore.Mvc;
using ReviewWebsite.Models.Db;
using System.ComponentModel;

namespace ReviewWebsite.Models.ViewModel.Request
{
    public class CreateReportRequest
    {

        [FromBody]
        public ICollection<EvaluationList> SelectedEvaluations { get; set; }
    }
}
