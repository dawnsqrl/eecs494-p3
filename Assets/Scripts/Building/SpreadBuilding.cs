using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SpreadBuilding : MonoBehaviour
{
    [SerializeField] private SpriteRenderer healthBar;
    private VitalityController vitality;
    SpreadBuildingDrag spreadBuilding;

    float spread_time = 0.0f;
    bool finishSpread = false;
    float cooldownTimer = 0;

    private GameObject buildingController;
    private GrowthDemo growthDemo;
    private float original_bar_length;

    Vector2 pos;
    GameObject building;

    private void Start()
    {
        original_bar_length = healthBar.size.x;
        healthBar.size = new Vector2(0, healthBar.size.y);

        spreadBuilding = GameObject.Find("BuildingCanvas").transform.Find("Building0").transform.Find("BuildingIconHolder1").transform.Find("Spread").GetComponent<SpreadBuildingDrag>();
        buildingController = GameObject.Find("BuildingCanvas");
        growthDemo = GameObject.Find("GrowthDemoController").GetComponent<GrowthDemo>();

        vitality = GameObject.Find("VitalityController").GetComponent<VitalityController>();
        vitality.decreaseVitality(400);
        vitality.decreaseVitalityGrowth(10);

        pos = spreadBuilding.getPos();
        building = spreadBuilding.getBuilding();

        spread_time = calculateSpreadTime(growthDemo.getInitPos(), pos);

        StartCoroutine(StartSpread());
    }

    private void Update()
    {
        if (finishSpread)
        {
            buildingController.GetComponent<BuildingController>().register_building(pos, building);
            growthDemo.Position2GroundManager(pos).SetGrowthed();
            growthDemo.AddToEdge(pos);
        }
    }

    private void OnDestroy()
    {
        GameObject.Find("BuildingCanvas").GetComponent<BuildingController>().unregister_building(gameObject);
    }

    IEnumerator StartSpread()
    {
        cooldownTimer = spread_time;
        while (cooldownTimer > 0)
        {
            cooldownTimer -= SimulationSpeedControl.GetSimulationSpeed() * Time.deltaTime;
            healthBar.size = new Vector2(( 1 - cooldownTimer / spread_time) * original_bar_length, healthBar.size.y); 
            yield return null;
        }
        gameObject.transform.Find("SpreadBar").gameObject.SetActive(false);
        finishSpread = true;
    }

    private float calculateSpreadTime(Vector2 pos1, Vector2 pos2)
    {
        // 5 -> 10, 25 -> 30
        return Vector2.Distance(pos1, pos2) + 5.0f;
    }
}