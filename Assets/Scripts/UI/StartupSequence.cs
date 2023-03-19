using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StartupSequence : MonoBehaviour
{
    private DisplayDialogEvent startDialog;
    private DisplayDialogEvent controlListDialog;

    private void Awake()
    {
        startDialog = new DisplayDialogEvent(
            "Welcome to Mycelium Demo!", "Make your choice.", Vector2.zero,
            new Dictionary<string, UnityAction>()
            {
                { "Start", () => EventBus.Publish(new GameStartEvent()) },
                { "Controls", () => EventBus.Publish(controlListDialog) }
            }
        );
        controlListDialog = new DisplayDialogEvent(
            "Control list",
            @"Drag the hamster to lead area growth
Click the mushroom, then a grey tile to add a growth source
[C] to spawn citizen at mouse position (builder only)
[K] to spawn enemy at mouse position (enemy only)
[MDrag] to drag map view
[LDrag] to select citizens
[RClick] to set destination of citizens
[+/-] to increase / decrease game speed
[ESC] to pause",
            new Vector2(1200, 700),
            new Dictionary<string, UnityAction>()
            {
                { "I see", () => EventBus.Publish(startDialog) }
            }
        );
    }

    private void Start()
    {
        EventBus.Publish(startDialog);
    }
}