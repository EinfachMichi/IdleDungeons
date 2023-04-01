using System;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon : MonoBehaviour, ITickable
{
    public event Action<Dungeon> OnDungeonDone;

    public Enemy Enemy => enemy;
    public Character Character => character;
    public int Difficulty => difficulty;
    
    [SerializeField] private int difficulty;
    [SerializeField] private List<Enemy> enemies;
    [SerializeField] private LootTable lootTable;

    private int currentEnemyIndex;
    private Enemy enemy;
    private Character character;

    public void StartDungeon(Character character)
    {
        InitCharacter(character);
        InitEnemy();
        
        GameManager.Instance.AddToRegister(this);
    }
    
    public void Tick()
    {
        HandleUnitTurn(character, enemy);
        HandleUnitTurn(enemy, character);
    }

    private void DungeonComplete()
    {
        GameManager.Instance.AddGold(lootTable.GoldDrop());
        GameManager.Instance.AddLoot(lootTable.ItemDrop());
        print("Dungeon Complete!");
        
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
        enemy.ScaleByLevel(difficulty);
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
        GameManager.Instance.AddGold(enemy.GoldDrop);
        Destroy(enemy.gameObject);
        InitEnemy();
    }
}