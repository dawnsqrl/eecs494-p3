
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
    }

    private void Update()
    {
        if (currentOpponent.IsDestroyed())
        {
            onAssult = false;
            currentOpponent = null;
        }
        if (onAssult)
        {
            movetoPosition = currentOpponent.transform.position;
            _rts.MoveTo(movetoPosition);
            return;
        }
        // movetoPosition = gameObject.transform.position;
        enemyList = new List<GameObject>(CitizenControl.citizenList);
        // enemyList.Add(mushroom);
        if (enemyList.Count > 0)
        {
            for (int i = 0; i < enemyList.Count; i++)
            {
                if (enemyList[i].IsDestroyed())
                {
                    continue;
                }
                GameObject opponent = enemyList[i];
                if ((opponent.transform.position - transform.position).magnitude < range)
                {
                    movetoPosition = opponent.transform.position;
                    onAssult = true;
                    _self_hitHealth.SetCurrentOpponent(opponent);
                    currentOpponent = opponent;
                    break;
                }
            }
        }

        // if no small citizen on sight
        if (currentOpponent == null)
        {
            GameObject building = BuildingController.NearestBuilding(transform.position);
            if (building != null && (building.transform.position - transform.position).magnitude < range)
            {
                movetoPosition = building.transform.position;
                onAssult = true;
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