using System;
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
                "Game paused!", "Take a rest.",
                new Dictionary<string, Tuple<UnityAction, bool>>()
                {
                    { "Resume", new Tuple<UnityAction, bool>(() => SetPauseState(false), true) },
                    {
                        "Restart", new Tuple<UnityAction, bool>(
                            () => SceneManager.LoadScene(SceneManager.GetActiveScene().name), true
                        )
                    }
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