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

    private bool startMove = false, isBuilderTutorialActive = false;

    private float _velocity;
    // private List<GameObject> clearedFogList;
    // private List<GameObject> prevClearedFogList;
    //private bool startTutorial;

    private void Awake()
    {
        EventBus.Subscribe<StartBuilderTutorialEvent>(_ => isBuilderTutorialActive = true);
    }
    private void Start()
    {
        //startTutorial = GameObject.Find("BuilderTutorial").GetComponent<BuilderTutorialController>().getStart() || BasecarController.is_tutorial;
        selectedGameObject = transform.Find("Selected").gameObject;
        movePosition = GetComponent<IMovePosition>();
        moveVelocity = GetComponent<IMoveVelocity>();
        _movePositionDirect = GetComponent<MovePositionDirect>();
        _rigidbody = GetComponent<Rigidbody>();
        SetSelectedActive(false);
        _velocity = 2;
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
        if (Input.GetKey(KeyCode.Alpha1))
        {
            MoveTo(transform.position + new Vector3(-32.0f, 18.0f, 0.0f));
        }
        if (!startMove || (targetPosition - transform.position).magnitude < 0.3f)
        {
            _rigidbody.velocity = Vector3.zero;
            return;
        }
        
        if (GameProgressControl.isGameActive || BasecarController.is_tutorial || isBuilderTutorialActive) //|| startTutorial)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            _rigidbody.velocity = direction.normalized * (_velocity * SimulationSpeedControl.GetSimulationSpeed());
        }
    }

    public void SetCitizenOnMucus(bool flag)
    {
        _velocity = flag ? 2f : 2;
    }
}