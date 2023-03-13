using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputInterface : MonoBehaviour
{
    private Controls controls;
    private Controls.PlayerActions playerActions;

    private void Awake()
    {
        controls = new Controls();
        playerActions = controls.Player;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Update()
    {
        if (playerActions.TriggerPause.WasPressedThisFrame())
        {
            EventBus.Publish(new TriggerPauseEvent());
        }

        if (playerActions.GenerateDialog.WasPressedThisFrame())
        {
            EventBus.Publish(new DisplayDialogEvent(
                "Chirp", "Something happened!",
                new Dictionary<string, UnityAction>()
                {
                    { "What?", () => print("Clicked what") },
                    { "How?", () => print("Clicked how") }
                }
            ));
        }

        if (playerActions.IncreaseSimulationSpeed.WasPressedThisFrame())
        {
            EventBus.Publish(new ScrollSimulationSpeedEvent(true));
        }

        if (playerActions.DecreaseSimulationSpeed.WasPressedThisFrame())
        {
            EventBus.Publish(new ScrollSimulationSpeedEvent(false));
        }

        if (playerActions.ToggleDemo.WasPressedThisFrame())
        {
            EventBus.Publish(new ToggleDemoEvent());
        }

        if (playerActions.SpawnCitizen.WasPressedThisFrame())
        {
            EventBus.Publish(new SpawnCitizenEvent());
        }
    }
}