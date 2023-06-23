using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LoginApp.Models;

namespace LoginApp.Data
{
    public class LoginAppContext : DbContext
    {
        public LoginAppContext (DbContextOptions<LoginAppContext> options)
            : base(options)
        {
        }

        public DbSet<LoginApp.Models.Customer> Customer { get; set; } = default!;
    }
}
