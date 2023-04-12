using UnityEngine;

public class TutorialStartSequence : MonoBehaviour
{
    private void Start()
    {
        SceneState.isTutorialAccessed = true;
        EventBus.Publish(new StartBuilderTutorialEvent());
        EventBus.Publish(new StartSnailTutorialEvent());
    }
}