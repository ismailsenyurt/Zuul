using System;

class Game
{
	// Private fields
	private Parser parser;
	private Player player;

	// Constructor
	public Game()
	{
		parser = new Parser();
		player = new Player();
		CreateRooms();
	}

	// Initialise the Rooms (and the Items)
	private void CreateRooms()
	{
		// Create the rooms
		Room outside = new Room("outside the main entrance of the university");
		Room theatre = new Room("in a lecture theatre");
		Room pub = new Room("in the campus pub");
		Room lab = new Room("in a computing lab");
		Room office = new Room("in the computing admin office");
		Room library = new Room("in the library");
		Room cafeteria = new Room("in the cafeteria");
		Room emergencyExit = new Room("in the exit");

		// Initialise room exits
		outside.AddExit("east", theatre);
		outside.AddExit("south", lab);
		outside.AddExit("west", pub);

		theatre.AddExit("west", outside);

		pub.AddExit("east", outside);

		lab.AddExit("north", outside);
		lab.AddExit("east", office);
		lab.AddExit("west", library);

		office.AddExit("west", lab);

		library.AddExit("south", cafeteria);
		library.AddExit("east", lab);

		cafeteria.AddExit("north", library);
		cafeteria.AddExit("south", emergencyExit);

		// Create your Items here
		Item knife = new Item(2, "basic knife");
		Item baseballBat = new Item(4, "regular baseball bat");
		Item bandage = new Item(2, "basic heal item");
		// And add them to the Rooms
		pub.Chest.Put("knife", knife);
		office.Chest.Put("baseballBat", baseballBat);
		lab.Chest.Put("bandage", bandage);
		// Start game outside
		player.CurrentRoom = outside;
	}

	//  Main play routine. Loops until end of play.
	public void Play()
	{
		PrintWelcome();

		
		bool finished = false;
		while (!finished)
		{
			Command command = parser.GetCommand();
			finished = ProcessCommand(command);
		}
		Console.WriteLine("Thank you for playing.");
		Console.WriteLine("Press [Enter] to continue.");
		Console.ReadLine();
	}

	// Print out the opening message for the player.
	private void PrintWelcome()
	{
		Console.WriteLine();
		Console.WriteLine("Welcome to Zuul!");
		Console.WriteLine("Zuul is a new, incredibly boring adventure game.");
		Console.WriteLine("Type 'help' if you need help.");
		Console.WriteLine();
		Console.WriteLine(player.CurrentRoom.GetLongDescription());
	}

	// Given a command, process (that is: execute) the command.
	// If this command ends the game, it returns true.
	// Otherwise false is returned.
	private bool ProcessCommand(Command command)
	{
		bool wantToQuit = false;

		if (command.IsUnknown())
		{
			Console.WriteLine("I don't know what you mean...");
			return wantToQuit; // false
		}

		switch (command.CommandWord)
		{
			case "help":
				PrintHelp();
				break;
			case "go":
				GoRoom(command);
				break;
			case "quit":
				wantToQuit = true;
				break;
			case "look":
				Look();
				break;
			case "status":
				Status();
				break;
			case "take":
				Take(command);
				break;
			case "drop":
				Drop(command);
				break;
			case "use":
				player.Use(command.SecondWord);
				break;
		}

		return wantToQuit;
	}

	// ######################################
	// implementations of user commands:
	// ######################################

	// Print out some help information.
	// Here we print the mission and a list of the command words.
	private void PrintHelp()
	{
		Console.WriteLine("You are lost. You are alone.");
		Console.WriteLine("You wander around at the university.");
		Console.WriteLine();
		// let the parser print the commands
		parser.PrintValidCommands();
	}

	// Try to go to one direction. If there is an exit, enter the new
	// room, otherwise print an error message.
	private void GoRoom(Command command)
	{
		if (!command.HasSecondWord())
		{
			// if there is no second word, we don't know where to go...
			Console.WriteLine("Go where?");
			return;
		}

		string direction = command.SecondWord;

		// Try to go to the next room.
		Room nextRoom = player.CurrentRoom.GetExit(direction);
		if (nextRoom == null)
		{
			Console.WriteLine("There is no door to " + direction + "!");
			return;
		}

		player.CurrentRoom = nextRoom;
		Console.WriteLine(player.CurrentRoom.GetLongDescription());
		player.Damage(10);

		if (player.CurrentRoom.GetShortDescription() == "in the exit")
		{
			Console.WriteLine("You won. Congratulations!");
			Environment.Exit(0);
		}
	}
	private void Look()
	{
		Console.WriteLine(player.CurrentRoom.GetLongDescription());
		if (player.CurrentRoom.Chest.TotalWeight() == 0)
		{
			System.Console.WriteLine("There is nothing to be found.");
		}
		else
		{
			player.CurrentRoom.Chest.ShowItems();
		}
	}

	private void Status()
	{
		Console.WriteLine($"Yout health is: {player.GetHealth()}/100");
		Console.WriteLine($"Capacity: {player.Backpack.TotalWeight()}/4");
		Console.WriteLine($"Inventory: {player.Backpack.GetItems()}");
	}

	private void Take(Command command)
	{
		if (!command.HasSecondWord())
		{
			System.Console.WriteLine("Take what?");
			return;
		}

		string itemName = command.SecondWord;
		bool success = player.TakeFromChest(itemName);

		if (success)
		{
			System.Console.WriteLine(itemName + " is added in your backpack.");
		}
	}

	private void Drop(Command command)
	{
		if (!command.HasSecondWord())
		{
			System.Console.WriteLine("Drop what?");
			return;
		}

		string itemName = command.SecondWord;
		bool success = player.DropToChest(itemName);
		if (success)
		{
			System.Console.WriteLine(itemName + " is deleted from your backpack.");
		}
	}
}
