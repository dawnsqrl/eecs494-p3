using System;
using UnityEngine;

public class MovePosition : MonoBehaviour, IMoveVelocity
{
    [SerializeField] private float moveSpeed;
    private Vector3 velocityVector;
    private Rigidbody2D _rigidbody2D;
    public void SetVelocity(Vector3 _velocityVector)
    {
        velocityVector = _velocityVector;
    }

    private void FixedUpdate()
    {
        _rigidbody2D.velocity = velocityVector * moveSpeed;
    }
}