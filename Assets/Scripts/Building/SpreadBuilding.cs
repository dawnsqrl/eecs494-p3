using UnityEngine;

public class SpreadBuilding : MonoBehaviour
{
    private VitalityController vitality;

    private void Start()
    {
        vitality = GameObject.Find("VitalityController").GetComponent<VitalityController>();
        vitality.decreaseVitality(400);
        vitality.decreaseVitalityGrowth(10);
    }
}