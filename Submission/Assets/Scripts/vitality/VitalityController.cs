using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class VitalityController : MonoBehaviour
{
    [SerializeField] private int init_vitality = 500, max_vitality = 1000;
    [SerializeField] private int init_vitality_increase = 50, gap_time = 4;
    [SerializeField] private TextMeshProUGUI _mesh;
    private int vitality;
    private int vitality_increase;

    private void Awake()
    {
        int a = -1;
        EventBus.Subscribe<ModifyVitalityEvent>(e => vitality = e.vitality);
    }

    private void Update()
    {
        String note = vitality_increase > 0 ? "+" : "-";
        _mesh.text = $"{note}{vitality_increase * 1.5f}% / min";
    }

    private void Start()
    {
        EventBus.Publish(new ModifyVitalityEvent(init_vitality));
        vitality_increase = init_vitality_increase;
        StartCoroutine(VitalityChange());
    }

    private IEnumerator VitalityChange()
    {
        while (true)
        {
            while (!GameProgressControl.isGameActive)
                yield return true;
            yield return new WaitForSeconds(gap_time);
            increaseVitality(vitality_increase);
        }
    }

    public void changeVitality(int new_vitality)
    {
        EventBus.Publish(new ModifyVitalityEvent(new_vitality));
    }

    public void changeVitalityIncrease(int new_vitality_increase)
    {
        vitality_increase = new_vitality_increase;
    }

    public void increaseVitalityGrowth(int amount)
    {
        vitality_increase += amount;
    }

    public void decreaseVitalityGrowth(int amount)
    {
        vitality_increase -= amount;
    }

    public void increaseVitality(int amount)
    {
        EventBus.Publish(new ModifyVitalityEvent(
            Mathf.Clamp(vitality + amount, 0, max_vitality)
        ));
    }

    public void decreaseVitality(int amount)
    {
        EventBus.Publish(new ModifyVitalityEvent(
            Mathf.Clamp(vitality - amount, 0, max_vitality)
        ));
    }
}