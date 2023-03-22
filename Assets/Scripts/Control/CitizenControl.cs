using System;
using System.Collections.Generic;
using CodeMonkey.Utils;
using UnityEditor;
using UnityEngine;

public class CitizenControl : MonoBehaviour
{
    public static List<GameObject> citizenList;
    private void Awake()
    {
        EventBus.Subscribe<SpawnCitizenEvent>(_SpawnCitizen);
        citizenList = new List<GameObject>();
    }

    private void _SpawnCitizen(SpawnCitizenEvent e)
    {
        // var citizen = Resources.Load<GameObject>("Prefabs/Objects/Citizen");
        GameObject citizen = Instantiate(Resources.Load<GameObject>("Prefabs/Objects/Citizen"), UtilsClass.GetMouseWorldPosition(), Quaternion.identity);
        citizen.GetComponent<UnitRTS>().MoveTo(citizen.transform.position);
        citizenList.Add(citizen);
    }
}