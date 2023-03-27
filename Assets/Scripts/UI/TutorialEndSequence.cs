using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class TutorialEndSequence : MonoBehaviour
{
    [SerializeField] private GameObject builderTutorialArea;
    [SerializeField] private GameObject mainCamera;

    private bool hasBuilderTutorialEnded;
    private bool hasSnailTutorialEnded;

    private void Awake()
    {
        EventBus.Subscribe<EndBuilderTutorialEvent>(_ => hasBuilderTutorialEnded = true);
        EventBus.Subscribe<EndSnailTutorialEvent>(_ => hasSnailTutorialEnded = true);
    }

    private void Start()
    {
        hasBuilderTutorialEnded = false;
        hasSnailTutorialEnded = false;
    }

    private void Update()
    {
        if (hasBuilderTutorialEnded && hasSnailTutorialEnded)
        {
            EventBus.Publish(new DisplayDialogEvent(
                "Tutorial ended!", "Make your choice.",
                new Dictionary<string, Tuple<UnityAction, bool>>()
                {
                    {
                        "Return", new Tuple<UnityAction, bool>(
                            () => SceneManager.LoadScene(SceneManager.GetActiveScene().name), true
                        )
                    },
                    {
                        "Start", new Tuple<UnityAction, bool>(
                            () =>
                            {
                                EventBus.Publish(new GameStartEvent());
                                builderTutorialArea.SetActive(false);
                                mainCamera.transform.position = new Vector3(7.8f, 8.8f, -10);
                            }, true
                        )
                    }
                }
            ));
        }
    }
}