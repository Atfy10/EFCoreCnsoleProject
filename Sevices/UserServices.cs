using EjadaEFProject.Interfaces;
using EjadaEFProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EjadaEFProject.Sevices
{
    internal class UserServices
    {
        enum MenuOptions
        {
            Departments,
            Students,
            Courses,
            Enrollments
        }
        enum CRUDOps
        {
            Create,
            GetAll,
            DisplayById,
            Update,
            Remove
        }

        /*********************General Methods*************************/
        private static IEnumerable<string> DisplayMenuOptions()
        {
            foreach (var option in Enum.GetValues(typeof(MenuOptions)))
            {
                yield return option.ToString();
            }
        }
        public static void DisplayMenu()
        {
            Console.WriteLine("\n\nWelcome to the University Management System\n");
            Console.WriteLine("Please choose an option from the menu below:");
            foreach (var option in DisplayMenuOptions())
            {
                Console.WriteLine($"({char.ToLower(option[0])}) for {option}");
            }
        }
        public static char AcceptUserChoice()
        {
            Console.Write("(z -> for exit) choice: ");
            char choice = Console.ReadKey().KeyChar;
            Console.WriteLine();
            if (choice is 'z')
            {
                Console.WriteLine("Exiting the application. Goodbye!");
                Environment.Exit(0);
            }
            return choice;
        }
        public static bool HandleUserChoice(char choice, out string type)
        {
            switch (char.ToLower(choice))
            {
                case 'd':
                    type = "Department";
                    break;
                case 's':
                    type = "Student";
                    break;
                case 'c':
                    type = "Course";
                    break;
                case 'e':
                    type = "Enrollment";
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    type = string.Empty;
                    return false;
            }
            return true;
        }
        private static IEnumerable<string> DisplayOptions()
        {
            foreach (var option in Enum.GetValues(typeof(CRUDOps)))
            {
                yield return option.ToString();
            }
        }
        public static void DisplayOptionsMenu<T>()
        {
            Console.WriteLine($"\n** You now can edit {typeof(T).Name}s.");
            Console.WriteLine("Please choose an operation from the menu below:");
            foreach (var option in DisplayOptions())
            {
                Console.WriteLine($"({char.ToLower(option[0])}) for {option}");
            }
        }
        public static bool HandleOperationChoice(char choice)
        {
            var opsList = new List<char> { 'c', 'g', 'd', 'u', 'r' };
            if (opsList.Contains(choice))
                return true;
            return false;
        }


        /*********************Entity Service Methods*************************/
        public static async Task HandleServiceAsync(object myService, char opChoice)
        {
            if (myService is null)
            {
                Console.WriteLine("Service is not initialized. " +
                    "Please check the service instance.");
                return;
            }

            if (myService is DepartmentServices)
            {
                await EntityServicesAsync<Department, DepartmentServices>(opChoice);
            }
            else if (myService is StudentServices)
            {
                await EntityServicesAsync<Student, StudentServices>(opChoice);
            }
            else if (myService is CourseServices)
            {
                await EntityServicesAsync<Course, CourseServices>(opChoice);
            }
            else if (myService is StdCrsServices)
            {
                await EntityServicesAsync<StdCr, StdCrsServices>(opChoice);
            }
            else
            {
                Console.WriteLine("No valid service found for the selected type.");
            }
        }
        private static async Task EntityServicesAsync<EType, EService>(char opChoice)
            where EType : class, new()
            where EService : IEntityServices<EType>, new()
        {
            Result<EType> result = default;
            var myService = new EService();
            int id;
            switch (opChoice)
            {
                case 'g':
                    // Handle GetAll operation for Department
                    DisplayListOfData(await myService.GetAll());
                    break;
                case 'd':
                    id = AcceptId();
                    result = await myService.GetById(id);
                    DisplaySingleData(result.Data);
                    break;
                case 'c':
                    EType entity = AcceptEntityData<EType>();
                    result = await myService.Add(entity);
                    break;
                case 'u':
                    DisplayListOfData(await myService.GetAll());
                    id = AcceptId();
                    result = await myService.GetById(id);
                    DisplaySingleData(result.Data);
                    Console.WriteLine("\tEnter new values.");
                    EType updatedEntity = AcceptEntityData<EType>();
                    result = await myService.Update(updatedEntity);
                    break;
                case 'r':
                    DisplayListOfData(await myService.GetAll());
                    id = AcceptId();
                    result = await myService.GetById(id);
                    DisplaySingleData(result.Data);
                    if (ConfirmDelete())
                        result = await myService.Delete(id);
                    break;
                default:
                    Console.WriteLine("Invalid operation choice.");
                    break;
            }
            HandleResult(result);
        }
        private static EType AcceptEntityData<EType>() where EType : class, new()
        {
            var entity = new EType();
            var props = typeof(EType).GetProperties();
            foreach (var prop in props)
            {
                if (typeof(EType) != typeof(StdCr))
                {
                    if (!prop.Name.Contains("Id", StringComparison.OrdinalIgnoreCase)
                        && (prop.PropertyType == typeof(int)
                        || prop.PropertyType == typeof(string)))
                    {
                        Console.Write($"Enter {prop.Name}: ");
                        string input = Console.ReadLine();
                        if (prop.PropertyType == typeof(int))
                        {
                            int value;
                            while (!int.TryParse(input, out value) || value <= 0)
                            {
                                Console.Write($"Invalid input. Please enter a valid {prop.Name}: ");
                                input = Console.ReadLine();
                            }
                            prop.SetValue(entity, value);
                        }
                        else
                        {
                            prop.SetValue(entity, input.Trim());
                        }
                    }
                }
                else
                {
                    if (prop.PropertyType == typeof(int)
                        || prop.PropertyType == typeof(string))
                    {
                        Console.Write($"Enter {prop.Name}: ");
                        string input = Console.ReadLine();
                        if (prop.PropertyType == typeof(int))
                        {
                            int value;
                            while (!int.TryParse(input, out value) || value <= 0)
                            {
                                Console.Write($"Invalid input. Please enter a valid {prop.Name}: ");
                                input = Console.ReadLine();
                            }
                            prop.SetValue(entity, value);
                        }
                        else
                        {
                            prop.SetValue(entity, input.Trim());
                        }

                    }
                }
            }
            return entity;
        }
        public static void DisplaySingleData<T>(T data)
        {
            var jsonData = System.Text.Json.JsonSerializer.Serialize(data);
            Console.WriteLine(jsonData);
        }
        public static void DisplayListOfData<T>(List<T> data)
        {
            if (data is null || data.Count == 0)
            {
                Console.WriteLine("No data available.");
                return;
            }

            Console.WriteLine($"List of {typeof(T).Name}s:");
            foreach (var item in data)
            {
                var jsonItem = System.Text.Json.JsonSerializer.Serialize(item);
                Console.WriteLine(jsonItem);
            }
        }
        public static void HandleResult<T>(Result<T> result) where T : class
        {
            if (result is not null)
                if (!result.IsSuccess)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\n\t\t{result.Message}\n");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\n\t\t{result.Message}\n");
                    Console.ResetColor();
                    if (result.Data is not null)
                        DisplaySingleData(result.Data);
                }
        }
        public static int AcceptId()
        {
            Console.Write("Enter ID: ");
            int id;
            while (!int.TryParse(Console.ReadLine(), out id) || id <= 0)
            {
                Console.Write("Invalid ID. Please enter a valid ID greater than 0: ");
            }
            return id;
        }
        public static bool ConfirmDelete()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n\tAre you sure to delete?\t(y\\n)\n");
            Console.ResetColor();
            Console.Write("- ");
            char choice = AcceptUserChoice();
            if (choice == 'y' || choice == 'Y')
            {
                Console.WriteLine("Deletion confirmed.");
                return true;
            }
            else
            {
                Console.WriteLine("Deletion cancelled.");
                return false;
            }
        }
    }
}
