using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameProgressControl : MonoBehaviour
{
    [SerializeField] private int timeDuration = 60;

    public static bool isGameStarted = false;
    public static bool isGameEnded = false;

    public float timeRemaining;

    private bool isTimerActive;
    private bool isEndDialogShown;
    private Vector3[] originalPos;

    private void Awake()
    {
        EventBus.Subscribe<ModifyPauseEvent>(e => isTimerActive = !e.status);
        EventBus.Publish(new AssignGameControlEvent(this));
    }

    private void Start()
    {
        // Setup countdown clock
        timeRemaining = timeDuration;
        isTimerActive = true;
        isEndDialogShown = false;
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
            if (isTimerActive)
            {
                timeRemaining -= Time.deltaTime;
            }
        }
        else if (!isEndDialogShown)
        {
            isGameEnded = true;
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