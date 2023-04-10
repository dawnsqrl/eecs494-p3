using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrapManager : MonoBehaviour
{
    private bool set = false;
    [SerializeField] private List<GroundTileManager> _tiles;
    private void OnTriggerEnter(Collider other)
    {
        if (!set && other.CompareTag("BaseCar"))
        {
            set = true;
            foreach (var _tile in _tiles)
            {
                _tile.SetGrowthed();
            }
        }
    }
}
