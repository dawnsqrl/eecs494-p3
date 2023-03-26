using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StartupSequence : MonoBehaviour
{
    private DisplayDialogEvent startDialog;
    // private UpdateDialogEvent gameDescriptionDialog;
    // private UpdateDialogEvent generalControlListDialog;
    // private UpdateDialogEvent builderControlListDialog;
    // private UpdateDialogEvent enemyControlListDialog;

    private void Awake()
    {
        startDialog = new DisplayDialogEvent(
            "BIOLOGY 452\nField Ecology of Snail-Fungus Interaction",
            "What would you like to try?",
            new Dictionary<string, Tuple<UnityAction, bool>>()
            {
                {
                    "Start", new Tuple<UnityAction, bool>(
                        () => EventBus.Publish(new GameStartEvent()), true
                    )
                },
                // {
                //     "Tutorial", new Tuple<UnityAction, bool>(
                //         () => EventBus.Publish(new DisplayDialogEvent(gameDescriptionDialog)), true
                //     )
                // },
                {
                    "Introduction", new Tuple<UnityAction, bool>(
                        () => EventBus.Publish(new StartBuilderTutorialEvent()), true
                    )
                }
            }
        );
        // gameDescriptionDialog = new UpdateDialogEvent(
        //     "About Mycelium [1/4]", StringPool.gameDescriptionText,
        //     new Dictionary<string, Tuple<UnityAction, bool>>()
        //     {
        //         {
        //             "Return", new Tuple<UnityAction, bool>(
        //                 () => EventBus.Publish(startDialog), true
        //             )
        //         },
        //         {
        //             StringPool.nextButtonText, new Tuple<UnityAction, bool>(
        //                 () => EventBus.Publish(generalControlListDialog), false
        //             )
        //         }
        //     },
        //     new Vector2(1000, 600)
        // );
        // generalControlListDialog = new UpdateDialogEvent(
        //     "General controls [2/4]", StringPool.generalControlList,
        //     new Dictionary<string, Tuple<UnityAction, bool>>()
        //     {
        //         {
        //             StringPool.previousButtonText, new Tuple<UnityAction, bool>(
        //                 () => EventBus.Publish(gameDescriptionDialog), false
        //             )
        //         },
        //         {
        //             StringPool.nextButtonText, new Tuple<UnityAction, bool>(
        //                 () => EventBus.Publish(builderControlListDialog), false
        //             )
        //         }
        //     },
        //     new Vector2(1000, 600)
        // );
        // builderControlListDialog = new UpdateDialogEvent(
        //     "Mushroom controls [3/4]", StringPool.builderControlList,
        //     new Dictionary<string, Tuple<UnityAction, bool>>()
        //     {
        //         {
        //             StringPool.previousButtonText, new Tuple<UnityAction, bool>(
        //                 () => EventBus.Publish(generalControlListDialog), false
        //             )
        //         },
        //         {
        //             StringPool.nextButtonText, new Tuple<UnityAction, bool>(
        //                 () => EventBus.Publish(enemyControlListDialog), false
        //             )
        //         }
        //     },
        //     new Vector2(1000, 600)
        // );
        // enemyControlListDialog = new UpdateDialogEvent(
        //     "Snail controls [4/4]", StringPool.enemyControlList,
        //     new Dictionary<string, Tuple<UnityAction, bool>>()
        //     {
        //         {
        //             StringPool.previousButtonText, new Tuple<UnityAction, bool>(
        //                 () => EventBus.Publish(builderControlListDialog), false
        //             )
        //         },
        //         {
        //             "Complete", new Tuple<UnityAction, bool>(
        //                 () => EventBus.Publish(startDialog), true
        //             )
        //         }
        //     },
        //     new Vector2(1000, 600)
        // );
    }

    private void Start()
    {
        EventBus.Publish(startDialog);
    }
}