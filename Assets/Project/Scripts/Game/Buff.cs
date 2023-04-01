using System;
using UnityEngine;

[Serializable]
public struct Buff
{
    public float GoldBuff => goldBuff;
    
    [SerializeField] private float goldBuff;

    public void MultiplyAllBuffs(float multiplier)
    {
        goldBuff *= multiplier;
    }
}
