public abstract class Item
{
    protected float rarityMultiplier => RarityClass.RarityMultiplier[(int) Rarity];
    
    public string Name;
    public Rarity Rarity;
    public ItemType ItemType;

    public abstract void ScaleWithRarity();
}
