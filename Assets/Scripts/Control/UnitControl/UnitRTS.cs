using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using CodeMonkey.Utils;
using UnityEngine.Diagnostics;
using GameObject = UnityEngine.GameObject;

public class UnitRTS : MonoBehaviour
{
    private GameObject selectedGameObject;
    private IMovePosition movePosition;
    private IMoveVelocity moveVelocity;

    private MovePositionDirect _movePositionDirect;

    private Rigidbody _rigidbody;

    private Vector3 targetPosition;

    private bool startMove = false;
    // private List<GameObject> clearedFogList;
    // private List<GameObject> prevClearedFogList;
    private void Start()
    {
        selectedGameObject = transform.Find("Selected").gameObject;
        movePosition = GetComponent<IMovePosition>();
        moveVelocity = GetComponent<IMoveVelocity>();
        _movePositionDirect = GetComponent<MovePositionDirect>();
        _rigidbody = GetComponent<Rigidbody>();
        SetSelectedActive(false);
    }

    public void SetSelectedActive(bool visible)
    {
        selectedGameObject.SetActive(visible);
    }

    public void MoveTo(Vector3 _targetPosition)
    {
        // movePosition.SetMovePosition(_targetPosition);
        // // moveVelocity.SetVelocity(
        // //     (targetPosition - transform.position).normalized * (5 * SimulationSpeedControl.GetSimulationSpeed())
        // // );
        // _movePositionDirect.SetMovePosition(_targetPosition);
        targetPosition = _targetPosition;
        startMove = true;
    }
    
    private void Update()
    {
        if (!startMove || (targetPosition - transform.position).magnitude < 0.3f)
        {
            _rigidbody.velocity = Vector3.zero;
            return;
        }
        
        if (GameProgressControl.isGameActive)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            _rigidbody.velocity = direction.normalized * (4 * SimulationSpeedControl.GetSimulationSpeed());
        }
    }
}