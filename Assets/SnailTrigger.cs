using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailTrigger : MonoBehaviour
{
    private BasecarController _controller;
    [SerializeField] private GameObject eatEffect;
    private float collisionTime;
    private float time_eat_hyphae = 3;
    private void Start()
    {
        _controller = transform.parent.GetComponent<BasecarController>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Hyphae"))
        {
            if (!eatEffect.activeSelf) 
            {
                eatEffect.SetActive(true);
            }
            if (Time.time - collisionTime > time_eat_hyphae)
            {
                other.gameObject.SetActive(false);
                eatEffect.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
                
        if (other.gameObject.CompareTag("Hyphae")) {
            collisionTime = Time.time;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.CompareTag("Hyphae")) {
            if (Time.time - collisionTime > time_eat_hyphae) {
                collision.gameObject.SetActive(false);
            }
        }
    }
    
}
