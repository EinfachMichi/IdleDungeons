using System;
using UnityEngine;

public abstract class Unit : MonoBehaviour, ITickable
{
    public event Action<Unit> OnUnitDeath;
    
    private const int TicksForRegeneration = 4;

    public string Name => name;
    public int Level => level;
    public float MaxHealth => maxHealth;
    public float Health => currentHealth;
    public float Damage => damage;
    public float Regeneration => regeneration;
    public bool IsAlive => isAlive;

    [SerializeField] private new string name;
    [SerializeField] private int level;
    [SerializeField] private float maxHealth;
    [SerializeField] private float damage;
    [SerializeField] private float regeneration;
    private float currentHealth;
    private bool isAlive = true;
    
    private float timer;
    private int currentRegenerationTick;

    private void Start()
    {
        currentHealth = MaxHealth;
        
        GameManager.Instance.AddToRegister(this);
    }

    public void Tick()
    {
        if (isAlive)
        {
            if (currentRegenerationTick >= TicksForRegeneration)
            {
                currentHealth += Regeneration;
                if (currentHealth >= MaxHealth) currentHealth = MaxHealth;
                currentRegenerationTick = 0;
            }
        }
        else
        {
            // TODO: Add cooldown
        }

        currentRegenerationTick++;
    }

    public void ApplyDamage(Unit target)
    {
        if (!isAlive) return;
        
        target.ReceiveDamage(Damage);
    }

    public float ReceiveDamage(float damage)
    {
        if (!isAlive) return 0;
        
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Death();
        }
        return currentHealth;
    }

    private void Death()
    {
        isAlive = false;
        OnUnitDeath?.Invoke(this);
    }
}
