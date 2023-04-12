using UnityEngine;
using UnityEngine.UI;

public class VitalityBarController : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    private void Awake()
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