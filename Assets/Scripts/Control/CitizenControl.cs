using System;
using System.Collections.Generic;
using System.Linq;
using CodeMonkey.Utils;
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
}