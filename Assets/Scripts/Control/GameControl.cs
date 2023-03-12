using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    public static bool isGameStarted = false;

    [SerializeField] private int timeDuration = 60;

    public float timeRemaining;
    private bool isEndDialogShown;
    private Vector3[] originalPos;

    private void Awake()
    {
        EventBus.Publish(new AssignGameControlEvent(this));
    }

    private void Start()
    {
        isEndDialogShown = false;
        // Setup countdown clock
        timeRemaining = timeDuration;
        StartCoroutine(StartInitialCountDown());
    }

    private void Update()
    {
        if (!isGameStarted)
        {
            return;
        }

        // check if time is up
        if (timeRemaining > 0)
        {
            if (!PauseControl.isPaused)
            {
                timeRemaining -= Time.deltaTime;
            }
        }
        else if (!isEndDialogShown)
        {
            // end game
            isGameStarted = false;
            isEndDialogShown = true;
            EventBus.Publish(new DisplayDialogEvent(
                "Time out!", "Game ended.",
                new Dictionary<string, UnityAction>()
                {
                    { "Restart", () => SceneManager.LoadScene(SceneManager.GetActiveScene().name) }
                }
            ));
        }
    }

    private IEnumerator StartInitialCountDown()
    {
        // used for get-ready countdown before actual game starts
        isGameStarted = true;
        yield return null;
    }
}