using UnityEngine;

public class PauseControl : MonoBehaviour
{
    public static bool isPaused;

    private void Awake()
    {
        EventBus.Subscribe<TogglePauseEvent>(_OnTogglePause);
    }

    private void Start()
    {
        isPaused = false;
    }

    private void _OnTogglePause(TogglePauseEvent _)
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
    }
}