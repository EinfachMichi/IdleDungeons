using System;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon : MonoBehaviour, ITickable
{
    public event Action<Dungeon> OnDungeonDone;

    public Enemy Enemy => enemy;
    public Character Character => character;
    public Difficullty Difficulty => difficulty;
    public bool AutoPlay => autoPlay;

    private float GoldBonus => character.GoldBonus;
    
    [SerializeField] private Difficullty difficulty;
    [SerializeField] private int enemyLevel;
    [SerializeField] private List<Enemy> enemies;
    [SerializeField] private LootTable lootTable;
    [SerializeField] private bool autoPlay;

    private int currentEnemyIndex;
    private Enemy enemy;
    private Character character;

    private void Start()
    {
        GameManager.Instance.AddToRegister(this);
    }

    public void EnterDungeon(Character character)
    {
        InitCharacter(character);
        StartDungeon();
    }

    private void StartDungeon()
    {
        currentEnemyIndex = 0;
        InitEnemy();
    }
    
    public void Tick()
    {
        HandleUnitTurn(character, enemy);
        HandleUnitTurn(enemy, character);
    }

    private void DungeonComplete()
    {
        Loot loot = lootTable.DropLoot();
        GameManager.Instance.AddLoot(BonusGoldDrop(loot));
        
        print("Dungeon Complete!");
        if (autoPlay)
        {
            StartDungeon();
            return;
        }
        LeaveDungeon();
    }
    
    private void DungeonFailed()
    {
        print("Dungeon Failed!");
        
        LeaveDungeon();
    }
    
    private void LeaveDungeon()
    {
        Character.gameObject.transform.SetParent(null);
        character.OnCharacterDeath -= OnCharacterDeath;
        OnDungeonDone?.Invoke(this);
    }
    
    private void HandleUnitTurn(Unit user, Unit target)
    {
        if (target == null || user == null) return;
        
        user.ApplyDamage(target);
    }

    private void InitEnemy()
    {
        if (currentEnemyIndex >= enemies.Count)
        {
            DungeonComplete();
            return;
        }
        
        enemy = Instantiate(enemies[currentEnemyIndex]);
        enemy.transform.SetParent(transform);
        enemy.ScaleByLevel(enemyLevel);
        currentEnemyIndex++;
    
        enemy.OnEnemyDeath += OnEnemyDeath;
    }

    private void InitCharacter(Character character)
    {
        this.character = character;
        this.character.gameObject.transform.SetParent(transform);
        this.character.OnCharacterDeath += OnCharacterDeath;
    }
    
    private void OnCharacterDeath()
    {
        DungeonFailed();
    }

    private void OnEnemyDeath(Enemy enemy)
    {
        character.AddExperience(enemy.ExperienceDrop);
        
        Loot loot = new Loot(enemy.GoldDrop);
        GameManager.Instance.AddLoot(BonusGoldDrop(loot));
        
        Destroy(enemy.gameObject);
        InitEnemy();
    }

    private Loot BonusGoldDrop(Loot loot)
    {
        loot.AddGold(loot.Gold * GoldBonus);
        return loot;
    }
}