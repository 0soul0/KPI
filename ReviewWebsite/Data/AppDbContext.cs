using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using ReviewWebsite.Models.Db;

namespace ReviewWebsite.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext (DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> User { get; set; } = default!;
        public DbSet<Unit> Unit { get; set; } = default!;
        public DbSet<FormList> FormList { get; set; } = default!;
        public DbSet<Form> Form { get; set; } = default!;
        public DbSet<EvaluationList> EvaluationList { get; set; } = default!;
        public DbSet<Evaluation> Evaluation { get; set; } = default!;
    }
}
