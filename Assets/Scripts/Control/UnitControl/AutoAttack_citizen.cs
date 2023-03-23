using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CodeMonkey.Utils;
using Unity.VisualScripting;
using UnityEngine;

public class AutoAttack_citizen : MonoBehaviour
{
    private float range;
    public bool onAssult;
    private GameObject basecar; 
    private void Start()
    {
        basecar = GameObject.FindGameObjectWithTag("BaseCar");
        range = 5;
    }

    private void Update()
    {
        onAssult = false;
        Vector3 movetoPosition = gameObject.transform.position;
        
        if ((basecar.transform.position - transform.position).magnitude < range)
        {
            movetoPosition = basecar.transform.position;
            onAssult = true;
        }

        
        if (AutoEnemyControl.autoEnemies.Count > 0)
        {
            foreach (GameObject opponent in AutoEnemyControl.autoEnemies)
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
        GetComponent<ClearSurroundingFog>().enabled = onAssult;
        UnitRTS unitRTS = GetComponent<UnitRTS>();
        unitRTS.MoveTo(movetoPosition);
    }

    private void OnDestroy()
    {
        CitizenControl.citizenList.Remove(gameObject);
    }
}