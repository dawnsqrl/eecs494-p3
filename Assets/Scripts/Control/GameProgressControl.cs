using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameProgressControl : MonoBehaviour
{
    public static bool isGameActive;

    public float timeElapsed;

    private bool isGamePaused;
    private bool isGameStarted;
    private bool isGameEnded;
    private bool isEndDialogShown;
    private Vector3[] originalPos;

    private void Awake()
    {
        EventBus.Subscribe<ModifyPauseEvent>(e => isGamePaused = e.status);
        EventBus.Subscribe<GameStartEvent>(_ => isGameStarted = true);
        EventBus.Subscribe<GameEndEvent>(_OnGameEnd);
    }

    private void _OnGameEnd(GameEndEvent e)
    {
        if (isGameEnded)
        {
            return;
        }

        isGameEnded = true;
        if (!isEndDialogShown)
        {
            isEndDialogShown = true;
            string winnerTag = e.status ? "Mushroom" : "Snail";
            int minutes = Mathf.FloorToInt(timeElapsed / 60);
            int seconds = Mathf.FloorToInt(timeElapsed % 60);
            EventBus.Publish(new DisplayDialogEvent(
                "Game ended!",
                $"{winnerTag} wins!\nYou played for {minutes}:{seconds:D2}.",
                new Dictionary<string, Tuple<UnityAction, bool>>()
                {
                    {
                        "Restart", new Tuple<UnityAction, bool>(
                            () => SceneManager.LoadScene(SceneManager.GetActiveScene().name), true
                        )
                    }
                }
            ));
        }
    }

    private void Start()
    {
        EventBus.Publish(new AssignGameControlEvent(this));
        isGameActive = false;
        // Setup countdown clock
        timeElapsed = 0;
        isGamePaused = false;
        isGameStarted = false;
        isGameEnded = false;
        isEndDialogShown = false;
        // StartCoroutine(StartInitialCountDown());
    }

    private void Update()
    {
        isGameActive = isGameStarted && !(isGamePaused || isGameEnded);
        if (!isGameActive)
        {
            return;
        }

        if (!isGamePaused)
        {
            timeElapsed += Time.deltaTime * SimulationSpeedControl.GetSimulationSpeed();
        }
    }

    // private IEnumerator StartInitialCountDown()
    // {
    //     // used for get-ready countdown before actual game starts
    //     isGameStarted = true;
    //     yield return null;
    // }
}