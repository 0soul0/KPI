using Microsoft.AspNetCore.Mvc;
using ReviewWebsite.Models.Db;
using System.ComponentModel;

namespace ReviewWebsite.Models.ViewModel.Request
{
    public class CreateFromRequest
    {

        [DisplayName("表單")]
        [FromBody]
        public String SelectedFromId { get; set; }

        [DisplayName("單位或中心")]
        [FromBody]
        public ICollection<Unit> SelectedUnits { get; set; }
    }
}
