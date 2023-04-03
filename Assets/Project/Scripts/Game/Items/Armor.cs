public class Armor : Item
{
    public float ExtraHealth => extraHealth;
    public float ExtraRegeneration => extraRegeneration;
    public ArmorType ArmorType => armorType;
    
    private float extraHealth;
    private float extraRegeneration;
    private ArmorType armorType;

    public Armor(ArmorData data, Rarity rarity)
    {
        Name = data.name;
        Rarity = rarity;

        extraHealth = data.extraHealth;
        extraRegeneration = data.extraRegeneration;
        armorType = data.armorType;
        
        ScaleWithRarity();
    }
    
    public override void ScaleWithRarity()
    {
        extraHealth *= rarityMultiplier;
        extraRegeneration *= rarityMultiplier;
    }
}