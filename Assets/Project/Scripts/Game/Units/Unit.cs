using UnityEngine;

public abstract class Unit : MonoBehaviour, ITickable
{
    protected const int TicksForRegeneration = 4;

    public string Name => name;
    public int Level => level;
    public float MaxHealth => maxHealth;
    public float AttackDamage => attackDamage;
    public float CritChance => critChance;
    public float CritDamage => critDamage;
    public float Health => currentHealth;
    public float Regeneration => regeneration;
    public bool IsAlive => isAlive;
    
    [SerializeField] protected new string name;
    [SerializeField] protected int level;
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float regeneration;
    [SerializeField] protected float attackDamage;
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
        
        Regenerate();
    }

    private void Regenerate()
    {
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

        target.ReceiveDamage(Damage.RealDamage(this));
    }

    public void ReceiveDamage(float damage)
    {
        if (!isAlive) return;
        
        currentHealth -= damage;
        if (currentHealth <= 0)
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
