using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Client;
using RavenDBConsoleApplication.Models;

namespace RavenDBConsoleApplication
{
    public static class Methods
    {
        // Returns the entered string if it is not empty, otherwise, keeps asking for it.
        public static string ReadNotEmptyString(string message)
        {
            Console.WriteLine(message);
            string res;
            do
            {
                res = Console.ReadLine().Trim();
                if (res == string.Empty)
                {
                    Console.WriteLine("Entered value cannot be empty.");
                }
            } while (res == string.Empty);

            return res;
        }

        // Will use this to prevent text from being cleared before we've read it.
        public static void PressAnyKeyToContinue()
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }

        // Prepends the 'owners/' prefix to the id if it is not present (more on it later)
        public static string NormalizeOwnerId(string id)
        {
            if (!id.ToLower().StartsWith("owners/"))
            {
                id = "owners/" + id;
            }

            return id;
        }

        // Displays the menu
        public static void DisplayMenu()
        {
            Console.WriteLine("Select a command");
            Console.WriteLine("C - Create an owner with pets");
            Console.WriteLine("L - List of id's owners");
            Console.WriteLine("G - Get an owner with its pets by Owner Id");
            Console.WriteLine("F - Get an owner with its pets by fiscal code");
            Console.WriteLine("N - Query owners whose name starts with...");
            Console.WriteLine("P - Query owners who have a pet whose name starts with...");
            Console.WriteLine("R - Rename an owner name by Id");
            Console.WriteLine("R1 - Rename an owner name by fiscal code (CF)");
            Console.WriteLine("D - Delete an owner by Id");
            Console.WriteLine("Q - Exit");
            Console.WriteLine();
        }


    }


}
