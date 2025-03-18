using System.Runtime.InteropServices;

class Player
{
    // fields
    private int health;
    private Inventory backpack;

    // auto property 
    public Room CurrentRoom { get; set; }

    // constructor
    public Player()
    {
        health = 100;
        CurrentRoom = null;
        backpack = new Inventory(10);
    }

    public Inventory Backpack
    {
        get { return backpack; }
    }

    // methods
    public void Damage(int amount)
    {
        health -= amount;
    }

    public void Heal(int amount)
    {
        health += amount;
    }

    public bool IsAlive()
    {
        return health > 0;
    }

    public bool TakeFromChest(string itemName)
    {
        Item item = CurrentRoom.Chest.Get(itemName);
        if (item == null)
        {
            Console.WriteLine(itemName + "is not in the chest");
            return false;
        }
        if (item.Weight > backpack.FreeWeight())
        {
            System.Console.WriteLine("Not enough space");
            return false;
        }
        bool success = backpack.Put(itemName, item);
        if (!success)
        {
            CurrentRoom.Chest.Put(itemName, item);
        }
        return true;
    }

    public bool DropToChest(string itemName)
    {
        Item item = backpack.Get(itemName);
        if (item == null)
        {
            System.Console.WriteLine(itemName + "is not in the backpack");
            return false;
        }

        CurrentRoom.Chest.Put(itemName, item);
        System.Console.WriteLine($"You dropped {itemName}");
        return true;
    }

    public int GetHealth()
    {
        return health;
    }

    public string GetInventory()
    {
        return backpack.GetItems();
    }
}
