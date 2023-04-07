using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    public GameObject mushroom;
    private static Dictionary<Vector2, GameObject> buildings;
    public int max_building_num;
    public int building_num;
    private VitalityController vitality;

    bool first = true;
    // Start is called before the first frame update
    void Start()
    {
        buildings = new Dictionary<Vector2, GameObject>();
        max_building_num = 5;
        building_num = 0;
        vitality = GameObject.Find("VitalityController").GetComponent<VitalityController>();
    }

    public bool check_avai(Vector2 pos)
    {
        return !buildings.ContainsKey(pos);
    }

    public void register_building(Vector2 pos, GameObject building)
    {
        if (first)
        {
            buildings = new Dictionary<Vector2, GameObject>();
            first = false;
        }
            
        //if (building == mushroom)
        //    buildings = new Dictionary<Vector2, GameObject>();
        //print("register");
        if (building != mushroom)
            building_num += 1;

        buildings.Add(pos, building);
        buildings.Add(new Vector2(pos.x + 1, pos.y), building);
        buildings.Add(new Vector2(pos.x + 1, pos.y - 1), building);
        buildings.Add(new Vector2(pos.x, pos.y - 1), building);
    }

    public void register_one_building(Vector2 pos, GameObject building)
    {
        max_building_num += 5;
        buildings.Add(pos, building);
    }

    public void deregister_one_building()
    {
        max_building_num -= 5;
    }

    public void unregister_building(GameObject building)
    {
        if (building != mushroom)
            building_num -= 1;

        var toRemove = buildings.Where(kvp => kvp.Value == building).ToList();
        foreach (var item in toRemove)
        {
            buildings.Remove(item.Key);
        }

    }

    public static GameObject NearestBuilding(Vector3 pos)
    {
        float leastDistance = 999;
        GameObject res = null;
        for (int i = 0; i < buildings.Count; i++)
        {
            KeyValuePair<Vector2, GameObject> building = buildings.ElementAt(i);
            if (building.Value.IsDestroyed())
            {
                continue;
            }

            float distance = Vector3.Distance(pos, building.Value.transform.position);
            if (distance < leastDistance)
            {
                leastDistance = distance;
                res = building.Value;
            }
        }
        return res;
    }

    public void destoryBuildingUnregister(Vector2 pos)
    {
        switch(buildings[pos].gameObject.name[2])
        {
            case 'f':
                vitality.increaseVitality(150);
                break;
            case 's':
                vitality.increaseVitality(50);
                break;
            case 'l':
                vitality.increaseVitality(100);
                break;
            case 'r':
                vitality.increaseVitality(200);
                break;
            default:
                break;
        }
        Destroy(buildings[pos].gameObject);
    }

    public void setBuildingHighlight(Vector2 pos, bool status)
    {
        if (check_avai(pos))
            return;

        Color origin = new Color(255.0f, 255.0f, 255.0f, 255.0f);
        Color red = new Color(1.0f, 0.0f, 0.0f, 58.0f / 255.0f);

        if(status)
            buildings[pos].gameObject.GetComponent<SpriteRenderer>().color = red;
        else
            buildings[pos].gameObject.GetComponent<SpriteRenderer>().color = origin;
    }
}
