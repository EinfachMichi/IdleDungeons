using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory<T> where T : Item
{
    public int Length => length;

    private int length = 18;
    private List<Slot<T>> slots;

    public Inventory()
    {
        slots = new();
        
        InitInventory(length);
    }

    private void InitInventory(int length)
    {
        for (int i = 0; i < length; i++)
        {
            slots.Add(new Slot<T>());
        }
    }
    
    public void AddItem(T newItem)
    {
        foreach (Slot<T> slot in slots)
        {
            if (slot.Item == null)
            {
                slot.Item = newItem;
                return;
            }
        }
        Debug.Log("Error: No inventory Space!");
    }

    public List<T> ToList()
    {
        List<T> list = new List<T>();
        foreach (Slot<T> slot in slots)
        {
            if(slot.Item == null) continue;
            list.Add(slot.Item);
        }
        return list;
    }

    public void Expand(int amount)
    {
        length += amount;
        InitInventory(amount);
    }

    public void SortByNameAscending() => slots.OrderBy(t => t.Item.Name);
    public void SortByNameDescending() => slots.OrderByDescending(t => t.Item.Name);
    public void SortByRarityAscending() => slots.OrderBy(t => t.Item.Rarity);
    public void SortByRarityDescending() => slots.OrderByDescending(t => t.Item.Rarity);
    
    private class Slot<T> where T : Item
    {
        public T Item;
    }
}

