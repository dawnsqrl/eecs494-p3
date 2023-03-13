using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PauseControl : MonoBehaviour
{
    private bool isPaused;

    private void Awake()
    {
        EventBus.Subscribe<TriggerPauseEvent>(_OnTriggerPause);
    }

    private void Start()
    {
        isPaused = false;
    }

    private void _OnTriggerPause(TriggerPauseEvent _)
    {
        if (!isPaused && GameProgressControl.isGameStarted)
        {
            SetPausedStatus(true);
            EventBus.Publish(new DisplayDialogEvent(
                "Paused!", "Paused.",
                new Dictionary<string, UnityAction>()
                {
                    { "Resume", () => SetPausedStatus(false) },
                    { "Restart", () => SceneManager.LoadScene(SceneManager.GetActiveScene().name) }
                }
            ));
        }
    }

    private void SetPausedStatus(bool status)
    {
        isPaused = status;
        EventBus.Publish(new ModifyPauseEvent(isPaused));
    }
}