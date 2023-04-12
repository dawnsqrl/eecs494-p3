using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CodeMonkey.Utils;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class AutoEnemyControl : MonoBehaviour
{
    public static List<GameObject> autoSnails_queue;
    public static List<GameObject> autoSnails;
    public static List<GameObject> foundSnails;
    [SerializeField] private SnailExpManager _snailExpManager;
    private BasecarController _basecarController;
    void Start()
    {
        autoSnails_queue = new List<GameObject>();
        autoSnails = new List<GameObject>();
        foundSnails = new List<GameObject>();
        // autoSnails = GameObject.FindGameObjectsWithTag("LittleSnail").ToList();
        _basecarController = gameObject.GetComponent<BasecarController>();
    }

    public void RemoveFromList(GameObject o)
    {
        autoSnails.Remove(o);
    }
    // private void _SpawnEnemy(SpawnEnemyEvent e)
    // {
    //     if (autoEnemies.Count < maxEnemyUnit)
    //     {
    //         GameObject enemy = Instantiate(Resources.Load<GameObject>("Prefabs/Objects/AutomatedEnemy"), transform.position, Quaternion.identity);
    //         enemy.GetComponent<UnitRTS>().MoveTo(transform.position);
    //         autoEnemies.Add(enemy);
    //     }
    // }

    private void Update()
    {
        for (int i = autoSnails.Count - 1; i >= 0; i--)
        {
            if (!autoSnails[i].IsDestroyed() && (autoSnails[i].transform.position - transform.position).magnitude < 5)
            {
                // Found a little snail
                foundSnails.Add(autoSnails[i]);
                _snailExpManager.AddExpPoints(2);
                autoSnails.RemoveAt(i);
            }
        }
        foreach (var autoSnail in autoSnails_queue)
        {
            autoSnails.Add(autoSnail);
        }
        autoSnails_queue.Clear();
        List<Vector3> targetPositionList =
            GetPositionListAround(transform.position + _basecarController.forwardDirection.normalized * 3, new float[] { 1f, 2f, 3f }, new int[] { 5, 10, 20 });
        int targetPositionListIndex = 0;
        for (int i = 0; i < foundSnails.Count; i++)
        {
            GameObject enemy = foundSnails[i];
            if (enemy.IsDestroyed())
            {
                foundSnails.RemoveAt(i);
                continue;
            }
            if (enemy.GetComponent<AutoAttack_enemy>().onAssult)
            {
                continue;
            }
            enemy.GetComponent<UnitRTS>().MoveTo(targetPositionList[targetPositionListIndex]);
            targetPositionListIndex = (targetPositionListIndex + 1) % targetPositionList.Count;
        }
    }
    
    private List<Vector3> GetPositionListAround(Vector3 startPosition, float[] ringDistanceArray,
        int[] ringPositionCountArray)
    {
        List<Vector3> positionList = new List<Vector3>();
        positionList.Add(startPosition);
        for (int i = 0; i < ringDistanceArray.Length; i++)
        {
            positionList.AddRange(GetPositionListAround(startPosition, ringDistanceArray[i],
                ringPositionCountArray[i]));
        }

        return positionList;
    }

    private List<Vector3> GetPositionListAround(Vector3 startPosition, float distance, int positionCount)
    {
        List<Vector3> positionList = new List<Vector3>();
        for (int i = 0; i < positionCount; i++)
        {
            float angle = i * (360f / positionCount);
            Vector3 dir = UtilsClass.ApplyRotationToVector(new Vector3(1, 0), angle);
            Vector3 position = startPosition + dir * distance;
            positionList.Add(position);
        }

        return positionList;
    }

    public static GameObject NearestEnemy( Vector3 pos)
    {
        float leastDistance = 999;
        GameObject res = null;
        for (int i = 0; i < foundSnails.Count; i++)
        {
            if (foundSnails[i].IsDestroyed())
            {
                continue;
            }

            float distance = Vector3.Distance(pos, foundSnails[i].transform.position);
            if (distance < leastDistance)
            {
                leastDistance = distance;
                res = foundSnails[i];
            }
        }
        return res;
    }
}
