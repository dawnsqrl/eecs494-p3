using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailTrigger : MonoBehaviour
{
    private BasecarController _controller;

    private void Start()
    {
        _controller = transform.parent.GetComponent<BasecarController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            _controller.on_wall = true;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        _controller.on_wall = false;
    }
}
