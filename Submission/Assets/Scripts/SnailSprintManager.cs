using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailSprintManager : MonoBehaviour
{
    private bool canSprint;
    private float sprintSpeed;
    private float sprintTime;
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
        sprintSpeed = 10;
        sprintTime = 0.3f;
    }
    public bool CanSprint()
    {
        return canSprint;
    }
    
    public void EnableSprint()
    {
        canSprint = true;
    }

    public void AddSprintSpeed(float spd)
    {
        sprintSpeed += spd;
    }

    public void Sprint()
    {
        if (!canSprint || _basecarController.forwardDirection == Vector3.zero)
        {
            return;
        }

        canSprint = false;
        StartCoroutine(SprintProcess());
    }

    private IEnumerator SprintProcess()
    {
        _basecarController.is_sprint = true;
        _basecarController.speed = sprintSpeed;
        yield return new WaitForSeconds(sprintTime);
        _basecarController.speed = _basecarController.normalSpeed;
        canSprint = true;
    }
}
