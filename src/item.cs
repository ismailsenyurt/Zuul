class Item
{
    public int Weight { get; }
    public string Description { get; }
    //are auto-properties with only getters, meaning their values can only be set once in the constructor and cant be changed later


    public Item(int weight, string description)
    {
        Weight = weight;
        Description = description;
    }
}
