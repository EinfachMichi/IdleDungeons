using System;
using UnityEngine;

public class Character : Unit
{
    public event Action OnCharacterDeath;

    [SerializeField] private float maxExperience;

    [Header("Levelup Stats")] 
    [SerializeField] private float lvlup_health = 1;
    [SerializeField] private float lvlup_damage = 0.5f;
    [SerializeField] private float lvlup_regeneration = 0.2f;
    [SerializeField] private float lvlup_critChance = 1f;
    [SerializeField] private float lvlup_critDamage = 10f;
    
    private float experience;
    private int deathCooldown => Level * 30;
    private int deathTimer;

    private EquipmentInventory equipmentInventory;

    protected override void Start()
    {
        base.Start();
        equipmentInventory.OnItemEquipped += stats.AddStats;
        equipmentInventory.OnItemUnequipped += stats.RemoveStats;
    }

    public override void Tick()
    {
        base.Tick();
        if (!isAlive) DeathTime();
    }

    private void DeathTime()
    {
        deathTimer++;
        if (deathTimer >= deathCooldown)
        {
            Heal();
            deathTimer = 0;
        }
    }

    public void Heal()
    {
        Health = MaxHealth;
        isAlive = true;
    }

    public void AddExperience(float value)
    {
        experience += value;
        if (experience >= maxExperience)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        Level++;
        experience = 0;
        maxExperience += Mathf.Floor(maxExperience / 2);
        MaxHealth += lvlup_health;
        AttackDamage += lvlup_damage;
        CritChance += lvlup_critChance;
        CritDamage += lvlup_critDamage;
        Regeneration += lvlup_regeneration;
        
        Heal();
    }

    protected override void Death()
    {
        base.Death();
        OnCharacterDeath?.Invoke();
    }
}