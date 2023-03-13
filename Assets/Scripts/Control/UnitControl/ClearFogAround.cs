using System;
using UnityEngine;


public class ClearFogAround : MonoBehaviour
{
    private void Update()
    {
        if (Physics2D.Raycast(transform.position, Vector2.positiveInfinity, 3))
        {
            print("detect");
        }
    }
}
