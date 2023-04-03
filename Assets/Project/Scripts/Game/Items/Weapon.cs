public class Weapon : Item
{
    public float AttackDamge => attackDamage;
    public float CritChance => critChance;
    public float CritDamage => critDamage;

    private float attackDamage;
    private float critChance;
    private float critDamage;

    public Weapon(WeaponData data, Rarity rarity)
    {
        Name = data.Name;
        Rarity = rarity;

        attackDamage = data.attackDamage;
        critChance = data.critChance;
        critDamage = data.critDamage;

        ScaleWithRarity();
    }

    public override void ScaleWithRarity()
    {
        attackDamage *= rarityMultiplier;
        critChance *= rarityMultiplier;
        critDamage *= rarityMultiplier;
    }
}