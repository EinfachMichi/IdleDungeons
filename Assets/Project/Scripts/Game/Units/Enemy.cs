using System;
using Mono.Cecil;
using UnityEngine;

public class Enemy : Unit
{
    public event Action<Enemy> OnEnemyDeath;

    public float ExperienceDrop => experienceDrop;
    public float GoldDrop => goldDrop;
    
    [SerializeField] private float experienceDrop;
    [SerializeField] private float goldDrop;

    protected override void Death()
    {
        base.Death();
        OnEnemyDeath?.Invoke(this);
    }

    public void ScaleByLevel(int level)
    {
        this.level = level;
        maxHealth *= level;
        attackDamage *= level;
        critChance += level;
        critDamage *= level;
        regeneration *= level;
        goldDrop *= level;
        experienceDrop *= level;
    }
}
