public class Relict : Item
{
    public Buff Buff => buff;
    
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
        float multiplier = GetRarityMultiplier((int) Rarity);
        buff.MultiplyAllBuffs(multiplier);
    }
}
