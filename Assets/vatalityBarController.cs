using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class vatalityBarController : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    // Start is called before the first frame update
    void Awake()
    {
        EventBus.Subscribe<ModifyVitalityEvent>(SetVitality);
    }

    private void SetVitality(ModifyVitalityEvent e)
    {
        slider.value = e.vitality;

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    private void SetMaxVitality(int vitality)
    {
        slider.value = vitality;
        slider.maxValue = vitality;
    }
}
