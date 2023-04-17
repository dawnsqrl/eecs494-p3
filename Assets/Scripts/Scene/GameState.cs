public static class GameState
{
    public static bool result; // builder = true, enemy = false
    public static bool isDraw;
    public static float timePlayed;

    // Mushroom
    public static int smallMushroomProduced;
    public static int smallSnailKilled;
    public static int myceliumProduced;
    public static int mucusDestroyed;
    public static int buildingPlaced;
    public static int nestDestroyed;
    public static int grassDestroyed;

    // Snail
    public static int smallSnailFound;
    public static int smallMushroomKilled;
    public static int mucusProduced;
    public static int myceliumDestroyed;
    public static int buildingDestroyed;
    public static int shieldUsed;
    public static int bombUsed;

    public static void ResetState()
    {
        result = false;
        isDraw = false;
        timePlayed = 0;
        smallMushroomProduced = 0;
        smallMushroomKilled = 0;
        smallSnailFound = 0;
        smallSnailKilled = 0;
        myceliumProduced = 0;
        myceliumDestroyed = 0;
        mucusProduced = 0;
        mucusDestroyed = 0;
        buildingPlaced = 0;
        buildingDestroyed = 0;
        nestDestroyed = 0;
        grassDestroyed = 0;
        shieldUsed = 0;
        bombUsed = 0;
    }
}