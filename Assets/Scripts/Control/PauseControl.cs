using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PauseControl : MonoBehaviour
{
    private bool isPaused;
    private bool isDialogBlocking;

    private void Awake()
    {
        EventBus.Subscribe<DialogBlockingEvent>(e => isDialogBlocking = e.status);
        EventBus.Subscribe<TriggerPauseEvent>(_OnTriggerPause);
    }

    private void Start()
    {
        isPaused = false;
        isDialogBlocking = false;
    }

    private void _OnTriggerPause(TriggerPauseEvent _)
    {
        if (!isPaused && GameProgressControl.isGameActive && !isDialogBlocking)
        {
            SetPauseState(true);
            EventBus.Publish(new DisplayDialogEvent(
                "Game paused!", "Take a rest.", Vector2.zero,
                new Dictionary<string, UnityAction>()
                {
                    { "Resume", () => SetPauseState(false) },
                    { "Restart", () => SceneManager.LoadScene(SceneManager.GetActiveScene().name) }
                }
            ));
        }
    }

    private void SetPauseState(bool status)
    {
        isPaused = status;
        EventBus.Publish(new ModifyPauseEvent(isPaused));
    }
}