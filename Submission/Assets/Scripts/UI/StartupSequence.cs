using System;
using System.Collections;
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
    private readonly string startingInString = "Starting in...";

    private void Awake()
    {
        startDialog = new DisplayDialogEvent(
            "Get ready!", $"{startingInString} 3",
            new Dictionary<string, Tuple<UnityAction, bool>>()
            {
                // {
                //     "Start", new Tuple<UnityAction, bool>(
                //         () => EventBus.Publish(new GameStartEvent()), true
                //     )
                // },
                // // {
                // //     "Tutorial", new Tuple<UnityAction, bool>(
                // //         () => EventBus.Publish(new DisplayDialogEvent(gameDescriptionDialog)), true
                // //     )
                // // },
                // {
                //     "Guide", new Tuple<UnityAction, bool>(
                //         () =>
                //         {
                //             EventBus.Publish(new StartBuilderTutorialEvent());
                //             EventBus.Publish(new StartSnailTutorialEvent());
                //         }, true
                //     )
                // }
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
        StartCoroutine(DelayStart());
    }

    private IEnumerator DelayStart()
    {
        EventBus.Publish(startDialog);
        yield return new WaitForSeconds(0.5f);
        EventBus.Publish(new GameStartEvent());
        yield return new WaitForSeconds(1);
        EventBus.Publish(new UpdateDialogEvent(null, $"{startingInString} 2", null));
        yield return new WaitForSeconds(1);
        EventBus.Publish(new UpdateDialogEvent(null, $"{startingInString} 1", null));
        yield return new WaitForSeconds(1);
        EventBus.Publish(new UpdateDialogEvent(null, "Start!", null));
        yield return new WaitForSeconds(0.5f);
        EventBus.Publish(new DismissDialogEvent());
    }
}