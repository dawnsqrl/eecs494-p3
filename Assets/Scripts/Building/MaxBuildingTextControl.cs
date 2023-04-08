using TMPro;
using UnityEngine;

public class MaxBuildingTextControl : MonoBehaviour
{
    private TextMeshProUGUI number;
    int buildingNum, maxBuildingNum;
    BuildingController bc; 

    private void Awake()
    {
        number = GetComponent<TextMeshProUGUI>();
        bc = GameObject.Find("BuildingCanvas").GetComponent<BuildingController>();
    }

    private void Update()
    {
        buildingNum = bc.building_num;
        maxBuildingNum = bc.max_building_num;
        number.text = buildingNum.ToString() + "/" + maxBuildingNum.ToString();
    }
}