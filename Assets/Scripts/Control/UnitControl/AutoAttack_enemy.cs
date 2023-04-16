
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
    private HitHealth _self_hitHealth;
    private GameObject currentOpponent;

    private UnitRTS _rts;
    private Vector3 movetoPosition;
    private void Start()
    {
        mushroom = GameObject.FindGameObjectWithTag("Mushroom");
        range = 5;
        enemyList = new List<GameObject>();
        _self_hitHealth = GetComponentInChildren<HitHealth>();
        _rts = GetComponent<UnitRTS>();
        movetoPosition = transform.position;
        currentOpponent = null;
    }

    private void Update()
    {
        // try
        // {
        //     print("current opponent is "+currentOpponent);
        //     if (currentOpponent == null || currentOpponent.IsDestroyed())
        //     {
        //         onAssult = false;
        //         currentOpponent = null;
        //     }
        //     if (onAssult && currentOpponent != null && !currentOpponent.IsDestroyed())
        //     {
        //         movetoPosition = currentOpponent.transform.position;
        //         _rts.MoveTo(movetoPosition);
        //         return;
        //     }
        //     // movetoPosition = gameObject.transform.position;
        //     enemyList = new List<GameObject>(CitizenControl.citizenList);
        //     print("current enemy is "+enemyList);
        //     // enemyList.Add(mushroom);
        //     if (enemyList.Count > 0)
        //     {
        //         for (int i = 0; i < enemyList.Count; i++)
        //         {
        //             if (enemyList[i].IsDestroyed())
        //             {
        //                 continue;
        //             }
        //             GameObject opponent = enemyList[i];
        //             if (!opponent.IsDestroyed() && (opponent.transform.position - transform.position).magnitude < range)
        //             {
        //                 movetoPosition = opponent.transform.position;
        //                 onAssult = true;
        //                 _self_hitHealth.SetCurrentOpponent(opponent);
        //                 currentOpponent = opponent;
        //                 break;
        //             }
        //         }
        //     }
        //
        //     // if no small citizen on sight
        //     if (currentOpponent == null)
        //     {
        //         GameObject building = BuildingController.NearestBuilding(transform.position);
        //         if (building != null && (building.transform.position - transform.position).magnitude < range)
        //         {
        //             movetoPosition = building.transform.position;
        //             onAssult = true;
        //             _self_hitHealth.SetCurrentOpponent(building);
        //             currentOpponent = building;
        //         }
        //     }
        // }
        // catch (NullReferenceException e)
        // {
        //     // Get stack trace for the exception with source file information
        //     var st = new StackTrace(e, true);
        //     // Get the top stack frame
        //     var frame = st.GetFrame(0);
        //     // Get the line number from the stack frame
        //     var line = frame.GetFileLineNumber();
        //     print("code line is "+line);
        //     throw;
        // }
        // print("5");
        if (currentOpponent != null && currentOpponent.IsDestroyed())
        {
            // print("5.1");
            onAssult = false;
            currentOpponent = null;
        }
        // print("5.2");
        if (onAssult && currentOpponent != null)
        {
            // continue chasing the current opponent
            // or change current opponent to the current collision
            // print("5.3");
            movetoPosition = currentOpponent.transform.position;
            _rts.MoveTo(movetoPosition);
            return;
        }
        // print("5.4");
        // search for new enemy
        bool foundCitizen = false;
        // print("5.5");
        if (CitizenControl.citizenList != null)
            enemyList = new List<GameObject>(CitizenControl.citizenList);
        // print("5.6");
        if (enemyList != null && enemyList.Count > 0)
        {
            // print("5.7");
            for (int i = 0; i < enemyList.Count; i++)
            {
                // print("5.8");
                if (enemyList[i] == null || enemyList[i].IsDestroyed())
                {
                    // print("5.9");
                    continue;
                }
                // print("6.0");
                GameObject opponent = enemyList[i];
                // print("6.1");
                if (opponent != null && (opponent.transform.position - transform.position).magnitude < range)
                {
                    // print("6.2");
                    movetoPosition = opponent.transform.position;
                    if (_self_hitHealth != null)
                        _self_hitHealth.SetCurrentOpponent(opponent);
                    currentOpponent = opponent;
                    onAssult = true;
                    foundCitizen = true;
                    break;
                }
            }
        }
        // print("6.3");
        if (!foundCitizen)
        {
            GameObject building = BuildingController.NearestBuilding(transform.position);
            if (building != null && building.CompareTag("Mushroom"))
            {
                return;
            }
            if (building != null && (building.transform.position - transform.position).magnitude < range)
            {
                movetoPosition = building.transform.position;
                onAssult = true;
                if (_self_hitHealth != null)
                    _self_hitHealth.SetCurrentOpponent(building);
                currentOpponent = building;
            }
        }
    }

    private void OnDestroy()
    {
        AutoEnemyControl.autoSnails.Remove(gameObject);
    }
}