using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialEndSequence : MonoBehaviour
{
    private Sprite mouseGameImage;
    private Sprite keyboardGameImage;
    private bool hasBuilderTutorialEnded;
    private bool hasSnailTutorialEnded;
    private bool isEndDialogShown;

    private void Awake()
    {
        EventBus.Subscribe<EndBuilderTutorialEvent>(_ => hasBuilderTutorialEnded = true);
        EventBus.Subscribe<EndSnailTutorialEvent>(_ => hasSnailTutorialEnded = true);
        mouseGameImage = Resources.Load<Sprite>("Sprites/Background/MouseGame");
        keyboardGameImage = Resources.Load<Sprite>("Sprites/Background/KeyboardGame");
    }

    private void Start()
    {
        hasBuilderTutorialEnded = false;
        hasSnailTutorialEnded = false;
        isEndDialogShown = false;
    }

    private void Update()
    {
        if (!isEndDialogShown && hasBuilderTutorialEnded && hasSnailTutorialEnded)
        {
            isEndDialogShown = true;
            EventBus.Publish(new EndAllTutorialEvent());
            EventBus.Publish(new DisplayDialogEvent(
                "Tutorial completed!", "Game will start soon.",
                new Dictionary<string, Tuple<UnityAction, bool>>()
                {
                    // {
                    //     "Return", new Tuple<UnityAction, bool>(
                    //         () =>
                    //         {
                    //             SceneState.SetTransition(
                    //                 1, 2, StringPool.mainMenuScene, mouseGameImage, keyboardGameImage
                    //             );
                    //             EventBus.Publish(new TransitSceneEvent());
                    //         }, true
                    //     )
                    // },
                    // {
                    //     "Start", new Tuple<UnityAction, bool>(
                    //         () =>
                    //         {
                    //             SceneState.SetTransition(
                    //                 1, 0, StringPool.mainGameScene, mouseGameImage, keyboardGameImage
                    //             );
                    //             EventBus.Publish(new TransitSceneEvent());
                    //         }, true
                    //     )
                    // }
                }
            ));
            StartCoroutine(DelayStart());
        }
    }

    private IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(0.5f);
        EventBus.Publish(new DismissHintEvent(0));
        EventBus.Publish(new DismissHintEvent(1));
        yield return new WaitForSeconds(6);
        SceneState.SetTransition(
            1, 0, StringPool.mainGameScene, mouseGameImage, keyboardGameImage
        );
        EventBus.Publish(new TransitSceneEvent());
    }
}