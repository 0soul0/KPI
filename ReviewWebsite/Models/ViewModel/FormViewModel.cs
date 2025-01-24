using Microsoft.AspNetCore.Mvc;
using ReviewWebsite.Models.Db;

namespace ReviewWebsite.Models.ViewModel
{
    public class FormViewModel
    {
        
        public FormViewModel() {
           
        }
        [FromBody]
        public FormHead FormHead { get; set; }
        [FromBody]
        public List<FormContent>? FormContentList { get; set; }
    }
}
