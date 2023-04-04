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
        equipmentInventory = new EquipmentInventory();
        equipmentInventory.OnItemEquipped += AddStatsFromItem;
        equipmentInventory.OnItemUnequipped += RemoveStatsFromItem;
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
        currentHealth = MaxHealth;
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
        level++;
        experience = 0;
        maxExperience += Mathf.Floor(maxExperience / 2);
        maxHealth += lvlup_health;
        attackDamage += lvlup_damage;
        critChance += lvlup_critChance;
        critDamage += lvlup_critDamage;
        regeneration += lvlup_regeneration;
        
        Heal();
    }

    private void AddStatsFromItem(Item item) => ChangeStats(item, 1f);
    private void RemoveStatsFromItem(Item item) => ChangeStats(item, -1f);

    private void ChangeStats(Item item, float direction)
    {
        switch (item.ItemType)
        {
            case ItemType.Weapon:
                Weapon weapon = (Weapon) item;
                attackDamage += weapon.AttackDamge * direction;
                critDamage += weapon.CritDamage * direction;
                critChance += weapon.CritChance * direction;
                break;
            case ItemType.Armor:
                Armor armor = (Armor) item;
                maxHealth += armor.ExtraHealth * direction;
                regeneration += armor.ExtraRegeneration * direction;
                break;
            case ItemType.Relict:
                Relict relict = (Relict) item;
                goldBonus += relict.GoldBonus * direction;
                break;
        }
    }
    
    protected override void Death()
    {
        base.Death();
        OnCharacterDeath?.Invoke();
    }
}