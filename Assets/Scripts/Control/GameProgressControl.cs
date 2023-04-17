using UnityEngine;

public class GameProgressControl : MonoBehaviour
{
    public static bool isGameActive;

    public float timeElapsed;

    private Sprite mouseGameImage;
    private Sprite keyboardGameImage;
    private bool isGamePaused;
    private bool isGameStarted;

    private bool isGameEnded;

    // private bool isEndDialogShown;
    private Vector3[] originalPos;

    private void Awake()
    {
        EventBus.Subscribe<ModifyPauseEvent>(e => isGamePaused = e.status);
        EventBus.Subscribe<GameStartEvent>(_ =>
        {
            GameState.ResetState();
            isGameStarted = true;
        });
        EventBus.Subscribe<StartBuilderTutorialEvent>(_ => isGameStarted = true);
        EventBus.Subscribe<GameEndEvent>(e =>
        {
            GameState.result = e.status;
            GameState.timePlayed = timeElapsed;
            SceneState.SetTransition(
                1, 2, StringPool.mainResultScene, mouseGameImage, keyboardGameImage
            );
            EventBus.Publish(new TransitSceneEvent());
        });
        EventBus.Subscribe<EndAllTutorialEvent>(_OnTutorialEnd);
        mouseGameImage = Resources.Load<Sprite>("Sprites/Background/MouseGame");
        keyboardGameImage = Resources.Load<Sprite>("Sprites/Background/KeyboardGame");
    }

    private void _OnTutorialEnd(EndAllTutorialEvent e)
    {
        if (isGameEnded)
        {
            return;
        }

        isGameEnded = true;
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
        // isEndDialogShown = false;
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
}