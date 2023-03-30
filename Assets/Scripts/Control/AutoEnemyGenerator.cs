using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CodeMonkey.Utils;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class AutoEnemyGenerator : MonoBehaviour
{
    [SerializeField] private int maxUnit = 1;
    private List<GameObject> unitList;

    private void Start()
    {
        unitList = new List<GameObject>();
        
    }

    private void Update()
    {
        foreach (var unit in unitList)
        {
            if (unit.IsDestroyed())
            {
                unitList.Remove(unit);
            }
        }

        while (unitList.Count < maxUnit)
        {
            GenerateNewUnit();
        }
    }

    public void GenerateNewUnit()
    {
        GameObject enemy = Instantiate(Resources.Load<GameObject>("Prefabs/Objects/LittleSnail"), transform.position, Quaternion.identity);
        enemy.GetComponent<UnitRTS>().MoveTo(transform.position);
        AutoEnemyControl.autoSnails_queue.Add(enemy);
        unitList.Add(enemy);
    }
}
