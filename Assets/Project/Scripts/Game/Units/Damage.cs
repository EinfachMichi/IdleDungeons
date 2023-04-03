using Random = UnityEngine.Random;

public static class Damage
{
    public static float RealDamage(Unit user)
    {
        float chanceToCrit = Random.Range(0f, 100f);

        if (user.CritChance <= chanceToCrit)
            return user.AttackDamage * user.CritDamage / 100f;

        return user.AttackDamage;
    }
}