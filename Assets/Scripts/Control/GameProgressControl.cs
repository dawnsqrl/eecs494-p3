using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameProgressControl : MonoBehaviour
{
    public static bool isGameActive;

    public float timeElapsed;

    private Sprite mouseGameImage;
    private Sprite keyboardGameImage;
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
        mouseGameImage = Resources.Load<Sprite>("Sprites/Background/MouseGame");
        keyboardGameImage = Resources.Load<Sprite>("Sprites/Background/KeyboardGame");
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
                        "Return", new Tuple<UnityAction, bool>(
                            () =>
                            {
                                SceneState.SetTransition(
                                    1, 2, "MainMenu", mouseGameImage, keyboardGameImage
                                );
                                EventBus.Publish(new TransitSceneEvent());
                            }, true
                        )
                    },
                    {
                        "Restart", new Tuple<UnityAction, bool>(
                            () =>
                            {
                                SceneState.SetTransition(
                                    1, 0, "MainGame", mouseGameImage, keyboardGameImage
                                );
                                EventBus.Publish(new TransitSceneEvent());
                            }, true
                        )
                    }
                }
            ));
        }
    }

    //void LoadAllDisplay() {
    //    DisplayTest displayTest = FindObjectOfType<DisplayTest>();
    //    displayTest.LoadSceneOnAllDisplays(2);
    //}
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