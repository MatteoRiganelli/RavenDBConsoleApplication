using Raven.Client;
using Raven.Client.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RavenDBConsoleApplication.Models;
using static RavenDBConsoleApplication.Methods;
using static RavenDBConsoleApplication.CustomCommand;

namespace RavenDBConsoleApplication
{
    class Program
    {

        static void Main(string[] args)
        {

            // The code provided will print ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.


            using (IDocumentStore store = new DocumentStore
            {
                Url = Variables.__URLCONNECTION,
                DefaultDatabase = Variables.__DBNAME
            })
            {
                store.Initialize();

                string command;
                do
                {
                    Console.Clear();
                    DisplayMenu();

                    command = Console.ReadLine().ToUpper();
                    switch (command)
                    {
                        case "C":
                            Creation(store);
                            break;
                        case "G":
                            GetOwnerById(store);
                            break;
                        case "F":
                            GetOwnerByCf(store);
                            break;
                        case "L":
                            ListOfIdOwners(store);
                            break;
                        case "N":
                            QueryOwnersByName(store);
                            break;
                        case "P":
                            QueryOwnersByPetsName(store);
                            break;
                        case "R":
                            RenameOwnerNameById(store);
                            break;
                        case "R1":
                            RenameOwnerNameByCf(store);
                            break;
                        case "D":
                            DeleteOwnerById(store);
                            break;
                        case "Q":
                            break;
                        default:
                            Console.WriteLine("Unknown command.");
                            break;
                    }
                } while (command != "Q");
            }

            Console.WriteLine("Bye Bye...");
            Console.ReadKey();

        }


    }






}
