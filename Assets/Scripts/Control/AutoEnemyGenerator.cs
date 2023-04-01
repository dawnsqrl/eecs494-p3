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
    private int onGenerationSnailNUm;

    private void Start()
    {
        unitList = new List<GameObject>();
        onGenerationSnailNUm = 0;
    }

    private void Update()
    {
        for (int i = 0; i < unitList.Count; i++)
        {
            if (unitList[i].IsDestroyed())
            {
                unitList.RemoveAt(i);
                onGenerationSnailNUm--;
            }
        }

        while (onGenerationSnailNUm < maxUnit)
        {
            StartCoroutine(GenerateNewUnit());
            onGenerationSnailNUm++;
        }
        
        if (GridManager._tiles[new Vector2((int)transform.position.x, (int)transform.position.y)].GetComponentInChildren<GroundTileManager>().growthed)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator GenerateNewUnit()
    {
        yield return new WaitForSeconds(10);
        GameObject enemy = Instantiate(Resources.Load<GameObject>("Prefabs/Objects/LittleSnail"), transform.position, Quaternion.identity);
        enemy.GetComponent<UnitRTS>().MoveTo(transform.position);
        AutoEnemyControl.autoSnails_queue.Add(enemy);
        unitList.Add(enemy);
    }
}
