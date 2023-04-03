using System.Collections.Generic;

public struct Loot
{
    public float Gold => gold;
    public List<Item> Items => items;

    private float gold;
    private List<Item> items;

    public Loot(float gold, List<Item> items)
    {
        this.gold = gold;
        this.items = items;
    }
    
    public Loot(float gold) : this(gold, new List<Item>())
    {
        this.gold = gold;
    }

    public void AddItem(Item newItem)
    {
        if (items == null) items = new List<Item>();
        items.Add(newItem);
    }
    public void AddGold(float gold) => this.gold += gold;
}