using TMPro;
using UnityEngine;

public class TimerDisplay : MonoBehaviour
{
    public GameProgressControl gameProgressControl;
    private TextMeshProUGUI countDownText;

    private void Awake()
    {
        EventBus.Subscribe<AssignGameControlEvent>(e => gameProgressControl = e.gameProgressControl);
        countDownText = GetComponent<TextMeshProUGUI>();
    }

    public void Update()
    {
        float timeElapsed = gameProgressControl.timeElapsed;
        int minutes = Mathf.FloorToInt(timeElapsed / 60);
        int seconds = Mathf.FloorToInt(timeElapsed % 60);

        countDownText.text = $"Time: {minutes}:{seconds:D2}";
    }
}