using TMPro;
using UnityEngine;

public class CountDownDisplay : MonoBehaviour
{
    public GameProgressControl gameProgressControl;
    private TextMeshProUGUI countDownText;

    private void Awake()
    {
        EventBus.Subscribe<AssignGameControlEvent>(e => gameProgressControl = e.gameProgressControl);
        countDownText = GetComponent<TextMeshProUGUI>();
    }

    // public void Update()
    // {
    //     float timeDisplayed = gameProgressControl.timeRemaining + 1;
    //     int minutes = Mathf.FloorToInt(timeDisplayed / 60);
    //     int seconds = Mathf.FloorToInt(timeDisplayed % 60);
    //
    //     countDownText.text = $"Time: {minutes}:{seconds:D2}";
    // }
}