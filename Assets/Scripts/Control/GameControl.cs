using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameControl : MonoBehaviour
{
    [SerializeField] private int timeDuration = 60;
    public float timeRemaining;
    private bool gameEnded;
    private Vector3[] originalPos;
    private bool gameStarted = false;

    private void Awake()
    {
        EventBus.Publish(new AssignGameControlEvent(this));
    }

    void Start()
    {
        gameEnded = false;
        // Setup countdown clock
        timeRemaining = timeDuration;
        Time.timeScale = 1;
        StartCoroutine(StartCountDown());

    }
    // Update is called once per frame
    void Update()
    {
        if (!gameStarted)
        {
            return;
        }
        // check if time is up
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }
        else
        {
            // end game
            Time.timeScale = 0;
            gameEnded = true;
        }
    }

    IEnumerator StartCountDown()
    {
        gameStarted = true;
        yield return null;
    }
}
