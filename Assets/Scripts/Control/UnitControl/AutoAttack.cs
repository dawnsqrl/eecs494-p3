
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CodeMonkey.Utils;
using Unity.VisualScripting;
using UnityEngine;

public class AutoAttack : MonoBehaviour
{

    [SerializeField] private bool is_enemy;
    [SerializeField] private bool is_citizen;
    [SerializeField] private bool is_wild;
    private List<GameObject> opponentList;
    private List<GameObject> parentList;
    private float range;

    private void Start()
    {
        if (is_enemy)
        {
            opponentList = CitizenControl.citizenList;
            opponentList.Insert(0, GameObject.FindGameObjectWithTag("Mushroom"));
            parentList = AutoEnemyControl.autoEnemies;
            range = 5;
        }
        
        if (is_citizen)
        {
            opponentList = AutoEnemyControl.autoEnemies;
            opponentList.Insert(0, GameObject.FindGameObjectWithTag("BaseCar"));
            parentList = CitizenControl.citizenList;
            range = 3;
        }

        if (is_wild)
        {
            opponentList = AutoEnemyControl.autoEnemies;
            opponentList.AddRange(CitizenControl.citizenList);
            parentList = WildBeastControl.beastList;
            range = 8;
        }
    }

    private void Update()
    {
        bool onAssult = false;
        Vector3 movetoPosition = gameObject.transform.position;
        foreach (GameObject opponent in opponentList)
        {
            if ((opponent.transform.position - transform.position).magnitude < range)
            {
                movetoPosition = opponent.transform.position;
                onAssult = true;
                break;
            }
        }

        if (!onAssult)
        {
            return;
        }
        GetComponent<ClearSurroundingFog>().enabled = onAssult;
        UnitRTS unitRTS = GetComponent<UnitRTS>();
        unitRTS.MoveTo(movetoPosition);
    }

    private void OnDestroy()
    {
        parentList.Remove(gameObject);
    }
}