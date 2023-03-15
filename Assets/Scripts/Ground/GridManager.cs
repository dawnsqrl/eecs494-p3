using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

// This script is for GridMap parents. 
public class GridManager : MonoBehaviour
{
    private Dictionary<Vector2, GameObject> _tiles;

    // private void Awake()
    // {
    //     EventBus.Subscribe<ModifyPauseEvent>(_OnModifyPause);
    // }
    //
    // private void _OnModifyPause(ModifyPauseEvent e)
    // {
    //     foreach (Tile tile in GetComponentsInChildren<Tile>())
    //     {
    //         tile.SetHoverState(!e.status);
    //     }
    // }

    private void Start()
    {
        _tiles = new Dictionary<Vector2, GameObject>();
        foreach (Transform item in transform)
        {
            string[] digits = Regex.Split(item.gameObject.name, @"\D+");
            int.TryParse(digits[1], out int x);
            int.TryParse(digits[2], out int y);
            _tiles.Add(new Vector2(x, y), item.gameObject);
            item.transform.Find("Tile_ground").gameObject.GetComponent<Tile>().SetSelfCoordinate(x, y);
        }
    }

    public GameObject GetTileAtPosition(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out var tile)) return tile;
        return null;
    }
}