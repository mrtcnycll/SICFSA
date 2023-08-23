using System;
using System.Collections.Generic;
using System.IO;
using System.Data.SqlClient;
using System.ComponentModel.Design;

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
    class DatabaseManager
    {
        private string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\mrtcn\\source\\repos\\SICFSA\\SICFSA\\Database.mdf;Integrated Security=True";
        public string ConnectionString => connectionString;
        public void AddStudent(Student student)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Students (FirstName, LastName, Age) VALUES (@FirstName, @LastName, @Age)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", student.FirstName);
                    command.Parameters.AddWithValue("@LastName", student.LastName);
                    command.Parameters.AddWithValue("@Age", student.Age);
                    command.ExecuteNonQuery();
                }
            }
        }
        public List<Student> GetStudents()
        {
            List<Student> students = new List<Student>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT FirstName, LastName, Age FROM Students";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Student student = new Student
                            {
                                FirstName = reader.GetString(0),
                                LastName = reader.GetString(1),
                                Age = reader.GetInt32(2)
                            };
                            students.Add(student);
                        }
                    }
                }
            }
            return students;
        }
    }
    class Program
    {
        static List<Student> students = new List<Student>();
        static string filePath = "students.txt";

        static DatabaseManager dbManager = new DatabaseManager();
        static void Main()
        {
            LoadStudentsFromFile();
            bool db = true;

            while (true)
            {
                Console.WriteLine("1 - Add Student");
                Console.WriteLine("2 - Delete Student");
                Console.WriteLine("3 - List Students");
                Console.WriteLine("4 - Update Student");
                Console.WriteLine("5 - Database Mode (" + (db ? "ON" : "OFF") + ")");
                Console.WriteLine("6 - Exit");
                Console.Write("Select an option (1/2/3/4/5): ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        if (db)
                        {
                            AddStudentToDatabase();
                        }
                        else
                        {
                            AddStudent();
                            SaveStudentsToFile();
                        }
                        break;
                    case "2":
                        if (db)
                        {
                            DeleteStudentFromDatabase();
                        }
                        else
                        {
                            DeleteStudent();
                            SaveStudentsToFile();
                        }
                        break;
                    case "3":
                        if (db)
                        {
                            ListStudentsInDatabase();
                        }
                        else
                        {
                            SaveStudentsToFile();
                            ListStudents();
                        }
                        break;
                    case "4":
                        UpdateStudent();
                        SaveStudentsToFile();
                        break;
                    case "5":
                        db = !db;
                        Console.WriteLine("Database is now " + (db ? "ON" : "OFF"));

                        if (db)
                        {
                            Console.WriteLine("Switched to Database mode.");
                        }
                        else
                        {
                            Console.WriteLine("Switched to Text mode.");
                        }
                        break;
                    case "6":
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
        static void UpdateStudent()
        {
            ListStudents();

            Console.Write("Enter the last name of the student to update: ");
            string lastNameToUpdate = Console.ReadLine();

            Student studentToUpdate = students.Find(s => s.LastName.Equals(lastNameToUpdate, StringComparison.OrdinalIgnoreCase));

            if (studentToUpdate != null)
            {
                Console.WriteLine($"Updating student: {studentToUpdate}");

                Console.WriteLine("Select attribute to update:");
                Console.WriteLine("1 - First Name");
                Console.WriteLine("2 - Last Name");
                Console.WriteLine("3 - Age");
                Console.Write("Enter option (1/2/3): ");
                string updateChoice = Console.ReadLine();
                switch (updateChoice)
                {
                    case "1":
                        Console.Write("Enter the new First Name: ");
                        studentToUpdate.FirstName = Console.ReadLine();
                        break;
                    case "2":
                        Console.Write("Enter the new Last Name: ");
                        studentToUpdate.LastName = Console.ReadLine();
                        break;
                    case "3":
                        Console.Write("Enter the new Age: ");
                        studentToUpdate.Age = int.Parse(Console.ReadLine());
                        break;
                    default:
                        Console.WriteLine("Invalid choice. No attributes were updated.");
                        break;
                }
                Console.WriteLine("Student updated successfully.");
                SaveStudentsToFile();
                ListStudents();
            }
            else
            {
                Console.WriteLine("Student with the specified last name not found. Please try again.");
            }
        }
        static void AddStudentToDatabase()
        {
            Console.Write("First Name: ");
            string firstName = Console.ReadLine().ToLower();

            Console.Write("Last Name: ");
            string lastName = Console.ReadLine().ToLower();

            Console.Write("Age: ");
            int age = int.Parse(Console.ReadLine());

            Student student = new Student
            {
                FirstName = firstName,
                LastName = lastName,
                Age = age
            };

            dbManager.AddStudent(student);
            Console.WriteLine("Student added to the database successfully.");
        }
        static void ListStudentsInDatabase()
        {

            List<Student> studentsFromDatabase = dbManager.GetStudents();

            if (studentsFromDatabase.Count == 0)
            {
                Console.WriteLine("No registered students in the database.");
                return;
            }
            else
            {
                Console.WriteLine("Registered Students in the Database:");
                for (int i = 0; i < studentsFromDatabase.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {studentsFromDatabase[i]}");
                }
            }
        }
        static void DeleteStudentFromDatabase()
        {
            ListStudentsInDatabase();

            Console.Write("Enter the last name of the student to delete: ");
            string lastNameToDelete = Console.ReadLine().ToLower();

            // Öğrenciyi veritabanından silme işlemi
            using (SqlConnection connection = new SqlConnection(dbManager.ConnectionString))
            {
                connection.Open();
                string query = "DELETE FROM Students WHERE LastName = @LastName";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LastName", lastNameToDelete);
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Student deleted from the database successfully.");
                        ListStudentsInDatabase();
                    }
                    else
                    {
                        Console.WriteLine("Student with the specified last name not found in the database.");
                    }
                }
            }
        }
    }
}

