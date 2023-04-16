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
    private GameObject currentOpponent;

    private List<GameObject> enemyList;
    private HitHealth _self_hitHealth;
    private UnitRTS _rts;
    private Vector3 movetoPosition;
    private void Start()
    {
        basecar = GameObject.FindGameObjectWithTag("BaseCar");
        range = 5;
        enemyList = new List<GameObject>();
        _self_hitHealth = GetComponentInChildren<HitHealth>();
        _rts = GetComponent<UnitRTS>();
        movetoPosition = transform.position;
    }

    private void Update()
    {
        if (currentOpponent != null && currentOpponent.IsDestroyed())
        {
            onAssult = false;
            currentOpponent = null;
        }
        if (onAssult)
        {
            // continue chasing the current opponent
            // or change current opponent to the current collision
            if (currentOpponent != null)
            {
                movetoPosition = currentOpponent.transform.position;
                _rts.MoveTo(movetoPosition);
                return;
            }
        }
        
        // search for new enemy
        if (AutoEnemyControl.foundSnails != null)
            enemyList = new List<GameObject>(AutoEnemyControl.foundSnails);
        if (enemyList != null && enemyList.Count > 0)
        {
            for (int i = 0; i < enemyList.Count; i++)
            {
                if (enemyList[i].IsDestroyed())
                {
                    continue;
                }
                GameObject opponent = enemyList[i];
                if (opponent != null && (opponent.transform.position - transform.position).magnitude < range)
                {
                    movetoPosition = opponent.transform.position;
                    if (_self_hitHealth != null)
                        _self_hitHealth.SetCurrentOpponent(opponent);
                    currentOpponent = opponent;
                    onAssult = true;
                    break;
                }
            }
        } else if (basecar != null && (basecar.transform.position - transform.position).magnitude < range) {
            movetoPosition = basecar.transform.position;
            if (_self_hitHealth != null)
                _self_hitHealth.SetCurrentOpponent(basecar);
            currentOpponent = basecar;
            onAssult = true;
        } 
    }

    private void OnDestroy()
    {
        CitizenControl.citizenList.Remove(gameObject);
    }

    public void SetOnAssult(bool flag)
    {
        onAssult = flag;
    }

    public IEnumerator CloseAutoAttackForTime(float _time)
    {
        range = -1;
        yield return new WaitForSeconds(_time);
        range = 5;
    }
}