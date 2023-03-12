using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PauseControl : MonoBehaviour
{
    public static bool isPaused;

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
        if (!isPaused && GameControl.isGameStarted)
        {
            isPaused = true;
            EventBus.Publish(new DisplayDialogEvent(
                "Paused!", "Paused.",
                new Dictionary<string, UnityAction>()
                {
                    { "Resume", () => isPaused = false }
                }
            ));
        }
    }
}