using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    public GameObject mushroom;
    private Dictionary<Vector2, GameObject> buildings;
    public int max_building_num;
    public int building_num;
    // Start is called before the first frame update
    void Start()
    {
        buildings = new Dictionary<Vector2, GameObject>();
        max_building_num = 5;
        building_num = 0;
    }

    public bool check_avai(Vector2 pos)
    {
        return !buildings.ContainsKey(pos);
    }

    public void register_building(Vector2 pos, GameObject building)
    {
        if (building == mushroom)
            buildings = new Dictionary<Vector2, GameObject>();
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

    public void unregister_building(GameObject building)
    {

        var toRemove = buildings.Where(kvp => kvp.Value == building).ToList();
        foreach (var item in toRemove)
        {
            buildings.Remove(item.Key);
        }

    }
}
