using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitMushroom : MonoBehaviour
{
    [SerializeField]
    float cd_time = 0.8f;

    float current_time;
    private void Start()
    {
        current_time = cd_time;
    }
    private void OnTriggerStay(Collider other)
    {
        if (current_time > 0) {
            current_time -= Time.deltaTime;
        }
        else {
            if (other.gameObject.CompareTag("Mushroom"))
            {
                other.gameObject.GetComponentInChildren<HitHealth>().GetDamage();
            }
            current_time = 0.8f;
        }
        
    }
}
