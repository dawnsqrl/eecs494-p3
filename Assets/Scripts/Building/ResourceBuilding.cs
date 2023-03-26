using UnityEngine;

public class ResourceBuilding : MonoBehaviour
{
    private VitalityController vitality;

    private void Start()
    {
        vitality = GameObject.Find("VitalityController").GetComponent<VitalityController>();
        vitality.decreaseVitality(100);
        vitality.increaseVitalityGrowth(20);
    }
}