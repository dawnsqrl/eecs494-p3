using System;
using System.Collections.Generic;
using System.Linq;
using CodeMonkey.Utils;
using UnityEditor;
using UnityEngine;

public class WildBeastControl : MonoBehaviour
{
    public static List<GameObject> beastList;
    private void Start()
    {
        beastList = new List<GameObject>();
        beastList = GameObject.FindGameObjectsWithTag("Beast").ToList();
    }

    // private void _SpawnCitizen(SpawnCitizenEvent e)
    // {
    //     // var citizen = Resources.Load<GameObject>("Prefabs/Objects/Citizen");
    //     GameObject citizen = Instantiate(Resources.Load<GameObject>("Prefabs/Objects/Citizen"), UtilsClass.GetMouseWorldPosition(), Quaternion.identity);
    //     citizen.GetComponent<UnitRTS>().MoveTo(citizen.transform.position);
    //     citizenList.Add(citizen);
    // }
}