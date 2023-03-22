using System.Collections;
using System.Collections.Generic;
using CodeMonkey.Utils;
using Unity.VisualScripting;
using UnityEngine;

public class AutoEnemyControl : MonoBehaviour
{
    private int maxEnemyUnit = 5;
    public static List<GameObject> autoEnemies;
    // Start is called before the first frame update
    void Start()
    {
        autoEnemies = new List<GameObject>();
        EventBus.Subscribe<SpawnEnemyEvent>(_SpawnEnemy);
    }

    public void RemoveFromList(GameObject o)
    {
        autoEnemies.Remove(o);
    }
    private void _SpawnEnemy(SpawnEnemyEvent e)
    {
        if (autoEnemies.Count < maxEnemyUnit)
        {
            GameObject enemy = Instantiate(Resources.Load<GameObject>("Prefabs/Objects/AutomatedEnemy"), transform.position, Quaternion.identity);
            enemy.GetComponent<UnitRTS>().MoveTo(transform.position);
            autoEnemies.Add(enemy);
        }
    }

}
