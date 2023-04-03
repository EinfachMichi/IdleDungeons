using System;
using UnityEngine;

public class Character : Unit
{
    public event Action OnCharacterDeath;

    public float GoldBonus => goldBonus;
    
    [SerializeField] private float maxExperience;
    [SerializeField] private float goldBonus;

    [Header("Levelup Stats")] 
    [SerializeField] private float lvlup_health = 1;
    [SerializeField] private float lvlup_damage = 0.5f;
    [SerializeField] private float lvlup_regeneration = 0.2f;
    [SerializeField] private float lvlup_critChance = 1f;
    [SerializeField] private float lvlup_critDamage = 10f;
    

    private float experience;
    private int deathCooldown => level * 30;
    private int deathTimer;
    private EquipmentSlots equipmentSlots;

    protected override void Start()
    {
        base.Start();
        equipmentSlots = new EquipmentSlots();
        equipmentSlots.OnItemEquipped += AddItemStats;
        equipmentSlots.OnItemUnequipped += RemoveItemStats;
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
        currentHealth = maxHealth;
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

    private void AddItemStats(Item item)
    {
        switch (item.ItemType)
        {
            case ItemType.Weapon:
                Weapon weapon = (Weapon) item;
                attackDamage += weapon.AttackDamge;
                critChance += weapon.CritChance;
                critDamage += weapon.CritDamage;
                break;
            case ItemType.Armor:
                Armor armor = (Armor) item;
                maxHealth += armor.ExtraHealth;
                regeneration += armor.ExtraRegeneration;
                break;
            case ItemType.Relict:
                Relict relict = (Relict) item;
                goldBonus += relict.GoldBonus;
                break;
        }
    }

    private void RemoveItemStats(Item item)
    {
        switch (item.ItemType)
        {
            case ItemType.Weapon:
                Weapon weapon = (Weapon) item;
                attackDamage -= weapon.AttackDamge;
                critChance -= weapon.CritChance;
                critDamage -= weapon.CritDamage;
                break;
            case ItemType.Armor:
                Armor armor = (Armor) item;
                maxHealth -= armor.ExtraHealth;
                regeneration -= armor.ExtraRegeneration;
                break;
            case ItemType.Relict:
                Relict relict = (Relict) item;
                goldBonus -= relict.GoldBonus;
                break;
        }
    }
    
    protected override void Death()
    {
        base.Death();
        OnCharacterDeath?.Invoke();
    }
}