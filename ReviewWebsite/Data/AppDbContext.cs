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
        public DbSet<FormHead> FormHead { get; set; } = default!;
        public DbSet<FormContent> FormContent { get; set; } = default!;
    }
}
