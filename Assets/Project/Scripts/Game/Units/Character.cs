using System;
using UnityEngine;

public class Character : Unit
{
    public event Action OnCharacterDeath;

    public float GoldBonus => goldBonus / 100f;
    
    [SerializeField] private float maxExperience;
    [SerializeField] private float goldBonus;
    [SerializeField] private int equipmentSlotsSize;
    
    [Header("Levelup Stats")] 
    [SerializeField] private float lvlup_health = 1;
    [SerializeField] private float lvlup_damage = 0.5f;
    [SerializeField] private float lvlup_regeneration = 0.2f;
    

    private float experience;
    private int deathCooldown => level * 30;
    private int deathTimer;
    private EquipmentSlots equipmentSlots;

    protected override void Start()
    {
        base.Start();
        equipmentSlots = new EquipmentSlots(equipmentSlotsSize);
        equipmentSlots.OnItemEquipped += AddItemStats;
        equipmentSlots.OnItemUnequipped += RemoveItemStats;
    }
    
    public override void Tick()
    {
        base.Tick();
        if (!isAlive)
        {
            deathTimer++;
            if (deathTimer >= deathCooldown)
            {
                Heal();
                deathTimer = 0;
            }
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
        damage += lvlup_damage;
        regeneration += lvlup_regeneration;
        
        Heal();
    }

    private void AddItemStats(Item item)
    {
        switch (item.ItemType)
        {
            case ItemType.Weapon:
                Weapon weapon = (Weapon) item;
                damage += weapon.AttackDamge;
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
                damage -= weapon.AttackDamge;
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
