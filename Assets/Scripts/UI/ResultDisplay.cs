using TMPro;
using UnityEngine;

public class ResultDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI result;
    [SerializeField] TextMeshProUGUI timePlayed;
    [SerializeField] TextMeshProUGUI smallMushroomProduced;
    [SerializeField] TextMeshProUGUI smallSnailKilled;
    [SerializeField] TextMeshProUGUI myceliumProduced;
    [SerializeField] TextMeshProUGUI mucusDestroyed;
    [SerializeField] TextMeshProUGUI buildingPlaced;
    [SerializeField] TextMeshProUGUI nestDestroyed;
    [SerializeField] TextMeshProUGUI grassDestroyed;
    [SerializeField] TextMeshProUGUI smallSnailFound;
    [SerializeField] TextMeshProUGUI smallMushroomKilled;
    [SerializeField] TextMeshProUGUI mucusProduced;
    [SerializeField] TextMeshProUGUI myceliumDestroyed;
    [SerializeField] TextMeshProUGUI buildingDestroyed;
    [SerializeField] TextMeshProUGUI shieldUsed;
    [SerializeField] TextMeshProUGUI bombUsed;

    private void Start()
    {
        string winnerTag = GameState.result ? "Mushroom" : "Snail";
        result.text = $"{winnerTag} wins!";

        float timePlayedValue = GameState.timePlayed;
        int minutes = Mathf.FloorToInt(timePlayedValue / 60);
        int seconds = Mathf.FloorToInt(timePlayedValue % 60);
        timePlayed.text = $"You played for {minutes}:{seconds:D2}.";

        smallMushroomProduced.text = GameState.smallMushroomProduced.ToString();
        smallSnailKilled.text = GameState.smallSnailKilled.ToString();
        myceliumProduced.text = GameState.myceliumProduced.ToString();
        mucusDestroyed.text = GameState.mucusDestroyed.ToString();
        buildingPlaced.text = GameState.buildingPlaced.ToString();
        nestDestroyed.text = GameState.nestDestroyed.ToString();
        grassDestroyed.text = GameState.grassDestroyed.ToString();
        smallSnailFound.text = GameState.smallSnailFound.ToString();
        smallMushroomKilled.text = GameState.smallMushroomKilled.ToString();
        mucusProduced.text = GameState.mucusProduced.ToString();
        myceliumDestroyed.text = GameState.myceliumDestroyed.ToString();
        buildingDestroyed.text = GameState.buildingDestroyed.ToString();
        shieldUsed.text = GameState.shieldUsed.ToString();
        bombUsed.text = GameState.bombUsed.ToString();
    }
}