using System;
using System.Collections.Generic;
using System.IO;

namespace SICFSA
{
    class Student
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }

        public override string ToString()
        {
            return $"{FirstName} {LastName}, Age: {Age}";
        }
    }

    class Program
    {
        static List<Student> students = new List<Student>();
        static string filePath = "students.txt";

        static void Main()
        {
            LoadStudentsFromFile();

            while (true)
            {
                Console.WriteLine("1 - Add Student");
                Console.WriteLine("2 - Delete Student");
                Console.WriteLine("3 - List Students");
                Console.WriteLine("4 - Exit");
                Console.Write("Select an option (1/2/3/4): ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddStudent();
                        SaveStudentsToFile();
                        break;
                    case "2":
                        DeleteStudent();
                        SaveStudentsToFile();
                        break;
                    case "3":
                        SaveStudentsToFile();
                        ListStudents();
                        break;
                    case "4":
                        SaveStudentsToFile();
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }

                Console.WriteLine();
            }
        }

        static void AddStudent()
        {
            Console.Write("First Name: ");
            string firstName = Console.ReadLine();

            Console.Write("Last Name: ");
            string lastName = Console.ReadLine();

            Console.Write("Age: ");
            int age = int.Parse(Console.ReadLine());

            Student student = new Student
            {
                FirstName = firstName,
                LastName = lastName,
                Age = age
            };

            students.Add(student);
            Console.WriteLine("Student added successfully.");
        }

        static void DeleteStudent()
        {
            ListStudents();

            Console.Write("Enter the name of the student to delete: ");
            string nameToDelete = Console.ReadLine();

            Student studentToDelete = students.Find(s => s.FirstName.Equals(nameToDelete, StringComparison.OrdinalIgnoreCase));

            if (studentToDelete != null)
            {
                students.Remove(studentToDelete);
                Console.WriteLine("Student deleted successfully.");

                ListStudents();
            }
            else
            {
                Console.WriteLine("Student with the specified name not found. Please try again.");
            }
        }

        static void ListStudents()
        {
            if (students.Count == 0)
            {
                Console.WriteLine("No registered students.");
                return;
            }
            else
            {
                Console.WriteLine("Registered Students:");
                for (int i = 0; i < students.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {students[i]}");
                }
            }
        }

        static void LoadStudentsFromFile()
        {
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    string[] parts = line.Split(',');
                    if (parts.Length == 3)
                    {
                        string firstName = parts[0].Trim();
                        string lastName = parts[1].Trim();
                        int age = int.Parse(parts[2].Trim());

                        Student student = new Student
                        {
                            FirstName = firstName,
                            LastName = lastName,
                            Age = age
                        };

                        students.Add(student);
                    }
                }
            }
        }

        static void SaveStudentsToFile()
        {
            int n = students.Count;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (string.Compare(students[j].LastName, students[j + 1].LastName) > 0)
                    {
                        Student temp = students[j];
                        students[j] = students[j + 1];
                        students[j + 1] = temp;
                    }
                }
            }
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (Student student in students)
                {
                    writer.WriteLine($"{student.FirstName}, {student.LastName}, {student.Age}");
                }
            }
        }
    }
}
