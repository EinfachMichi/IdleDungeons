using System.Collections.Generic;
using UnityEngine;

public class Inventory<T>
{
    public int Size => size;

    private int size;
    private Slot<T>[] slots;
    
    public Inventory(int size)
    {
        this.size = size;
        slots = new Slot<T>[size];
        
        InitInventory();
    }

    private void InitInventory()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            Slot<T> newSlot = new Slot<T>();
            slots[i] = newSlot;
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

    public int ItemCount()
    {
        int count = 0;
        foreach (Slot<T> slot in slots)
        {
            if (slot.Item != null)
            {
                count++;
            }
        }
        return count;
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
    
    private class Slot<T>
    {
        public T Item;
    }
}

