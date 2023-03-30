
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CodeMonkey.Utils;
using Unity.VisualScripting;
using UnityEngine;

public class AutoAttack_enemy : MonoBehaviour
{
    private float range;
    public bool onAssult;
    private GameObject mushroom;
    private List<GameObject> enemyList;
    private void Start()
    {
        mushroom = GameObject.FindGameObjectWithTag("Mushroom");
        range = 5;
        enemyList = new List<GameObject>();
    }

    private void Update()
    {
        onAssult = false;
        Vector3 movetoPosition = gameObject.transform.position;
        enemyList = new List<GameObject>(CitizenControl.citizenList);
        enemyList.Add(mushroom);
        if (enemyList.Count > 0)
        {
            foreach (GameObject opponent in enemyList)
            {
                if ((opponent.transform.position - transform.position).magnitude < range)
                {
                    movetoPosition = opponent.transform.position;
                    onAssult = true;
                    break;
                }
            }
        }
        
        // if ( WildBeastControl.beastList.Count > 0)
        // {
        //     foreach (GameObject opponent in  WildBeastControl.beastList)
        //     {
        //         if ((opponent.transform.position - transform.position).magnitude < range)
        //         {
        //             movetoPosition = opponent.transform.position;
        //             onAssult = true;
        //             break;
        //         }
        //     }
        // }

        if (!onAssult)
        {
            return;
        }
        
        // GetComponent<ClearSurroundingFog>().enabled = onAssult;
        UnitRTS unitRTS = GetComponent<UnitRTS>();
        unitRTS.MoveTo(movetoPosition);
    }

    private void OnDestroy()
    {
        AutoEnemyControl.autoSnails.Remove(gameObject);
    }
}