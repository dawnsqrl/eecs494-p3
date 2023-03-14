using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CitizenTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        transform.parent.GetComponent<SpriteRenderer>().color = Color.red;
    }
}