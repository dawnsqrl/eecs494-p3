using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using CodeMonkey.Utils;
using UnityEngine.Diagnostics;

public class UnitRTS : MonoBehaviour
{
    private GameObject selectedGameObject;
    private IMovePosition movePosition;
    private void Awake()
    {
        selectedGameObject = transform.Find("Selected").gameObject;
        movePosition = GetComponent<IMovePosition>();
        SetSelectedActive(false);
    }

    public void SetSelectedActive(bool visible)
    {
        selectedGameObject.SetActive(visible);
    }

    public void MoveTo(Vector3 targetPosition)
    {
        movePosition.SetMovePosition(targetPosition);
    }
}