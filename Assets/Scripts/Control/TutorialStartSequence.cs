using UnityEngine;

public class TutorialStartSequence : MonoBehaviour
{
    private void Start()
    {
        EventBus.Publish(new StartBuilderTutorialEvent());
        EventBus.Publish(new StartSnailTutorialEvent());
    }
}