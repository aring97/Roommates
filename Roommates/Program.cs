using System;
using System.Collections.Generic;
using Roommates.Repositories;
using Roommates.Models;
using System.Linq;

namespace Roommates
{
    class Program
    { //  This is the address of the database.
      //  We define it here as a constant since it will never change.
        private const string CONNECTION_STRING = @"server=localhost\SQLExpress;database=Roommates;integrated security=true";

        static void Main(string[] args)
        {
            RoomRepository roomRepo = new RoomRepository(CONNECTION_STRING);
            ChoreRepository choreRepo = new ChoreRepository(CONNECTION_STRING);
            RoommateRepository roommateRepo = new RoommateRepository(CONNECTION_STRING);
            bool runProgram = true;
            while (runProgram)
            {
                string selection = GetMenuSelection();

                switch (selection)
                {
                    case ("Show all rooms"):
                        List<Room> rooms = roomRepo.GetAll();
                        foreach (Room r in rooms)
                        {
                            Console.WriteLine($"{r.Id} - {r.Name} Max Occupancy({r.MaxOccupancy})");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Search for room"):
                        Console.Write("Room Id: ");
                        int id = int.Parse(Console.ReadLine());

                        Room room = roomRepo.GetById(id);

                        Console.WriteLine($"{room.Id} - {room.Name} Max Occupancy({room.MaxOccupancy})");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Add a room"):
                        Console.Write("Room name: ");
                        string name = Console.ReadLine();

                        Console.Write("Max occupancy: ");
                        int max = int.Parse(Console.ReadLine());

                        Room roomToAdd = new Room()
                        {
                            Name = name,
                            MaxOccupancy = max
                        };

                        roomRepo.Insert(roomToAdd);
                        Console.WriteLine($"{roomToAdd.Name} has been added and assigned an Id of {roomToAdd.Id}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Show all chores"):
                        List<Chore> chores = choreRepo.GetAll();
                        foreach (Chore c in chores) 
                        {
                            Console.WriteLine($"{c.Id} - {c.Name}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Search for chore"):
                        Console.WriteLine("Chore Id: ");
                        int choreId = int.Parse(Console.ReadLine());
                        Chore chore = choreRepo.GetById(choreId);
                        Console.WriteLine($"{chore.Id} - {chore.Name}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Add a chore"):
                        Console.WriteLine("Chore name: ");
                        string choreName = Console.ReadLine();
                        Chore choreToAdd = new Chore()
                        {
                            Name = choreName
                        };
                        choreRepo.Insert(choreToAdd);
                        Console.WriteLine($"{choreToAdd.Name} has been added and assigned an Id of {choreToAdd.Id}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Search for roommate"):
                        Console.WriteLine("Roommate Id: ");
                        int roommateId = int.Parse(Console.ReadLine());
                        Roommate roommate = roommateRepo.GetById(roommateId);
                        Console.WriteLine($"{roommate.Firstname}:");
                        Console.WriteLine($"    Rent Portion:{roommate.RentPortion}");
                        Console.WriteLine($"    Room: {roommate.Room.Name}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Unassigned chores"):
                        List<Chore> unassignedchores = choreRepo.UnassignedChores();
                        foreach(Chore c in unassignedchores)
                        {
                            Console.WriteLine(c.Name);
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Assign chore to roommate"):
                        List<Chore> choreList = choreRepo.GetAll();
                        List<Roommate> roommateList = roommateRepo.GetAll();
                        foreach(Chore c in choreList)
                        {
                            Console.WriteLine($"{c.Id} - {c.Name}");
                        }
                        Console.WriteLine("Please enter the id of the chore.");
                        int choreIdToAdd = int.Parse(Console.ReadLine());
                        Chore chore1 = choreRepo.GetById(choreIdToAdd);
                        foreach(Roommate r in roommateList)
                        {
                            Console.WriteLine($"{r.Id} - {r.Firstname} {r.Lastname}");
                        }Console.WriteLine("Please enter the id of the roommate.");
                        int roommateIdToAdd = int.Parse(Console.ReadLine());
                        Roommate roommate1 = roommateRepo.GetById(roommateIdToAdd);
                        choreRepo.assignChore(roommateIdToAdd, choreIdToAdd);
                        Console.WriteLine($"{roommate1.Firstname} {roommate1.Lastname} has been assigned to {chore1.Name}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Update a room"):
                        List<Room> roomOptions = roomRepo.GetAll();
                        foreach (Room r in roomOptions)
                        {
                            Console.WriteLine($"{r.Id} - {r.Name} Max Occupancy({r.MaxOccupancy})");
                        }

                        Console.Write("Which room would you like to update? ");
                        int selectedRoomId = int.Parse(Console.ReadLine());
                        Room selectedRoom = roomOptions.FirstOrDefault(r => r.Id == selectedRoomId);

                        Console.Write("New Name: ");
                        selectedRoom.Name = Console.ReadLine();

                        Console.Write("New Max Occupancy: ");
                        selectedRoom.MaxOccupancy = int.Parse(Console.ReadLine());

                        roomRepo.Update(selectedRoom);

                        Console.WriteLine($"Room has been successfully updated");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Delete room"):
                        List<Room> allRooms = roomRepo.GetAll();
                        foreach(Room r in allRooms)
                        {
                            Console.WriteLine($"{r.Id} - {r.Name}");
                        }
                        Console.WriteLine("Selecet the Id of the room to delete.");
                        int roomToDelete = int.Parse(Console.ReadLine());
                        roomRepo.Delete(roomToDelete);
                        break;
                    case ("Update a chore"):
                        List<Chore> choreOptions = choreRepo.GetAll();
                        foreach(Chore c in choreOptions)
                        {
                            Console.WriteLine($"{c.Id} - {c.Name}");
                        }
                        Console.WriteLine("Which chore would you like to update?");
                        int selectedChoreId = int.Parse(Console.ReadLine());
                        Chore selectedChore = choreOptions.FirstOrDefault(c => c.Id == selectedChoreId);
                        Console.WriteLine("New Name: ");
                        selectedChore.Name = Console.ReadLine();
                        choreRepo.Update(selectedChore);
                        Console.WriteLine("Chore has been successfully updates.");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Delete chore"):
                        List<Chore> allChore = choreRepo.GetAll();
                        foreach(Chore c in allChore)
                        {
                            Console.WriteLine($"{c.Id} - {c.Name}");
                        }
                        Console.WriteLine("Selecet the Id of the chore to delete.");
                        int choreToDelete = int.Parse(Console.ReadLine());
                        choreRepo.Delete(choreToDelete);
                        break;
                    case ("Exit"):
                        runProgram = false;
                        break;
                }
            }

        }

        static string GetMenuSelection()
        {
            Console.Clear();

            List<string> options = new List<string>()
        {
            "Show all rooms",
            "Search for room",
            "Add a room",
            "Show all chores",
            "Search for chore",
            "Add a chore",
            "Search for roommate",
            "Unassigned chores",
            "Assign chore to roommate",
            "Update a room",
            "Delete room",
            "Update a chore",
            "Delete chore",
            "Exit"
        };

            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i]}");
            }

            while (true)
            {
                try
                {
                    Console.WriteLine();
                    Console.Write("Select an option > ");

                    string input = Console.ReadLine();
                    int index = int.Parse(input) - 1;
                    return options[index];
                }
                catch (Exception)
                {

                    continue;
                }
            }

        }
    }
}
