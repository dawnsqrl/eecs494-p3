using TMPro;
using UnityEngine;

public class CountDownDisplay : MonoBehaviour
{
    public GameControl gameControl;
    private TextMeshProUGUI countDownText;

    private void Awake()
    {
        EventBus.Subscribe<AssignGameControlEvent>(e => gameControl = e.gameControl);
    }

    public void Start()
    {
        countDownText = GetComponent<TextMeshProUGUI>();
    }

    public void Update()
    {
        float timeDisplayed = gameControl.timeRemaining + 1;
        float minutes = Mathf.FloorToInt(timeDisplayed / 60);
        float seconds = Mathf.FloorToInt(timeDisplayed % 60);

        countDownText.text = $"{minutes}:{seconds}";
    }
}