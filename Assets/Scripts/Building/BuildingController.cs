using System.Collections;
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
    }
}
