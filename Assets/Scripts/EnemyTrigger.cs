using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyTrigger : MonoBehaviour
{
    private GameObject baseCar;

    private void Start()
    {
        baseCar = GameObject.FindGameObjectWithTag("BaseCar");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Builder"))
        {
            baseCar.GetComponent<AutoEnemyControl>().RemoveFromList(transform.parent.gameObject);
            Destroy(transform.parent.gameObject);
        }
    }
}