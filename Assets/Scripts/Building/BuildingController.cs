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

    public bool check_mushroom(Vector2 pos)
    {
        if(buildings.ContainsKey(pos))
        {
            if (buildings[pos].gameObject.name == "Mushroom")
            {
                return true;
            }
        }
        return false;
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

        if (building_num >= 50)
            EventBus.Publish(new GameEndEvent(true));

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

    public void deregister_one_building(GameObject building)
    {
        max_building_num -= 5;
        var toRemove = buildings.Where(kvp => kvp.Value == building).ToList();
        foreach (var item in toRemove)
        {
            buildings.Remove(item.Key);
        }
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

    public static GameObject NearestBuilding(Vector3 pos, bool includeMushroom = true)
    {
        float leastDistance = 999;
        GameObject res = null;
        for (int i = 0; i < buildings.Count; i++)
        {
            KeyValuePair<Vector2, GameObject> building = buildings.ElementAt(i);
            if (building.Value.IsDestroyed() || (!includeMushroom && building.Value.CompareTag("Mushroom")))
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
                vitality.increaseVitality(100);
                break;
            case 'l':
                vitality.increaseVitality(150);
                break;
            case 'r':
                vitality.increaseVitality(250);
                break;
            default:
                break;
        }
        AudioClip clip = Resources.Load<AudioClip>("Audio/BuildingDown");
        AudioSource.PlayClipAtPoint(clip, AudioListenerManager.audioListenerPos, 0.7f);
        Destroy(buildings[pos].gameObject);
    }

    

public void setBuildingHighlight(Vector2 pos, bool status)
    {
        if (check_avai(pos))
            return;

        Color origin = new Color(255.0f, 255.0f, 255.0f, 255.0f);
        Color red = new Color(1.0f, 0.0f, 0.0f, 58.0f / 255.0f);

        if(status)
        {
            if (buildings[pos].gameObject.name[2] == 'r')
                buildings[pos].gameObject.transform.Find("Sprite").GetComponent<SpriteRenderer>().color = red;
            else
                buildings[pos].gameObject.GetComponent<SpriteRenderer>().color = red;
        } 
        else
        {
            if (buildings[pos].gameObject.name[2] == 'r')
                buildings[pos].gameObject.transform.Find("Sprite").GetComponent<SpriteRenderer>().color = origin;
            else
                buildings[pos].gameObject.GetComponent<SpriteRenderer>().color = origin;
        }
    }
}
