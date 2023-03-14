using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
 
public class GridGenerator : MonoBehaviour {
    [SerializeField] private int _width, _height;
 
    [SerializeField] private GameObject _GridPrefab;
    // [SerializeField] private Transform _cam;
 
    // private Dictionary<Vector2, Tile> _tiles;
 
    void Start() {
        GenerateGrid();
    }
 
    void GenerateGrid() {
        // _tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < _width; x++) {
            for (int y = 0; y < _height; y++) {
                var spawnedGrid = PrefabUtility.InstantiatePrefab(_GridPrefab) as GameObject;
                spawnedGrid.transform.position = new Vector3(x, y);
                spawnedGrid.transform.rotation = Quaternion.identity;
                spawnedGrid.name = $"Tile {x} {y}";
 
                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                foreach (Transform child in spawnedGrid.transform)
                {
                    child.gameObject.GetComponent<Tile>().Init(isOffset);
                }

            }
        }
    }
}