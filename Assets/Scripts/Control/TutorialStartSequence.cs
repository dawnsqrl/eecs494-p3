using System.Collections;
using UnityEngine;

public class TutorialStartSequence : MonoBehaviour
{
    private void Start()
    {
        SceneState.isTutorialAccessed = true;
        StartCoroutine(DelayStart());
    }

    private IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(0.5f);
        EventBus.Publish(new StartBuilderTutorialEvent());
        EventBus.Publish(new StartSnailTutorialEvent());
    }
}