using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ConsoleApp1
{
    public class OfficeDbContext : DbContext 
    {

        public OfficeDbContext() : base(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Student\source\repos\Solution4\myDb.mdf;Integrated Security=True;Connect Timeout=30") { }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Worker> Workers { get; set; }


    }
}
