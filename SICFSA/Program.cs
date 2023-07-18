using System;
using System.IO;

namespace SICFSA

{
    class Program
    {
        static void Main(string[] args)
        {
        SaveNewUser();
        }
            
        static void SaveNewUser()
        {
            newUser register = new newUser();

            Console.WriteLine("Enter your name: ");

            register.firstName = Console.ReadLine();


            Console.WriteLine("Enter your surname:");
            register.lastName = Console.ReadLine();

            Console.WriteLine("Enter your age:");
            register.age = Convert.ToInt32(Console.ReadLine());


            File.AppendAllText("save.txt", $"{register.firstName}-{register.lastName}-{register.age} {Environment.NewLine}");

            string readText = File.ReadAllText("Save.txt");
        }
    }
        class newUser
        { 
            public string firstName = "";
            public string lastName = "";
            public int age = 0;
        }
}
