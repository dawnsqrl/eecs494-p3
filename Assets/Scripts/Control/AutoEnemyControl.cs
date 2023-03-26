using System;
using System.Collections;
using System.Collections.Generic;
using CodeMonkey.Utils;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class AutoEnemyControl : MonoBehaviour
{
    // [SerializeField] private int maxEnemyUnit = 5;
    public static List<GameObject> autoEnemies;
    private BasecarController _basecarController;

    public TextMeshProUGUI maxUnitDisplay;
    // Start is called before the first frame update
    void Start()
    {
        // maxUnitDisplay.text =  "Max unit: " + maxEnemyUnit.ToString();
        autoEnemies = new List<GameObject>();
        // EventBus.Subscribe<SpawnEnemyEvent>(_SpawnEnemy);
        _basecarController = gameObject.GetComponent<BasecarController>();
    }

    public void RemoveFromList(GameObject o)
    {
        autoEnemies.Remove(o);
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
        List<Vector3> targetPositionList =
            GetPositionListAround(transform.position - _basecarController.forwardDirection.normalized * 2, new float[] { 1f, 2f, 3f }, new int[] { 5, 10, 20 });
        int targetPositionListIndex = 0;
        foreach (GameObject enemy in autoEnemies)
        {
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
}
