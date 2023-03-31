using System;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon : MonoBehaviour, ITickable
{
    public event Action<Dungeon> OnDungeonDone;
    
    [HideInInspector] public Enemy Enemy;
    [HideInInspector] public Character Character;
    
    [SerializeField] private List<Enemy> enemies;
    private int currentEnemyIndex;

    public void StartDungeon(Character character)
    {
        InitCharacter(character);
        InitEnemy();
        
        GameManager.Instance.AddToRegister(this);
    }

    private void DungeonDone()
    {
        Character.gameObject.transform.SetParent(null);
        OnDungeonDone?.Invoke(this);
    }
    
    public void Tick()
    {
        HandleUnitTurn(Character, Enemy);
        HandleUnitTurn(Enemy, Character);
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
            DungeonDone();
            return;
        }
        
        Enemy = Instantiate(enemies[currentEnemyIndex]);
        Enemy.transform.SetParent(transform);
        currentEnemyIndex++;

        Enemy.OnUnitDeath += OnEnemyDeath;
    }

    private void InitCharacter(Character character)
    {
        Character = character;
        character.gameObject.transform.SetParent(transform);
        character.OnUnitDeath += OnCharacterDeath;
    }
    
    private void OnCharacterDeath(Unit unit)
    {
        // TODO: Dungeon Over
        DungeonDone();
    }

    private void OnEnemyDeath(Unit unit)
    {
        Destroy(unit.gameObject);
        InitEnemy();
    }
}