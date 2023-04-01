public enum Rarity
{
    Common = 0,
    Uncommon = 1,
    Rare = 2,
    Epic = 3,
    Legendary = 4,
    Mythical = 5,
    Godlike = 6
}

public static class RarityClass
{
    private static float[] chances =
    {
        1f,      // COMMON           // 50%
        0.5f,    // UNCOMMON         // 25%
        0.25f,   // RARE             // 12%
        0.13f,   // EPIC             // 9%
        0.04f,   // LEGENDARY        // 2.5%
        0.015f,  // MYTHIC           // 1%
        0.005f   // GODLIKE          // 0.5%
    };

    public static int Length => chances.Length;
    public static float[] RarityChances => chances;
}