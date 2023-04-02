using System;

public class EquipmentSlots
{
    public event Action<Item> OnItemEquipped;
    public event Action<Item> OnItemUnequipped;
    
    private Item[] items;

    public EquipmentSlots(int size)
    {
        items = new Item[size];
    }

    public void SetItemAtSlot(Item item, int index)
    {
        if (index >= items.Length
            || items[index] != null) return;

        items[index] = item;
        OnItemEquipped?.Invoke(item);
    }

    public void RemoveItemAtSlot(int index)
    {
        if (index >= items.Length) return;
        
        OnItemEquipped?.Invoke(items[index]);
        items[index] = null;
    }
}
