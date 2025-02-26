using ConsoleApp1;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;

public class Controller
{
    public Department GetDepart(int id)
    {
        using (OfficeDbContext db = new OfficeDbContext())
        {
            Department department = db.Departments.FirstOrDefault(d => d.DepartmentID == id);

            if (department == null)
            {
                Console.WriteLine("Department not found.");
                return null;
            }

            return department;
        }
    }

    public void AddDepartment(Department department)
    {
        using (OfficeDbContext db = new OfficeDbContext())
        {
            db.Departments.Add(department);
            db.SaveChanges();
        }

    }

    public void RemoveDepartment(int id)
    {
        Department deps = GetDepart(id);


        if (deps == null)
        {
            Console.WriteLine("Department cannot be removed, it does not exist.");
            return;
        }

        using (OfficeDbContext db = new OfficeDbContext())
        {
            db.Departments.Attach(deps);

            db.Departments.Remove(deps);
            db.SaveChanges();
            Console.WriteLine("Department removed successfully.");
        }

        ResetDepartmentsIds();
    }

    public void ResetDepartmentsIds()
    {
        using (OfficeDbContext db = new OfficeDbContext())
        {
            // Изтриване на всички департаменти
            var departments = db.Departments.ToList();
            db.Departments.RemoveRange(departments);
            db.SaveChanges();

            // Ресетване на автоинкрементното поле
            db.Database.ExecuteSqlCommand("DBCC CHECKIDENT ('Departments', RESEED, 0)");

            db.Departments.AddRange(departments);
            db.SaveChanges();
        }
    }

    public void AddWork(Worker worker)
    {

        using (OfficeDbContext db = new OfficeDbContext())
        {
            db.Workers.Add(worker);
            db.SaveChanges();
        }

    }

}

public class View
{
    public Controller Controller = new Controller();

    public void AddDepartmentView()
    {
        Console.Write("Enter Worker(Ivan/4000): ");
        string[] name = Console.ReadLine().Split('/').ToArray();

        if (name.Length > 1)
        {
            Department department = new Department(name[0], int.Parse(name[1]));
            Controller.AddDepartment(department);
            Console.WriteLine($"{department.Name} was added in db");
        }
        else
        {
            Console.WriteLine("Error: Invalid input.");
        }
        Console.WriteLine();
    }

    public void RemoveDepartentView()
    {
        Console.Write("Enter DepartmentId to be removed: ");
        int Id = int.Parse(Console.ReadLine());
        Controller.RemoveDepartment(Id);
    }

    public void DisplayDepartments()
    {
        Console.Clear();
        using (OfficeDbContext db = new OfficeDbContext())
        {
            foreach(var item in db.Departments)
            {

                Console.WriteLine(item);

            }
        }

    }

    public void AddWorkView()
    {
        Console.WriteLine("----Add-Worker---");
        Console.WriteLine("Enter W Name: ");
        string name = Console.ReadLine();

        Console.WriteLine("Enter W Positoin: ");
        string position = Console.ReadLine();

        Console.WriteLine("Enter W Salary: ");
        int salary = int.Parse(Console.ReadLine());

        Console.Clear();

        DisplayDepartments();

        Console.WriteLine();
        Console.WriteLine("Enter Department code: ");
        int deptCode = int.Parse(Console.ReadLine());
        Worker worker = new Worker(name, position, salary, Controller.GetDepart(deptCode));
        if(worker != null)
        {

            Controller.AddWork(worker);

        }
        else
        {
            Console.WriteLine("Error");
        }

    }

    public void Menu()
    {

        while (true)
        {
            Console.WriteLine("-----Menu----");
            Console.WriteLine("[A]Add-D");
            Console.WriteLine("[R]Remove-D");
            Console.WriteLine("[D]Display-D");
            Console.WriteLine("[Shift + A]Add-W");
            ConsoleKeyInfo key = Console.ReadKey(intercept: true);
            if (key.Key == ConsoleKey.A && key.Modifiers == ConsoleModifiers.Shift)
            {

                Console.Clear();
                AddWorkView();

            }
            else if (key.Key == ConsoleKey.A)
            {
                Console.Clear();
                AddDepartmentView();
            }
            else if (key.Key == ConsoleKey.R)
            {
                Console.Clear();
                RemoveDepartentView();
            }
            else if (key.Key == ConsoleKey.D)
            {
                Console.Clear();
                DisplayDepartments();
                Console.WriteLine();
            }
            else
            {
                Console.Clear();
            }



        }

    }
}

public class Department
{
    [Key]
    public int DepartmentID { get; set; }

    public string Name { get; set; }

    public int Capacity { get; set; }

    public List<Worker> Workers { get; set; }

    public Department() { }

    public Department(string Name, int Capacity)
    {
        this.Name = Name;
        this.Capacity = Capacity;
        this.Workers = new List<Worker>();
    }

    public override string ToString()
    {
        return $"|{DepartmentID}|Name: {Name}|Capacity: {Capacity}|";
    }
}
