using System.Collections;
using System.Collections.Generic;
using CodeMonkey.Utils;
using Unity.VisualScripting;
using UnityEngine;

public class SoliderBuilding : MonoBehaviour
{
    private int maxSolider = 3;
    private List<GameObject> autoSoliders;
    private float last_res = 0.0f;
    private void Start()
    {
        autoSoliders = new List<GameObject>();
        StartCoroutine(GenerateSolider());
    }

    IEnumerator GenerateSolider()
    {
        while (true)
        {
            if (autoSoliders.Count < maxSolider)
            {
                yield return new WaitForSeconds(3.0f);
                GameObject solider = Instantiate(Resources.Load<GameObject>("Prefabs/Objects/Citizen"), transform.position, Quaternion.identity);
                solider.GetComponent<UnitRTS>().MoveTo(transform.position + generateRandomVector());
                solider.GetComponent<CitizenBuildingControl>().change_status(gameObject);
                autoSoliders.Add(solider);
            }

            yield return new WaitForSeconds(1.0f);
        }
        
    }

    private Vector3 generateRandomVector()
    {
        float new_res1, new_res2;
        while(true)
        {
            new_res1 = Random.Range(0.6f, 2.0f);
            if (new_res1 != last_res)
            {
                last_res = new_res1;
                break;
            }
                
        }
        while (true)
        {
            new_res2 = Random.Range(0.6f, 2.0f);
            if (new_res2 != last_res)
            {
                last_res = new_res2;
                break;
            }

        }
        return new Vector3(new_res1, new_res2, 0.0f);
    }

    public void removeCitizen(GameObject citizen)
    {
        autoSoliders.Remove(citizen);
    }
}