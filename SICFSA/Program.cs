using Microsoft.VisualBasic.FileIO;
using System;
using System.ComponentModel;
using System.IO;

namespace SICFSA

{
    class Program
    {
        static void Main(string[] args)
        {

            newUser register = new newUser();

            Console.WriteLine("What would you like to do? ");
            Console.WriteLine("Add student, Delete student or display students");
            Console.WriteLine("----------------------------------------------");
            string x = Console.ReadLine().ToLower();

            if (x == "add student" || x == "add")
            {
                Console.WriteLine("Enter your name: ");
                register.firstName = Console.ReadLine();

                Console.WriteLine("Enter your surname:");
                register.lastName = Console.ReadLine();

                Console.WriteLine("Enter your age: ");
                register.age = Convert.ToInt32(Console.ReadLine());

                SaveNewUser(register);
            }
            else if (x == "delete student" || x == "delete")
            {
                Console.WriteLine("----------------------------");
                DisplaySavedUsers();
                Console.WriteLine("----------------------------");
                Console.WriteLine("Enter the index of the student you want to delete:");
                int indexToDelete = Convert.ToInt32(Console.ReadLine());
                DeleteUser(indexToDelete);
            }
            else if (x == "display student" || x=="display")
            {
                DisplaySavedUsers();
            }
            else
            {
                Console.WriteLine("You cant do that");
            }
        }
            
        static void SaveNewUser(newUser User)
        {

            File.AppendAllText("save.txt", $"{User.firstName}-{User.lastName}-{User.age} {Environment.NewLine}");
            string readText = File.ReadAllText("Save.txt");
        }
        static void DisplaySavedUsers()
        {
            string readText = File.ReadAllText("save.txt");
            Console.WriteLine(Environment.NewLine +"Saved Students:" + Environment.NewLine);
            Console.WriteLine(readText);
        }
        static void DeleteUser(int index)
        {
            string[] lines = File.ReadAllLines("save.txt");
            if (index >= 1 && index <= lines.Length)
            {
                lines[index - 1] = null;
                File.WriteAllLines("save.txt", lines);
                Console.WriteLine("Student deleted successfully.");
            }
            else
            {
                Console.WriteLine("Invalid index no student deleted!");
            }
        }
    }
        class newUser
        { 
            public string firstName = "";
            public string lastName = "";
            public int age = 0;
        }
}
