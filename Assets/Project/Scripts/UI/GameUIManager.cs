using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : MonoBehaviour, ITickable
{
    public bool debugGold;
    public bool debugCharacter;
    public bool debugInventory;
    
    private void Start()
    {
        GameManager.Instance.AddToRegister(this);
    }

    public void Tick()
    {
        if(debugGold) print($"Gold: {GameManager.Instance.Gold}");
        UpdateUnits(); 
        PrintInventory();
    }

    private void PrintInventory()
    {
        if (!debugInventory) return;

        List<Item> inventoryList = GameManager.Instance.Inventory.ToList();
        foreach (Item item in inventoryList)
        {
            print($"Name: {item.Name}, Rarity: {item.Rarity}");
        }
    }
    
    private void UpdateUnits()
    {
        if (!debugCharacter) return;
        List<Unit> unitsToUpdate = new();
        unitsToUpdate.AddRange(GameManager.Instance.OwnedCharacters);
        foreach (Dungeon dungeon in GameManager.Instance.RunningDungeons)
        {
            unitsToUpdate.Add(dungeon.Enemy);
        }

        foreach (Unit unit in unitsToUpdate)
        {
            UpdateUnitUI(unit);
        }
        
        void UpdateUnitUI(Unit unit)
        {
            //if (unit == null) return;
            
            print($"Name: {unit.name}, " +
                  $"Level: {unit.Level}, " +
                  $"Health: {unit.Health}/{unit.MaxHealth}, " + 
                  $"Damage: {unit.Damage}, " +
                  $"Alive: {unit.IsAlive}"
            );
        }
    }
}
