using EjadaEFProject.Data;
using EjadaEFProject.Models;
using EjadaEFProject.Sevices;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections.Generic;

namespace EjadaEFProject
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            while (true)
            {
                UserServices.DisplayMenu();
                char choice;
                string type;
                do
                {
                    choice = UserServices.AcceptUserChoice();
                } while (!UserServices.HandleUserChoice(choice, out type));

                object myService = null;
                switch (type)
                {
                    case "Department":
                        myService = new DepartmentServices();
                        UserServices.DisplayOptionsMenu<Department>();
                        break;
                    case "Student":
                        myService = new StudentServices();
                        UserServices.DisplayOptionsMenu<Student>();
                        break;
                    case "Course":
                        myService = new CourseServices();
                        UserServices.DisplayOptionsMenu<Course>();
                        break;
                    case "Enrollment":
                        myService = new StdCrsServices();
                        UserServices.DisplayOptionsMenu<StdCr>();
                        break;
                    default:
                        Console.WriteLine($"Unknown type: {type}");
                        break;
                }

                // Choose operation based on the selected service
                char operationChoice;
                do
                {
                    operationChoice = UserServices.AcceptUserChoice();
                } while (!UserServices.HandleOperationChoice(operationChoice));

                await UserServices.HandleServiceAsync(myService, operationChoice);

            }
        }
    }
}
