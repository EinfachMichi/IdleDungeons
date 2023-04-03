using System;
using UnityEngine;

public abstract class Unit : MonoBehaviour, ITickable
{
    protected const int TicksForRegeneration = 4;

    public int Level
    {
        get => stats.level;
        protected set => stats.level = value;
    }
    public string Name
    {
        get => stats.name;
        protected set => stats.name = value;
    }
    public float MaxHealth
    {
        get => stats.maxHealth;
        protected set => stats.maxHealth = value;
    }
    public float Health
    {
        get => stats.currentHealth;
        protected set => stats.currentHealth = value;
    }
    public float AttackDamage
    {
        get => stats.attackDamage;
        protected set => stats.attackDamage = value;
    }
    public float CritChance
    {
        get => stats.critChance;
        protected set => stats.critChance = value;
    }
    public float CritDamage
    {
        get => stats.critDamage;
        protected set => stats.critDamage = value;
    }
    public float Regeneration
    {
        get => stats.regeneration;
        protected set => stats.regeneration = value;
    }
    public float GoldBonus
    {
        get => stats.goldBonus;
        protected set => stats.goldBonus = value;
    }
    public bool IsAlive => isAlive;

    [SerializeField] protected UnitStats stats;
    
    protected bool isAlive = true;
    protected int currentRegenerationTick;

    protected virtual void Start()
    {
        Health = MaxHealth;
        
        GameManager.Instance.AddToRegister(this);
    }

    public virtual void Tick()
    {
        if (!isAlive) return;
        
        Regenerate();
    }

    private void Regenerate()
    {
        if (currentRegenerationTick >= TicksForRegeneration)
        {
            Health += Regeneration;
            if (Health >= MaxHealth) Health = MaxHealth;
            currentRegenerationTick = 0;
        }
        currentRegenerationTick++;
    }
    
    public void ApplyDamage(Unit target)
    {
        if (!isAlive) return;

        target.ReceiveDamage(Damage.RealDamage(this));
    }

    public void ReceiveDamage(float damage)
    {
        if (!isAlive) return;
        
        Health -= damage;
        if (Health <= 0)
        {
            Health = 0;
            Death();
        }
    }

    protected virtual void Death()
    {
        isAlive = false;
        print($"{Name} died.");
    }
}

[Serializable]
public struct UnitStats
{
    public string name;
    public int level;
    public float maxHealth;
    public float regeneration;
    public float attackDamage;
    public float critChance;
    public float critDamage;
    public float currentHealth;
    public float goldBonus;

    public void AddStats(Item item)
    {
        ChangeStats(item, 1f);
    }

    public void RemoveStats(Item item)
    {
        ChangeStats(item, -1f);
    }

    private void ChangeStats(Item item, float direction)
    {
        switch (item.ItemType)
        {
            case ItemType.Weapon:
                Weapon weapon = (Weapon) item;
                attackDamage += weapon.AttackDamge * direction;
                critChance += weapon.CritChance * direction;
                critDamage += weapon.CritDamage * direction;
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
}