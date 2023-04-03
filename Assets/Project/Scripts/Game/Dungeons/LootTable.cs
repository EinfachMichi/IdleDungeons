using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "New LootTable", menuName = "New LootTable")]
public class LootTable : ScriptableObject
{
    [SerializeField] private Vector2 goldRange;
    [SerializeField] private ItemPool[] itemPools;

    private int LastPoolIndex => itemPools.Length - 1;

    public Loot DropLoot()
    {
        Loot loot = new Loot(GetRandomGold(), new List<Item>());

        int drops = Drops();
        for (int i = 0; i < drops; i++)
        {
            loot.AddItem(GetRandomItem(itemPools[i]));
        }

        return loot;
    }

    private float GetRandomGold() => (int) Random.Range(goldRange.x, goldRange.y);
    
    private int Drops()
    {
        for (int i = LastPoolIndex; i >= 0; i--)
        {
            float chance = Random.Range(0f, 1f);
            float unlockChance = GetChance(i);
            if (chance <= unlockChance)
            {
                return i + 1;
            }
        }
        return 0;
        
        float GetChance(int length)
        {
            float chance = itemPools[0].chanceToUnlock;
            for (int i = 1; i <= length; i++)
            {
                chance *= itemPools[i].chanceToUnlock;
            }
            return chance;
        }
    }

    public static Item GetRandomItem(ItemPool pool)
    {
        float rand = Random.Range(0f, 1f);

        float start = 0;
        foreach (ItemDrop drop in pool.items)
        {
            if (rand >= start && rand <= start + drop.dropChance)
            {
                Rarity itemRarity = GetRandomRarity();
                Item itemDrop = GetItemType(drop.item, itemRarity);
                return itemDrop;
            }
            start += drop.dropChance;
        }

        return null;
    }

    public static Item GetItemType(ItemData data, Rarity rarity)
    {
        Item newItem;
        switch (data.ItemType)
        {
            case ItemType.Weapon:
                newItem = new Weapon((WeaponData) data, rarity);
                break;
            case ItemType.Armor:
                newItem = new Armor((ArmorData) data, rarity);
                break;
            case ItemType.Relict:
                newItem = new Relict((RelictData) data, rarity);
                break;
            default:
                newItem = null;
                break;
        }
        return newItem;
    }
    
    public static Rarity GetRandomRarity()
    {
        float chance = Random.Range(0f, 1f);
        float start = 0;
        for(int i = 0; i < RarityClass.Length; i++)
        {
            if (chance >= start && chance <= start + RarityClass.RarityChances[i])
            {
                return (Rarity) i;
            }
            start += RarityClass.RarityChances[i];
        }
        return Rarity.Common;
    }

    [Serializable]
    public struct ItemPool
    {
        public ItemDrop[] items;
        public float chanceToUnlock;
    }
    
    [Serializable]
    public struct ItemDrop
    {
        public ItemData item;
        public float dropChance;
    }
}