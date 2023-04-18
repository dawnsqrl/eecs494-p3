using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultDisplay : MonoBehaviour
{
    [SerializeField] private Image mushroomMedal;
    [SerializeField] private Image snailMedal;
    [SerializeField] private TextMeshProUGUI result;
    [SerializeField] private TextMeshProUGUI timePlayed;
    [SerializeField] private TextMeshProUGUI smallMushroomProduced;
    [SerializeField] private TextMeshProUGUI smallSnailKilled;
    [SerializeField] private TextMeshProUGUI myceliumProduced;
    [SerializeField] private TextMeshProUGUI mucusDestroyed;
    [SerializeField] private TextMeshProUGUI buildingPlaced;
    [SerializeField] private TextMeshProUGUI nestDestroyed;
    [SerializeField] private TextMeshProUGUI grassDestroyed;
    [SerializeField] private TextMeshProUGUI smallSnailFound;
    [SerializeField] private TextMeshProUGUI smallMushroomKilled;
    [SerializeField] private TextMeshProUGUI mucusProduced;
    [SerializeField] private TextMeshProUGUI myceliumDestroyed;
    [SerializeField] private TextMeshProUGUI buildingDestroyed;
    [SerializeField] private TextMeshProUGUI shieldUsed;
    [SerializeField] private TextMeshProUGUI bombUsed;

    private AudioClip resultAudio;

    private readonly Color winColor = new Color(1, 0.8f, 0, 0.95f);
    private readonly Color loseColor = new Color(0.8f, 0.8f, 0.8f, 0.95f);

    private void Awake()
    {
        resultAudio = Resources.Load<AudioClip>("Audio/GameEnd");
    }

    private void Start()
    {
        if (GameState.isDraw)
        {
            result.text = "It's a draw!";
            mushroomMedal.color = loseColor;
            snailMedal.color = loseColor;
            timePlayed.text = $"{GameProgressControl.maxMinutesElapsed} minutes' up!";
        }
        else
        {
            if (GameState.result)
            {
                result.text = "Mushroom wins!";
                mushroomMedal.color = winColor;
                snailMedal.color = loseColor;
            }
            else
            {
                result.text = "Snail wins!";
                mushroomMedal.color = loseColor;
                snailMedal.color = winColor;
            }

            Debug.Log(GameState.timePlayed);
            float timePlayedValue = GameState.timePlayed;
            Debug.Log(timePlayedValue);
            int minutes = Mathf.FloorToInt(timePlayedValue / 60);
            int seconds = Mathf.FloorToInt(timePlayedValue % 60);
            timePlayed.text = $"You played for {minutes}:{seconds:D2}.";
        }

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

        StartCoroutine(DelayPlay());
    }

    private IEnumerator DelayPlay()
    {
        yield return new WaitForSeconds(0.5f);
        AudioSource.PlayClipAtPoint(resultAudio, AudioListenerManager.audioListenerPos, 0.7f);
    }
}