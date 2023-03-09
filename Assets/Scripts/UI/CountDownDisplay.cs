using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR;

public class CountDownDisplay : MonoBehaviour
{
    public GameControl gameControl;
    private TextMeshProUGUI countDownText;
    // Start is called before the first frame update
    private void Awake()
    {
        EventBus.Subscribe<AssignGameControl>(e => gameControl = e.gameControl);
    }

    void Start()
    {
        countDownText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        float timeDisplayed = gameControl.timeRemaining + 1;
        float minutes = Mathf.FloorToInt(timeDisplayed / 60);
        float seconds = Mathf.FloorToInt(timeDisplayed % 60);

        countDownText.text = $"{minutes}:{seconds}";
    }
}