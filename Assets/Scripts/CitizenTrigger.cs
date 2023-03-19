using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CitizenTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(transform.parent.gameObject);
        }
    }
}