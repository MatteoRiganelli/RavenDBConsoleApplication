using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Client;
using RavenDBConsoleApplication.Models;
using static RavenDBConsoleApplication.Methods;

namespace RavenDBConsoleApplication
{
    public static class CustomCommand
    {

        public static Owner CreateOwner(IDocumentStore store)
        {

            string name = ReadNotEmptyString("Enter the owner's name.");
            string surname = ReadNotEmptyString("Enter the owner's surname");
            string cf = ReadNotEmptyString("Enter the owner's fiscal code");

            return new Owner
            {
                Name = name,
                Surname = surname,
                Cf = cf.ToUpper()
            };
        }

        public static bool OwnerAlreadyExists(IDocumentStore store, Owner owner)
        {
            int checkIfAlreadyExist = 0;

            using (IDocumentSession session = store.OpenSession())
            {
                checkIfAlreadyExist = session.Query<Owner>()
                    .Where(ow => ow.Cf == owner.Cf)
                    .ToList().Count;

                Console.WriteLine("Already " + checkIfAlreadyExist + " owners with this CF");
            }

            if (checkIfAlreadyExist > 0)
                return true;
            else
                return false;
        }

        public static Pet CreatePet()
        {
            string name = ReadNotEmptyString("Enter the name of the pet.");
            string race = ReadNotEmptyString("Enter the race of the pet.");
            string color = ReadNotEmptyString("Enter the color of the pet.");

            return new Pet
            {
                Color = color,
                Race = race,
                Name = name
            };
        }

        public static void Creation(IDocumentStore store)
        {
            
            Owner owner = CreateOwner(store);

            bool exist = OwnerAlreadyExists(store, owner);

            if (exist)
            {
                Console.WriteLine("User Already exist");
                PressAnyKeyToContinue();
            }
            else
            { 
                Console.WriteLine(
                    "Do you want to create a pet and assign it to {0}? (Y/y: yes, anything else: no)",
                    owner.Name);

                bool createPets = Console.ReadLine().ToLower() == "y";

                while (createPets)
                {
                    owner.Pets.Add(CreatePet());

                    Console.WriteLine("Do you want to create a pet and assign it to {0}?", owner.Name);
                    createPets = Console.ReadLine().ToLower() == "y";
                } 

                using (IDocumentSession session = store.OpenSession())
                {
                    session.Store(owner);
                    session.SaveChanges();
                }
            }
        }

        public static void GetOwnerById(IDocumentStore store)
        {
            Owner owner;
            string id = NormalizeOwnerId(ReadNotEmptyString("Enter the Id of the owner to display."));

            using (IDocumentSession session = store.OpenSession())
            {
                owner = session.Load<Owner>(id);
            }

            if (owner == null)
            {
                Console.WriteLine("Owner not found.");
            }
            else
            {
                Console.WriteLine(owner);
            }

            PressAnyKeyToContinue();
        }

        public static void GetOwnerByCf(IDocumentStore store)
        {
            Owner owner;
            string cf = ReadNotEmptyString("Enter the fiscal code of the owner to display");

            using (IDocumentSession session = store.OpenSession())
            {
                owner = session.Query<Owner>()
                    .Where(ow => ow.Cf == cf.ToUpper())
                    .ToList().FirstOrDefault();
            }

            if (owner != null)
                Console.WriteLine(owner);
            else
                Console.WriteLine("owner not found.");

            PressAnyKeyToContinue();


        }

        public static void QueryOwnersByName(IDocumentStore store)
        {
            string namePart = ReadNotEmptyString("Enter a name to filter by.");

            List<Owner> result;
            using (IDocumentSession session = store.OpenSession())
            {
                result = session.Query<Owner>()
                   .Where(ow => ow.Name.StartsWith(namePart))
                   .Take(10)
                   .ToList();
            }

            if (result.Count > 0)
            {
                result.ForEach(ow => Console.WriteLine(ow));
            }
            else
            {
                Console.WriteLine("No matches.");
            }
            PressAnyKeyToContinue();
        }

        public static void ListOfIdOwners(IDocumentStore store)
        {
            List<string> result;

            using (IDocumentSession session = store.OpenSession())
            {
                result = session.Query<Owner>()
                    .Select(ow => ow.Id)
                    .ToList();
            }

            if (result.Count > 0)
            {
                result.ForEach(ow => Console.WriteLine(ow.Replace("owners/", "- ")));
            }
            else
            {
                Console.WriteLine("Empty List.");
            }
            PressAnyKeyToContinue();


        }

        public static void QueryOwnersByPetsName(IDocumentStore store)
        {
            string petsNamePart = ReadNotEmptyString("Enter a pets name to filter by.");

            List<Owner> result;

            using (IDocumentSession session = store.OpenSession())
            {
                result = session.Query<Owner>()
                    .Where(ow => ow.Pets.Any(p => p.Name.StartsWith(petsNamePart)))
                    .Take(10)
                    .ToList();    
            }

            if (result.Count > 0)
            {
                result.ForEach(ow => Console.WriteLine(ow));
            }
            else
            {
                Console.WriteLine("No Matches.");
            }

            PressAnyKeyToContinue();

        }

        public static void DeleteOwnerById(IDocumentStore store)
        {
            string id = NormalizeOwnerId(ReadNotEmptyString("Enter the Id of the owner to delete."));

            double Num;

            bool isNum = double.TryParse(id, out Num);

            if (isNum)
                Console.WriteLine(id);
            else
                Console.WriteLine("Invalid Number");


            using (IDocumentSession session = store.OpenSession())
            {
                session.Delete(id);
                session.SaveChanges();
                Console.WriteLine("Deleted user " + id);
            }
            PressAnyKeyToContinue();
            
        }

        public static void RenameOwnerNameById(IDocumentStore store)
        {
            string id = NormalizeOwnerId(ReadNotEmptyString("Enter the id of the owner to rename."));
            string newName = ReadNotEmptyString("Enter the new name.");

            store.DatabaseCommands.Patch(id, new Raven.Abstractions.Data.PatchRequest[] {
                new Raven.Abstractions.Data.PatchRequest
                {
                    Name = "Name",
                    Value = newName
                }
            });
        }

        public static void RenameOwnerNameByCf(IDocumentStore store)
        {
            Owner owner;

            string cf = ReadNotEmptyString("Please insert the owner's fiscal code (CF): ");

            using (IDocumentSession session = store.OpenSession())
            {
                owner = session.Query<Owner>()
                    .Where(ow => ow.Cf == cf).ToList().FirstOrDefault();
            }

            if (owner != null)
                Console.WriteLine(owner);
            else
                Console.WriteLine("owner not found");

            PressAnyKeyToContinue();
        }




    }
}
