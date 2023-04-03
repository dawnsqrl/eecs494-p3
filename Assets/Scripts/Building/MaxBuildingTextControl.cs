using TMPro;
using UnityEngine;

public class MaxBuildingTextControl : MonoBehaviour
{
    private TextMeshProUGUI number;
    int buildingNum, maxBuildingNum;

    private void Awake()
    {
        number = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        buildingNum = GameObject.Find("BuildingCanvas").GetComponent<BuildingController>().building_num;
        maxBuildingNum = GameObject.Find("BuildingCanvas").GetComponent<BuildingController>().max_building_num;
        number.text = buildingNum.ToString() + "/" + maxBuildingNum.ToString();
    }
}