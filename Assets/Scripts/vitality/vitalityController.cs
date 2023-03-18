using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class vitalityController : MonoBehaviour
{
    [SerializeField] private int init_vitality = 500, max_vitality = 1000;

    private int vitality;
    // Start is called before the first frame update
    void Awake()
    {
        EventBus.Subscribe<ModifyVitalityEvent>(e => vitality = e.vitality);
    }

    private void Start()
    {
        EventBus.Publish(new ModifyVitalityEvent(init_vitality));
    }

    // Update is called once per frame
    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard.spaceKey.wasPressedThisFrame)
        {
            EventBus.Publish(new ModifyVitalityEvent(vitality - 100));
        }
    }
}
