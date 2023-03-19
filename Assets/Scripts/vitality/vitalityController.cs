using UnityEngine;
using UnityEngine.InputSystem;

public class VitalityController : MonoBehaviour
{
    [SerializeField] private int init_vitality = 500, max_vitality = 1000;

    private int vitality;

    private void Awake()
    {
        EventBus.Subscribe<ModifyVitalityEvent>(e => vitality = e.vitality);
    }

    private void Start()
    {
        EventBus.Publish(new ModifyVitalityEvent(init_vitality));
    }

    private void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard.spaceKey.wasPressedThisFrame)
        {
            EventBus.Publish(new ModifyVitalityEvent(vitality - 100));
        }
    }
}