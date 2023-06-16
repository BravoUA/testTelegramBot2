using ConsoleApp1.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
     class dbConnect: DbContext
    {
        public DbContext DbContext { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<History> History{ get; set; }
        public DbSet<Products> Products { get; set; }

        public dbConnect() { }

        public dbConnect(DbContextOptions<dbConnect> options): base(options) {  }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlite("DataSource=NewBD.db;");
        }
    }
}
