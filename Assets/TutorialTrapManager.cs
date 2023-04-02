using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrapManager : MonoBehaviour
{
    [SerializeField] private List<GroundTileManager> _tiles;
    private void OnTriggerEnter(Collider other)
    {
        foreach (var _tile in _tiles)
        {
            _tile.SetGrowthed();
        }
    }
}
