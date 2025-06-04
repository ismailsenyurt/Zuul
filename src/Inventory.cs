using System.Runtime.InteropServices;
using System.Transactions;

class Inventory
{
    //maxWeight: the max weight the inventory can hold
    private int maxWeight;
    private Dictionary<string, Item> items;

    //max weight for inventory
    public Inventory(int maxWeight)
    {
        this.maxWeight = maxWeight;
        this.items = new Dictionary<string, Item>();
    }


    //methods, add an item if there is enough free weight
    public bool Put(string itemName, Item item)
    {
        if (item.Weight > FreeWeight())
        {
            Console.WriteLine("Not enough space");
            return false;
        }
        items.Add(itemName, item);
        return true;
    }

    //remove and return an item by name.
    public Item Get(string itemName)
    {
        if (!items.ContainsKey(itemName))
        {
            Console.WriteLine(itemName + " couldn't be found.");
            return null;
        }

        Item item = items[itemName];
        items.Remove(itemName);
        return item;
    }

    //calculates and returns the total weight of all items
    public int TotalWeight()
    {
        int total = 0;
        foreach (var item in items.Values)
        {
            total += item.Weight;
        }
        return total;
    }

    //returns how much more weight can be added
    public int FreeWeight()
    {
        return maxWeight - TotalWeight();
    }

    //print the names of all item names to console
    public void ShowItems()
    {
        System.Console.WriteLine($"Items:");
        foreach (var itemName in items.Keys)
        {
            System.Console.WriteLine(itemName);
        }
    }

    public string GetItems()
    {
        if (items.Count == 0)
        {
            return "Your backpack is empty";
        }
        return string.Join(",", items.Keys);
    }
}