using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public struct LootTable
{
    [SerializeField] private Vector2 goldRange;
    [SerializeField] private Drop firstDrop;
    [SerializeField] private Drop secondDrop;
    [SerializeField] private Drop thirdDrop;

    public float GoldDrop()
    {
        return Mathf.RoundToInt(Random.Range(goldRange.x, goldRange.y));
    }

    public List<Item> ItemDrop()
    {
        List<Item> items = new();

        float firstDropChance = Random.Range(0f, 100f);
        if (firstDropChance > firstDrop.ChanceToActivate) return items;
        items.Add(firstDrop.GetItem());
        
        float secondDropChance = Random.Range(0f, 100f); 
        if (secondDropChance > secondDrop.ChanceToActivate) return items;
        items.Add(secondDrop.GetItem());
        
        float thirdDropChance = Random.Range(0f, 100f);
        if (thirdDropChance > thirdDrop.ChanceToActivate) return items;
        items.Add(thirdDrop.GetItem());
        
        return items;
    }
    
    [Serializable]
    struct Drop
    {
        public float ChanceToActivate => chanceToActivate;
        
        [SerializeField] private ItemDrop[] itemDrops;
        [SerializeField] private float chanceToActivate;
        
        public Item GetItem()
        {
            float range = itemDrops.Sum(t => t.dropChance);
            float rand = Random.Range(0f, range);

            float start = 0;
            foreach (var drop in itemDrops)
            {
                if (rand >= start && rand <= start + drop.dropChance)
                {
                    return drop.Item();
                }

                start += drop.dropChance;
            }

            return null;
        }

        [Serializable]
        struct ItemDrop
        {
            public ItemData itemData;
            public float dropChance;
            
            public Item Item()
            {
                Rarity rarity = Rarity.Common;
                float rarityChance = Random.Range(0f, 1f);
                for (int i = 0; i < RarityClass.Length; i++)
                {
                    if (rarityChance < RarityClass.RarityChances[i])
                    {
                        rarity = (Rarity) i;
                    }
                }
                
                Item newItem;
                switch (itemData.ItemType)
                {
                    case ItemType.Weapon:
                        newItem = new Weapon((WeaponData) itemData, rarity);
                        break;
                    case ItemType.Armor:
                        newItem = new Armor((ArmorData) itemData, rarity);
                        break;
                    case ItemType.Relict:
                        newItem = new Relict((RelictData) itemData, rarity);
                        break;
                    default:
                        newItem = null;
                        break;
                }

                return newItem;
            }
        }
    }
}