﻿using Microsoft.AspNetCore.Mvc;
using ReviewWebsite.Models.Db;
using System.ComponentModel.DataAnnotations;

namespace ReviewWebsite.Models.ViewModel
{
    public class FormViewModel
    {
        
        public string Data { get; set; }

        public String FormId { get; set; }

        public String Name { get; set; }

        public int Year { get; set; }
    }
}
