using Microsoft.AspNetCore.Mvc;
using ReviewWebsite.Models.Db;
using System.ComponentModel;

namespace ReviewWebsite.Models.ViewModel.Request
{
    public class ReportRequest
    {
        public int Page { get; set; }
    }
}
