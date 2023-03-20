using UnityEngine;

public class GameEndTrigger : MonoBehaviour
{
    [SerializeField] private bool isBuilder;

    public void TriggerDeath()
    {
        EventBus.Publish(new GameEndEvent(!isBuilder));
    }
}