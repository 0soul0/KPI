using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ReviewWebsite.Models.ViewModel.Request
{
    public class UserRequest
    {
        public string UserId { get; set; } // 主鍵

        public string? FullName { get; set; }

        public string? Email { get; set; }

        public string? Identification { get; set; }

        public string? Birthday { get; set; }

        public string? Location { get; set; }

        public string? Telephone { get; set; }
        public string? AccessRight { get; set; }


    }
}
