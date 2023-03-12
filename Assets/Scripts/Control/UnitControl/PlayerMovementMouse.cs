using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using CodeMonkey.Utils;
using UnityEngine.Diagnostics;

public class PlayerMovementMouse : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            GetComponent<MovePositionDirect>().SetMovePosition(UtilsClass.GetMouseWorldPosition());
        }
    }
}