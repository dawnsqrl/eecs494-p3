using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameProgressControl : MonoBehaviour
{
    // [SerializeField] private int timeDuration = 60;

    public static bool isGameActive;

    public float timeElapsed;

    private bool isTimerActive;
    private bool isGameStarted;
    private bool isGameEnded;
    private bool isEndDialogShown;
    private Vector3[] originalPos;

    private void Awake()
    {
        EventBus.Subscribe<ModifyPauseEvent>(e => isTimerActive = !e.status);
        EventBus.Subscribe<GameStartEvent>(_ => isGameStarted = true);
        EventBus.Subscribe<GameEndEvent>(_ => isGameEnded = true);
    }

    private void Start()
    {
        EventBus.Publish(new AssignGameControlEvent(this));
        isGameActive = false;
        // Setup countdown clock
        timeElapsed = 0;
        isTimerActive = true;
        isGameStarted = false;
        isGameEnded = false;
        isEndDialogShown = false;
        // StartCoroutine(StartInitialCountDown());
    }

    private void Update()
    {
        isGameActive = isGameStarted && !isGameEnded;
        if (!isGameActive)
        {
            return;
        }

        if (isTimerActive)
        {
            timeElapsed += Time.deltaTime * SimulationSpeedControl.GetSimulationSpeed();
        }

        // TODO: set isGameEnded
        if (isGameEnded && !isEndDialogShown)
        {
            isEndDialogShown = true;
            int minutes = Mathf.FloorToInt(timeElapsed / 60);
            int seconds = Mathf.FloorToInt(timeElapsed % 60);
            EventBus.Publish(new DisplayDialogEvent(
                "Game ended!", $"You played for {minutes}:{seconds:D2}.", Vector2.zero,
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