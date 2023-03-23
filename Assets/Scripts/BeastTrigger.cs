using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BeastTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Builder") || other.CompareTag("Enemy") || other.CompareTag("BaseCar"))
        {
            Destroy(transform.parent.gameObject);
        }
    }
}