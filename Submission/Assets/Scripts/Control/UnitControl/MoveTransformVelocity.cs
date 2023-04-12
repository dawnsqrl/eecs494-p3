using System;
using UnityEngine;

public class MoveTransformVelocity : MonoBehaviour, IMoveVelocity
{
    [SerializeField] private float moveSpeed;
    private Vector3 velocityVector;
    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void SetVelocity(Vector3 _velocityVector)
    {
        velocityVector = _velocityVector;
    }

    private void Update()
    {
        // transform.position += velocityVector * (
        //     moveSpeed * SimulationSpeedControl.GetSimulationSpeed() * Time.deltaTime
        // );
        
        // _rigidbody.velocity = velocityVector * (
        //     moveSpeed * SimulationSpeedControl.GetSimulationSpeed()
        // );
    }
}