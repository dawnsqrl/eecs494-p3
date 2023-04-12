using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class RandomResourceEvent : MonoBehaviour
{
    [SerializeField] private bool doGenerate;
    [SerializeField] private float minInterval = 5;
    [SerializeField] private float maxInterval = 10;

    private float nextEventTime;

    private void Start()
    {
        UpdateEventTime();
    }

    private void Update()
    {
        if (Time.time > nextEventTime)
        {
            if (doGenerate)
            {
                EventBus.Publish(new DisplayDialogEvent(
                    StringPool.defaultDialogTitle,
                    StringPool.defaultDialogContent,
                    new Dictionary<string, Tuple<UnityAction, bool>>()
                    {
                        { "Random!", null }
                    }
                ));
            }

            UpdateEventTime();
        }
    }

    private void UpdateEventTime()
    {
        nextEventTime = Time.time + Random.Range(minInterval, maxInterval);
    }
}