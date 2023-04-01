public abstract class Item
{
    private readonly float[] rarityMultiplier = 
    {
        1f, // COMMON
        1.2f, // UNCOMMON
        1.5f, // RARE
        2f, // EPIC
        5f, // LEGENDARY
        10f, // MYTHIC
        25f // GODLIKE
    };

    
    public string Name;
    public Rarity Rarity;
    public ItemType ItemType;

    public float GetRarityMultiplier(int rarity)
    {
        return rarityMultiplier[rarity];
    }
    
    public abstract void ScaleWithRarity();
}
