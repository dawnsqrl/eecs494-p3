using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameProgressControl : MonoBehaviour
{
    // [SerializeField] private int timeDuration = 60;

    public static bool isGameStarted;
    public static bool isGameEnded;

    // public float timeRemaining;

    // private bool isTimerActive;
    private bool isEndDialogShown;
    private Vector3[] originalPos;

    private void Awake()
    {
        // EventBus.Subscribe<ModifyPauseEvent>(e => isTimerActive = !e.status);
    }

    private void Start()
    {
        EventBus.Publish(new AssignGameControlEvent(this));
        isGameStarted = true;
        isGameEnded = false;
        // Setup countdown clock
        // timeRemaining = timeDuration;
        // isTimerActive = true;
        isEndDialogShown = false;
        // StartCoroutine(StartInitialCountDown());
    }

    private void Update()
    {
        if (!isGameStarted)
        {
            return;
        }

        // check if time is up
        // if (timeRemaining > 0)
        // {
        //     if (isTimerActive)
        //     {
        //         timeRemaining -= Time.deltaTime * SimulationSpeedControl.GetSimulationSpeed();
        //     }
        // }

        // TODO: set isGameEnded
        if (isGameEnded && !isEndDialogShown)
        {
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

    // private IEnumerator StartInitialCountDown()
    // {
    //     // used for get-ready countdown before actual game starts
    //     isGameStarted = true;
    //     yield return null;
    // }
}