using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Auth.DAL.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
    }
}