using UnityEngine;

public class GameEndTrigger : MonoBehaviour
{
    [SerializeField] private bool isBuilder;

    private void OnDestroy()
    {
        EventBus.Publish(new GameEndEvent(!isBuilder));
    }
}