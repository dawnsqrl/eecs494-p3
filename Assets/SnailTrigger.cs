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
}
