using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceBuilding : MonoBehaviour
{
    [SerializeField] private GameObject vitality;
    // Start is called before the first frame update
    void Start()
    {
        vitality = GameObject.Find("VitalityController");
        vitality.GetComponent<VitalityController>().decreaseVitality(100);
        vitality.GetComponent<VitalityController>().increaseVitalityGrowth(10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
