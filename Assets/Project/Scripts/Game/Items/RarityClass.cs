public static class RarityClass
{
    private static float[] chances =
    {
        0.5f,       //1f,      // COMMON      // 50%
        0.25f,      //0.5f,    // UNCOMMON    // 25%
        0.12f,      //0.25f,   // RARE        // 12%
        0.09f,      //0.13f,   // EPIC        // 9%
        0.025f,     //0.04f,   // LEGENDARY   // 2.5%
        0.01f,      //0.015f,  // MYTHIC      // 1%
        0.005f,     //0.005f   // GODLIKE     // 0.5%
    };
    
    private static float[] rarityMultiplier = 
    {
        1f, // COMMON
        1.2f, // UNCOMMON
        1.5f, // RARE
        2f, // EPIC
        5f, // LEGENDARY
        10f, // MYTHIC
        25f // GODLIKE
    };

    public static int Length => chances.Length;
    public static float[] RarityChances => chances;
    public static float[] RarityMultiplier => rarityMultiplier;
}