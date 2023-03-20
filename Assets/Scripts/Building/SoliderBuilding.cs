using System.Collections;
using System.Collections.Generic;
using CodeMonkey.Utils;
using Unity.VisualScripting;
using UnityEngine;

public class SoliderBuilding : MonoBehaviour
{
    private int maxSolider = 3;
    private List<GameObject> autoSoliders;
    private void Start()
    {
        autoSoliders = new List<GameObject>();
    }

    private void Update()
    {
        if (autoSoliders.Count < maxSolider)
        {
            GameObject solider = Instantiate(Resources.Load<GameObject>("Prefabs/Objects/Citizen"), transform.position, Quaternion.identity);
            solider.GetComponent<UnitRTS>().MoveTo(transform.position + generateRandomVector());
            autoSoliders.Add(solider);
        }
    }

    private Vector3 generateRandomVector()
    {
        Random.seed = System.DateTime.Now.Millisecond;
        int x = UnityEngine.Random.Range(60, 200);
        Random.seed = System.DateTime.Now.Millisecond;
        int y = UnityEngine.Random.Range(60, 200);
        return new Vector3(x / 100, y / 100, 0.0f);
    }
}