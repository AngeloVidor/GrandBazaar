using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Buyers.Domain.Domain;
using Microsoft.EntityFrameworkCore;

namespace Buyers.DAL.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Buyer> Buyers { get; set; }
    }
}