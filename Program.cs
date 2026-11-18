
using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;

class Student
{
    public int RollNo;
    public string Name;
    public DateTime DOB;
    public double Marks;

    public Student(int rollNo, string name, DateTime dob, double marks)
    {
        RollNo = rollNo;
        Name = name;
        DOB = dob;
        Marks = marks;
    }
}

class Program
{
    static List<Student> students = new List<Student>();
    static string filePath = "students.txt";

    static void Main()
    {
        LoadRecords();

        while (true)
        {
            Console.WriteLine("\n===== STUDENT RECORD MANAGER =====");
            Console.WriteLine("1. Add Student");
            Console.WriteLine("2. View Students");
            Console.WriteLine("3. Search Student");
            Console.WriteLine("4. Delete Student");
            Console.WriteLine("5. Save Records");
            Console.WriteLine("6. Exit");

            Console.Write("Enter your choice: ");

            if (!int.TryParse(Console.ReadLine() ?? "", out int choice))
            {
                Console.WriteLine("Invalid choice!");
                continue;
            }

            switch (choice)
            {
                case 1:
                    AddStudent();
                    break;

                case 2:
                    ViewStudents();
                    break;

                case 3:
                    SearchStudent();
                    break;

                case 4:
                    DeleteStudent();
                    break;

                case 5:
                    SaveRecords();
                    break;

                case 6:
                    SaveRecords();
                    Console.WriteLine("Thank you!");
                    return;

                default:
                    Console.WriteLine("Invalid choice!");
                    break;
            }
        }
    }

    static void AddStudent()
    {
        Console.Write("Enter Roll Number: ");

        if (!int.TryParse(Console.ReadLine() ?? "", out int roll))
        {
            Console.WriteLine("Invalid Roll Number!");
            return;
        }

        Console.Write("Enter Student Name: ");
        string name = Console.ReadLine() ?? "";

        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Name cannot be empty!");
            return;
        }

        Console.Write("Enter Date of Birth (yyyy-mm-dd): ");

        if (!DateTime.TryParseExact(
            Console.ReadLine() ?? "",
            "yyyy-MM-dd",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out DateTime dob))
        {
            Console.WriteLine("Invalid Date! Use yyyy-mm-dd.");
            return;
        }

        Console.Write("Enter Marks: ");

        if (!double.TryParse(Console.ReadLine() ?? "", out double marks))
        {
            Console.WriteLine("Invalid Marks!");
            return;
        }

        students.Add(new Student(roll, name, dob, marks));

        Console.WriteLine("Student added successfully!");
    }

    static void ViewStudents()
    {
        if (students.Count == 0)
        {
            Console.WriteLine("No student records found.");
            return;
        }

        foreach (Student s in students)
        {
            Console.WriteLine("\nRoll No: " + s.RollNo);
            Console.WriteLine("Name: " + s.Name);
            Console.WriteLine("DOB: " + s.DOB.ToShortDateString());
            Console.WriteLine("Marks: " + s.Marks);
            Console.WriteLine("Age: " + CalculateAge(s.DOB));
        }
    }

    static void SearchStudent()
    {
        Console.Write("Enter student name to search: ");

        string searchName = (Console.ReadLine() ?? "").Trim().ToUpper();

        if (searchName == "")
        {
            Console.WriteLine("Enter a name to search!");
            return;
        }

        Student? found = students.Find(
      s => s.Name.Trim().ToUpper().Contains(searchName)
  );


        if (found != null)
        {
            Console.WriteLine("\nStudent Found!");
            Console.WriteLine("Roll No: " + found.RollNo);
            Console.WriteLine("Name: " + found.Name);
            Console.WriteLine("Marks: " + found.Marks);
        }
        else
        {
            Console.WriteLine("Student not found.");
        }
    }

    static void DeleteStudent()
    {
        Console.Write("Enter Roll Number to delete: ");

        if (!int.TryParse(Console.ReadLine() ?? "", out int roll))
        {
            Console.WriteLine("Invalid Roll Number!");
            return;
        }

        Student? found = students.Find(s => s.RollNo == roll);

        if (found != null)
        {
            students.Remove(found);
            Console.WriteLine("Student deleted successfully!");
            SaveRecords();
        }
        else
        {
            Console.WriteLine("Student not found.");
        }
    }

    static int CalculateAge(DateTime dob)
    {
        int age = DateTime.Today.Year - dob.Year;

        if (dob.Date > DateTime.Today.AddYears(-age))
        {
            age--;
        }

        return age;
    }

    static void SaveRecords()
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            foreach (Student s in students)
            {
                writer.WriteLine(
                    $"{s.RollNo}|{s.Name}|{s.DOB:yyyy-MM-dd}|{s.Marks}"
                );
            }
        }

        Console.WriteLine("Records saved successfully!");
    }

    static void LoadRecords()
    {
        if (!File.Exists(filePath))
            return;

        foreach (string line in File.ReadAllLines(filePath))
        {
            string[] data = line.Split('|');

            if (data.Length == 4 &&
                int.TryParse(data[0], out int roll) &&
                DateTime.TryParseExact(
                    data[2],
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime dob) &&
                double.TryParse(data[3], out double marks))
            {
                students.Add(new Student(
                    roll,
                    data[1],
                    dob,
                    marks
                ));
            }
        }
    }
}
