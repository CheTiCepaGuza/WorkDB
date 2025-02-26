using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Worker
    {

        [Key]
        public int Id {  get; set; }

        public string Name { get; set; }

        public string Position { get; set; }

        public int Salary { get; set; }

        public Department Department { get; set; }

        public Worker(string Name, string Position, int Salaty, Department Department)
        {

            this.Name = Name;
            this.Position = Position;
            this.Salary = Salaty;
            this.Department = Department;

        }

    }
}
