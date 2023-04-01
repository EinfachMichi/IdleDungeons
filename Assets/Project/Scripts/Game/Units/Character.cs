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

    private float experience;
    private int deathCooldown => level * 30;
    private int deathTimer;

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
    
    protected override void Death()
    {
        base.Death();
        OnCharacterDeath?.Invoke();
    }
}
