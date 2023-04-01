public class Armor : Item
{
    public float ExtraHealth => extraHealth;
    public float ExtraRegeneration => extraRegeneration;
    
    private float extraHealth;
    private float extraRegeneration;

    public Armor(ArmorData data, Rarity rarity)
    {
        Name = data.name;
        Rarity = rarity;
        
        ScaleWithRarity();
    }
    
    public override void ScaleWithRarity()
    {
        float multiplier = GetRarityMultiplier((int) Rarity);
        extraHealth *= multiplier;
        extraRegeneration *= multiplier;
    }
}
