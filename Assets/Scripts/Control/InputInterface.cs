using UnityEngine;
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

        if (playerActions.SnailLevelUpBegins.WasPressedThisFrame())
        {
            EventBus.Publish(new SnailLevelUpEvent());
        }

        if (playerActions.LevelUpOption1.WasPressedThisFrame())
        {
            EventBus.Publish(new SnailLevelupOptionEvent_1());
        }

        if (playerActions.LevelUpOption2.WasPressedThisFrame())
        {
            EventBus.Publish(new SnailLevelupOptionEvent_2());
        }

        if (playerActions.LevelUpOption3.WasPressedThisFrame()) // press 3 to choose skill
        {
            EventBus.Publish(new SnailLevelupOptionEvent_3());
        }

        if (playerActions.Sprint.WasPressedThisFrame())
        {
            EventBus.Publish(new SnailSprintEvent());
        }

        if (playerActions.Spit.WasPressedThisFrame())
        {
            EventBus.Publish(new SnailSpitEvent());
        }

        if (playerActions.Shield.WasPressedThisFrame())
        {
            EventBus.Publish(new SnailShieldEvent());
        }

        if (playerActions.Bomb.WasPressedThisFrame())
        {
            EventBus.Publish(new SnailBombEvent());
        }

        if (Keyboard.current.commaKey.wasPressedThisFrame)
        {
            EventBus.Publish(new GameEndEvent(true));
        }

        if (Keyboard.current.periodKey.wasPressedThisFrame)
        {
            EventBus.Publish(new GameEndEvent(false));
        }

        // if (playerActions.GenerateDialog.WasPressedThisFrame())
        // {
        //     EventBus.Publish(new DisplayDialogEvent(
        //         "Chirp", "Something happened!",
        //         new Dictionary<string, Tuple<UnityAction, bool>>()
        //         {
        //             { "What?", new Tuple<UnityAction, bool>(() => print("Clicked what"), true) },
        //             { "How?", new Tuple<UnityAction, bool>(() => print("Clicked how"), true) }
        //         }
        //     ));
        // }

        // if (playerActions.GeneratePopup.WasPressedThisFrame())
        // {
        //     EventBus.Publish(new DisplayPopupEvent(
        //         false, Mouse.current.position.ReadValue(), 1,
        //         Color.cyan, new Dictionary<string, UnityAction>()
        //         {
        //             { "Sprites/pngegg", () => print("Clicked 1") },
        //             { "", () => print("Clicked 2") },
        //             { "Sprites/null", () => print("Clicked 3") }
        //         }
        //     ));
        // }

        // if (playerActions.GenerateBanner.WasPressedThisFrame())
        // {
        //     EventBus.Publish(new DisplayBannerEvent(
        //         false, Mouse.current.position.ReadValue(), 1,
        //         Color.red + Color.yellow / 2, "Do something!"
        //     ));
        // }

        // if (playerActions.IncreaseSimulationSpeed.WasPressedThisFrame())
        // {
        //     EventBus.Publish(new ScrollSimulationSpeedEvent(true));
        // }

        // if (playerActions.DecreaseSimulationSpeed.WasPressedThisFrame())
        // {
        //     EventBus.Publish(new ScrollSimulationSpeedEvent(false));
        // }

        // if (playerActions.SpawnCitizen.WasPressedThisFrame())
        // {
        //     EventBus.Publish(new SpawnCitizenEvent());
        // }

        // if (playerActions.SpawnEnemy.WasPressedThisFrame())
        // {
        //     EventBus.Publish(new SpawnEnemyEvent());
        // }

        // if (Keyboard.current.digit1Key.wasPressedThisFrame)
        // {
        //     EventBus.Publish(new DisplayHintEvent("This is useless!"));
        // }

        // if (Keyboard.current.digit2Key.wasPressedThisFrame)
        // {
        //     EventBus.Publish(new UpdateHintEvent("What?"));
        // }

        // if (Keyboard.current.digit3Key.wasPressedThisFrame)
        // {
        //     EventBus.Publish(new DismissHintEvent());
        // }
    }
}