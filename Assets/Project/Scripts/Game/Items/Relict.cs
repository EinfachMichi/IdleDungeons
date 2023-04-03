public class Relict : Item
{
    public float GoldBonus => buff.GoldBuff;

    private Buff buff;

    public Relict(RelictData data, Rarity rarity)
    {
        Name = data.Name;
        Rarity = rarity;

        buff = data.buff;

        ScaleWithRarity();
    }

    public override void ScaleWithRarity()
    {
        buff.MultiplyAllBuffs(rarityMultiplier);
    }
}
