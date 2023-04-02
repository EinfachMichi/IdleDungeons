using UnityEngine;

public abstract class Unit : MonoBehaviour, ITickable
{
    protected const int TicksForRegeneration = 4;

    public string Name => name;
    public int Level => level;
    public float MaxHealth => maxHealth;
    public float Health => currentHealth;
    public float Damage => damage;
    public float Regeneration => regeneration;
    public float CritChance => critChance;
    public float CritDamage => critDamage;
    public bool IsAlive => isAlive;
    
    [SerializeField] protected new string name;
    [SerializeField] protected int level;
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float damage;
    [SerializeField] protected float regeneration;
    [SerializeField] protected float critChance;
    [SerializeField] protected float critDamage;
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
        
        if (currentRegenerationTick >= TicksForRegeneration)
        {
            currentHealth += Regeneration;
            if (currentHealth >= MaxHealth) currentHealth = MaxHealth;
            currentRegenerationTick = 0;
        }
        
        currentRegenerationTick++;
    }

    public void ApplyDamage(Unit target)
    {
        if (!isAlive) return;

        float realDamage = Damage;
        float crit = Random.Range(0f, 100f);
        if (critChance <= crit)
        {
            realDamage *= critDamage / 100f;
        }
        target.ReceiveDamage(realDamage);
    }

    public float ReceiveDamage(float damage)
    {
        if (!isAlive) return 0;
        
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Death();
        }
        return currentHealth;
    }

    protected virtual void Death()
    {
        isAlive = false;
        print($"{Name} died.");
    }
}
