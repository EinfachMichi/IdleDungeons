using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static event Action<Dungeon> OnDungeonEntered;
    public static event Action OnCharacterUnlocked; 

    private const float TickSpeed = 1;

    private List<ITickable> tickables = new();
    private float timer;

    #region Events / Tick-Handeling

    private void Awake()
    {
        Instance = this;
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
    
    #region Character C_

    [SerializeField] private List<Character> C_characterArchive;
    private List<Character> C_ownedCharacters = new();
    public List<Character> OwnedCharacters => C_ownedCharacters;
    public Character LatestCharacter => OwnedCharacters.Last();
    public int OwnedCharacterCount => C_ownedCharacters.Count;

    public void C_UnlockNewCharacter()
    {
        if (OwnedCharacters.Count >= C_characterArchive.Count) return;
        
        Character newCharacter = Instantiate(C_characterArchive[C_ownedCharacters.Count]);
        newCharacter.name = newCharacter.Name;
        C_ownedCharacters.Add(newCharacter);
        
        OnCharacterUnlocked?.Invoke();
    }
    
    #endregion

    #region Dungeon D_

    [SerializeField] private List<Dungeon> D_dungeonArchive;
    private List<Dungeon> D_runningDungeons = new();
    public List<Dungeon> RunningDungeons => D_runningDungeons;
    
    public void EnterDungeon(int dungeonIndex, int characterIndex)
    {
        if (D_runningDungeons.Count != 0)
        {
            bool characterAlreadyInDungeon = false;
            foreach (Dungeon runningDungeon in D_runningDungeons)
            {
                if (runningDungeon.Character == C_ownedCharacters[characterIndex])
                {
                    characterAlreadyInDungeon = true;
                    break;
                }
            }
            
            if (D_runningDungeons.Contains(D_dungeonArchive[dungeonIndex])
                || characterIndex >= C_ownedCharacters.Count
                || characterAlreadyInDungeon
               )
            {
                return;
            }
        }

        Dungeon dungeon = Instantiate(D_dungeonArchive[dungeonIndex]);
        dungeon.name = $"Dungeon: {dungeonIndex + 1}";
        dungeon.StartDungeon(C_ownedCharacters[characterIndex]);
        D_runningDungeons.Add(dungeon);
        dungeon.OnDungeonDone += ExitDungeon;
        
        OnDungeonEntered?.Invoke(dungeon);
    }

    private void ExitDungeon(Dungeon dungeon)
    {
        D_runningDungeons.Remove(dungeon);
        Destroy(dungeon.gameObject);
    }
    
    #endregion
}
