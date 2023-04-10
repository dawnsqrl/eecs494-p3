using System;
using System.Collections.Generic;
using System.Linq;
using CodeMonkey.Utils;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CitizenControl : MonoBehaviour
{
    public static List<GameObject> citizenList;
    [SerializeField] private List<GameObject> presetCitizen;
    private void Awake()
    {
        EventBus.Subscribe<SpawnCitizenEvent>(_SpawnCitizen);
    }

    private void Start()
    {
        citizenList = new List<GameObject>(presetCitizen);
        // citizenList = GameObject.FindGameObjectsWithTag("Citizen").ToList();
    }

    private void _SpawnCitizen(SpawnCitizenEvent e)
    {
        // var citizen = Resources.Load<GameObject>("Prefabs/Objects/Citizen");
        GameObject citizen = Instantiate(Resources.Load<GameObject>("Prefabs/Objects/Citizen"), UtilsClass.GetMouseWorldPosition(), Quaternion.identity);
        citizen.GetComponent<UnitRTS>().MoveTo(citizen.transform.position);
        citizenList.Add(citizen);
    }
    
    public static GameObject NearestCitizen( Vector3 pos)
    {
        float leastDistance = 999;
        GameObject res = null;
        for (int i = 0; i < citizenList.Count; i++)
        {
            print("citizen list:   ");
            print(i);
            print("    is    ");
            print(citizenList[i]);
            if (citizenList[i] == null || citizenList[i].IsDestroyed())
            {
                citizenList.RemoveAt(i);
                continue;
            }

            float distance = Vector3.Distance(pos, citizenList[i].transform.position);
            if (distance < leastDistance)
            {
                leastDistance = distance;
                res = citizenList[i];
            }
        }
        return res;
    }
}