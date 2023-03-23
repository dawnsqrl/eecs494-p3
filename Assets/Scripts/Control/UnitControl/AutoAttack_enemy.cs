
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
    private void Start()
    {
        mushroom = GameObject.FindGameObjectWithTag("Mushroom");
        range = 5;
    }

    private void Update()
    {
        onAssult = false;
        Vector3 movetoPosition = gameObject.transform.position;
        
        if ((mushroom.transform.position - transform.position).magnitude < range)
        {
            movetoPosition = mushroom.transform.position;
            onAssult = true;
        }
        
        if (CitizenControl.citizenList.Count > 0)
        {
            foreach (GameObject opponent in CitizenControl.citizenList)
            {
                if ((opponent.transform.position - transform.position).magnitude < range)
                {
                    movetoPosition = opponent.transform.position;
                    onAssult = true;
                    break;
                }
            }
        }
        
        if ( WildBeastControl.beastList.Count > 0)
        {
            foreach (GameObject opponent in  WildBeastControl.beastList)
            {
                if ((opponent.transform.position - transform.position).magnitude < range)
                {
                    movetoPosition = opponent.transform.position;
                    onAssult = true;
                    break;
                }
            }
        }

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
        AutoEnemyControl.autoEnemies.Remove(gameObject);
    }
}