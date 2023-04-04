using System;
using UnityEngine;

public abstract class Unit : MonoBehaviour, ITickable
{
    protected const int TicksForRegeneration = 4;

    public int Level => level;
    public string Name => name;
    public float MaxHealth => maxHealth;
    public float Health => currentHealth;
    public float AttackDamage => attackDamage;
    public float CritChance => critChance;
    public float CritDamage => critDamage;
    public float Regeneration => regeneration;
    public float GoldBonus => goldBonus;
    public bool IsAlive => isAlive;

    [SerializeField] protected string name;
    [SerializeField] protected int level;
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float regeneration;
    [SerializeField] protected float attackDamage;
    [SerializeField] protected float critChance;
    [SerializeField] protected float critDamage;
    [SerializeField] protected float goldBonus;
    protected float currentHealth;
    protected bool isAlive = true;
    protected int currentRegenerationTick;

    protected virtual void Start()
    {
        currentHealth = MaxHealth;
        
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
            currentHealth += Regeneration;
            if (Health >= MaxHealth) currentHealth = MaxHealth;
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
        
        currentHealth -= damage;
        if (Health <= 0)
        {
            currentHealth = 0;
            Death();
        }
    }

    protected virtual void Death()
    {
        isAlive = false;
        print($"{Name} died.");
    }
}