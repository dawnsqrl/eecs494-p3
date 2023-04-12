using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAnimation : MonoBehaviour
{
    Vector3 pos1;
    Vector3 pos2;

    float fastTime = 0.5f, slowTime = 1.0f;
    int stage = 1;
    bool lockStart = false;

    void Start()
    {
        pos1 = gameObject.transform.position;
        pos2 = new Vector3(pos1.x  - 2.0f, pos1.y + 1.5f, 0.0f);
        if (stage == 1 && !lockStart)
            StartCoroutine(pos1Fast2pos2());
        if (stage == 2 && !lockStart)
            StartCoroutine(pos2Slow2pos1());
    }

    IEnumerator pos1Fast2pos2()
    {
        lockStart = true;
        float initial_time = Time.time;
        float progress = (Time.time - initial_time) / fastTime;

        while (progress < 1.0f)
        {
            progress = (Time.time - initial_time) / fastTime;
            Vector3 new_position = Vector3.Lerp(pos1, pos2, progress);
            gameObject.transform.position = new_position;

            yield return null;
        }
        stage = 2;
        lockStart = false;
    }

    IEnumerator pos2Slow2pos1()
    {
        lockStart = true;
        float initial_time = Time.time;
        float progress = (Time.time - initial_time) / slowTime;

        while (progress < 1.0f)
        {
            progress = (Time.time - initial_time) / slowTime;
            Vector3 new_position = Vector3.Lerp(pos2, pos1, progress);
            gameObject.transform.position = new_position;

            yield return null;
        }
        stage = 1;
        lockStart = false;
    }
}
