public static class StringPool
{
    public static readonly string defaultDialogTitle = "Quack";
    public static readonly string defaultDialogContent = "Nothing happened!";
    public static readonly string defaultDialogButtonText = "Maybe?";

    public static readonly string previousButtonText = "Previous";
    public static readonly string nextButtonText = "Next";

    public static readonly string gameDescriptionText
        = @"Mycelium is a two-player RTS game.
One side is an expanding mushroom biome, and the other is a free-roaming snail.
Guide the mushroom to kill the snail, or lead the snail to destroy the mycelium!

(This tutorial will be improved in the future o.o)";

    public static readonly string generalControlList
        = @"[+/-] to increase / decrease game speed
[ESC] to pause";

    public static readonly string builderControlList
        = @"[MDrag] to drag map view
[C] to spawn small mushroom (white) at cursor
[LClick] the central mushroom, then [LClick] a grey fog tile to add a mycelium source
[LDrag] to select small mushrooms
[RClick] to set destination of small mushrooms";

    public static readonly string enemyControlList
        = @"[W/A/S/D] to control snail base movement
[K] to spawn small snail (red) at cursor";

    public static readonly string infestPromptBanner = "Click to infest the cherry";
}