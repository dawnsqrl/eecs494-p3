using UnityEngine;

public class MovePositionDirect : MonoBehaviour, IMovePosition
{
    private Vector3 movePosition;

    private void Awake()
    {
        movePosition = transform.position;
    }

    public void SetMovePosition(Vector3 _movePosition)
    {
        movePosition = _movePosition;
    }

    private void Update()
    {
        // Vector3 moveDir = (movePosition - transform.position).normalized;
        // if (Vector3.Distance(movePosition, transform.position) < 0.5f)
        // {
        //     moveDir = Vector3.zero;
        // }
        //
        // GetComponent<IMoveVelocity>().SetVelocity(moveDir);
    }
}