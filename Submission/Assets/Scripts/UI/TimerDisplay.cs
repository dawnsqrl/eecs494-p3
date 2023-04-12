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

    private void Start()
    {
        countDownText.enabled = false;
    }

    public void Update()
    {
        if (!countDownText.enabled && GameProgressControl.isGameActive)
        {
            countDownText.enabled = true;
        }

        float timeElapsed = gameProgressControl.timeElapsed;
        int minutes = Mathf.FloorToInt(timeElapsed / 60);
        int seconds = Mathf.FloorToInt(timeElapsed % 60);

        countDownText.text = $"Time: {minutes}:{seconds:D2}";
    }
}