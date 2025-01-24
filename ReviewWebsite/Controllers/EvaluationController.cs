using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReviewWebsite.Data;
using ReviewWebsite.Models.Db;

namespace ReviewWebsite.Controllers
{
    public class EvaluationController : Controller
    {
        private readonly AppDbContext _context;

        public EvaluationController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.User.ToListAsync());
        }

       
    }
}
