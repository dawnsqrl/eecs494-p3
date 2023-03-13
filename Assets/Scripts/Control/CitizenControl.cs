using System;
using System.Collections.Generic;
using CodeMonkey.Utils;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

public class CitizenControl : MonoBehaviour
{
    private void Start()
    {
        EventBus.Subscribe<SpawnCitizenEvent>(SpawnCitizen);
    }

    private void SpawnCitizen(SpawnCitizenEvent e)
    {
        var citizen =
            PrefabUtility.InstantiatePrefab(
                Resources.Load<GameObject>("Prefabs/Objects/Citizen")
            ) as GameObject;
        citizen.transform.position = UtilsClass.GetMouseWorldPosition();
        citizen.transform.rotation = Quaternion.identity;
        citizen.GetComponent<UnitRTS>().MoveTo(citizen.transform.position);
    }
}