using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailSprintManager : MonoBehaviour
{
    private bool canSprint;
    private float pushForce;
    private Rigidbody _rigidbody;
    private BasecarController _basecarController;

    private void Awake()
    {
        EventBus.Subscribe<SnailSprintEvent>(_ => Sprint());
        _rigidbody = GetComponent<Rigidbody>();
        _basecarController = GetComponent<BasecarController>();
    }

    private void Start()
    {
        canSprint = false;
        pushForce = 5;
    }
    public bool CanSprint()
    {
        return canSprint;
    }
    
    public void EnableSprint()
    {
        canSprint = true;
    }

    public void AddSprintForce(float frc)
    {
        pushForce += frc;
    }

    public void Sprint()
    {
        if (!canSprint || _basecarController.forwardDirection == Vector3.zero)
        {
            return;
        }
        _rigidbody.AddForce(pushForce * _basecarController.forwardDirection);
    }
}
