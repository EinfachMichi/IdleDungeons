using System;

public class EquipmentInventory
{
    public event Action<Item> OnItemEquipped;
    public event Action<Item> OnItemUnequipped;

    private EquipmentSlot<Armor>[] armorSlots;
    private EquipmentSlot<Weapon> weaponSlot;
    private EquipmentSlot<Relict> relictSlot;
    
    public EquipmentInventory()
    {
        InitEquipmentSlots();
    }

    private void InitEquipmentSlots()
    {
        armorSlots = new EquipmentSlot<Armor>[Enum.GetNames(typeof(ArmorType)).Length];
        weaponSlot = new EquipmentSlot<Weapon>();
        relictSlot = new EquipmentSlot<Relict>();
    }

    public void AddItem(Item item)
    {
        bool itemAdded = false;
        switch (item.ItemType)
        {
            case ItemType.Weapon:
                Weapon weapon = (Weapon) item;
                itemAdded = weaponSlot.AddItem(weapon);
                break;
            case ItemType.Armor:
                Armor armor = (Armor) item;
                itemAdded = armorSlots[(int) armor.ArmorType].AddItem(armor);
                break;
            case ItemType.Relict:
                Relict relict = (Relict) item;
                itemAdded = relictSlot.AddItem(relict);
                break;
        }
        
        if(itemAdded) OnItemEquipped?.Invoke(item);
    }

    public void RemoveArmorItem(int index)
    {
        OnItemUnequipped?.Invoke(armorSlots[index].Item);
        armorSlots[index].RemoveItem();
    }

    public void RemoveWeaponItem()
    {
        OnItemUnequipped?.Invoke(weaponSlot.Item);
        weaponSlot.RemoveItem();
    }

    public void RemoveRelictItem()
    {
        OnItemUnequipped?.Invoke(relictSlot.Item);
        relictSlot.RemoveItem();
    }

    private class EquipmentSlot<TItem>
    {
        public TItem Item;

        public bool AddItem(TItem item)
        {
            if (Item == null)
            {
                Item = item;
                return true;
            }

            return false;
        }

        public void RemoveItem()
        {
            Item = default;
        }
    }
}