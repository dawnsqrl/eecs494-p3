using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    private Dictionary<Vector2, GameObject> buildings;
    // Start is called before the first frame update
    void Start()
    {
        buildings = new Dictionary<Vector2, GameObject>();
    }

    public bool check_avai(Vector2 pos)
    {
        return !buildings.ContainsKey(pos);
    }

    public void register_building(Vector2 pos, GameObject building)
    {
        buildings.Add(pos, building);
        buildings.Add(new Vector2(pos.x + 1, pos.y), building);
        buildings.Add(new Vector2(pos.x + 1, pos.y - 1), building);
        buildings.Add(new Vector2(pos.x, pos.y - 1), building);
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
