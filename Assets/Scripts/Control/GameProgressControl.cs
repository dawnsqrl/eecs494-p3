using System.Collections;
using UnityEngine;

public class GameProgressControl : MonoBehaviour
{
    public static bool isGameActive;
    public readonly static float maxMinutesElapsed = 20;

    public float timeElapsed;

    private Sprite mouseGameImage;
    private Sprite keyboardGameImage;
    private bool isGamePaused;
    private bool isGameStarted;
    private bool isGameEnded;
    private readonly float timeScale = 1.2f;
    private float maxTimeElapsed;

    private bool has2minHintShown;
    private bool has30secHintShown;
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
            GameState.isDraw = e.isDraw;
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
        timeElapsed = 0;
        isGamePaused = false;
        isGameStarted = false;
        isGameEnded = false;
        maxTimeElapsed = maxMinutesElapsed * 60;
        has2minHintShown = false;
        has30secHintShown = false;
        Time.timeScale = timeScale;
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
            timeElapsed += Time.deltaTime / timeScale;
        }

        if (!has2minHintShown && timeElapsed > maxTimeElapsed - 120)
        {
            has2minHintShown = true;
            string hurryText = "<size=+8><b>Hurry up! 2 minutes left!";
            EventBus.Publish(new DisplayHintEvent(0, hurryText));
            EventBus.Publish(new DisplayHintEvent(1, hurryText));
            StartCoroutine(DelayDismiss());
        }

        if (!has30secHintShown && timeElapsed > maxTimeElapsed - 30)
        {
            has30secHintShown = true;
            string hurryText = "<size=+8><b>Hurry up! 30 seconds left!";
            EventBus.Publish(new DisplayHintEvent(0, hurryText));
            EventBus.Publish(new DisplayHintEvent(1, hurryText));
            StartCoroutine(DelayDismiss());
        }

        if (timeElapsed > maxTimeElapsed)
        {
            EventBus.Publish(new GameEndEvent(false, true));
        }
    }

    private IEnumerator DelayDismiss()
    {
        yield return new WaitForSeconds(6);
        EventBus.Publish(new DismissHintEvent(0));
        EventBus.Publish(new DismissHintEvent(1));
    }
}