using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

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
                new Dictionary<string, Tuple<UnityAction, bool>>()
                {
                    { "What?", new Tuple<UnityAction, bool>(() => print("Clicked what"), true) },
                    { "How?", new Tuple<UnityAction, bool>(() => print("Clicked how"), true) }
                }
            ));
        }

        if (playerActions.GeneratePopup.WasPressedThisFrame())
        {
            EventBus.Publish(new DisplayPopupEvent(
                false, Mouse.current.position.ReadValue(), 1,
                Color.cyan, new Dictionary<string, UnityAction>()
                {
                    { "Sprites/pngegg", () => print("Clicked 1") },
                    { "", () => print("Clicked 2") },
                    { "Sprites/null", () => print("Clicked 3") }
                }
            ));
        }

        // if (playerActions.GenerateBanner.WasPressedThisFrame())
        // {
        //     EventBus.Publish(new DisplayBannerEvent(
        //         false, Mouse.current.position.ReadValue(), 1,
        //         Color.red + Color.yellow / 2, "Do something!"
        //     ));
        // }

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

        if (playerActions.SpawnEnemy.WasPressedThisFrame())
        {
            EventBus.Publish(new SpawnEnemyEvent());
        }
    }
}