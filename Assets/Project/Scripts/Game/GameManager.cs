using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float Gold => gold;
    private float gold;

    public Inventory<Item> Inventory => inventory;
    [SerializeField] private int inventorySize;
    private Inventory<Item> inventory;

    #region Events / Tick-Handeling

    private const float TickSpeed = 1;
    
    private List<ITickable> tickables = new();
    private float timer;
    
    private void Awake()
    {
        Instance = this;
        inventory = new Inventory<Item>(inventorySize);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= TickSpeed)
        {
            foreach (ITickable tickable in tickables)
            {
                tickable.Tick();
            }

            timer = 0;
        }
    }

    public void AddToRegister(ITickable tickable)
    {
        tickables.Add(tickable);
    }

    public void RemoveFromRegister(ITickable tickable)
    {
        tickables.Remove(tickable);
    }
    
    #endregion
    
    #region Character

    [SerializeField] private List<Character> characterArchive;
    public List<Character> OwnedCharacters => ownedCharacters;
    private List<Character> ownedCharacters = new();
    public int OwnedCharacterCount => ownedCharacters.Count;

    public void UnlockNewCharacter()
    {
        if (OwnedCharacterCount >= characterArchive.Count) return;
        
        Character newCharacter = Instantiate(characterArchive[ownedCharacters.Count]);
        newCharacter.name = newCharacter.Name;
        ownedCharacters.Add(newCharacter);
    }
    
    #endregion

    #region Dungeon

    [SerializeField] private List<Dungeon> dungeonArchive;
    private List<Dungeon> runningDungeons = new();
    public List<Dungeon> RunningDungeons => runningDungeons;
    
    public void EnterDungeon(int dungeonIndex, int characterIndex)
    {
        bool characterAlreadyInDungeon = false;
        foreach (Dungeon runningDungeon in runningDungeons)
        {
            if (runningDungeon.Character == ownedCharacters[characterIndex])
            {
                characterAlreadyInDungeon = true;
                break;
            }
        }
        
        if (runningDungeons.Contains(dungeonArchive[dungeonIndex])
            || characterIndex >= ownedCharacters.Count
            || characterAlreadyInDungeon
            || !ownedCharacters[characterIndex].IsAlive
           )
        {
            return;
        }
        
        Dungeon dungeon = Instantiate(dungeonArchive[dungeonIndex]);
        dungeon.name = $"Dungeon Difficulty: {dungeon.Difficulty}";
        dungeon.EnterDungeon(ownedCharacters[characterIndex]);
        runningDungeons.Add(dungeon);
        dungeon.OnDungeonDone += ExitDungeon;
    }

    private void ExitDungeon(Dungeon dungeon)
    {
        runningDungeons.Remove(dungeon);
        dungeon.OnDungeonDone -= ExitDungeon;
        Destroy(dungeon.gameObject);
    }

    public void AddLoot(Loot loot)
    {
        gold += loot.Gold;
        foreach (Item item in loot.Items)
        {
            inventory.AddItem(item);
        }
    }

    #endregion
}