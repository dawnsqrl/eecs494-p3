using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

// This script is for GridMap parents. 
public class GridManager : MonoBehaviour
{
    private Dictionary<Vector2, GameObject> _tiles;
    private Dictionary<GameObject, GameObject> _tiles_ground;

    private void Awake()
    {
        EventBus.Subscribe<DialogBlockingEvent>(_OnDialogBlocking);
    }

    private void _OnDialogBlocking(DialogBlockingEvent e)
    {
        foreach (Tile tile in GetComponentsInChildren<Tile>())
        {
            tile.SetDialogBlockingState(e.status);
        }
    }

    private void Start()
    {
        _tiles = new Dictionary<Vector2, GameObject>();
        _tiles_ground = new Dictionary<GameObject, GameObject>();
        foreach (Transform item in transform)
        {
            string[] digits = Regex.Split(item.gameObject.name, @"\D+");
            int.TryParse(digits[1], out int x);
            int.TryParse(digits[2], out int y);
            _tiles.Add(new Vector2(x, y), item.gameObject);
            _tiles_ground.Add(item.gameObject, item.transform.Find("Tile_ground").gameObject);
            item.transform.Find("Tile_ground").gameObject.GetComponent<Tile>().SetSelfCoordinate(x, y);
        }
    }

    public GameObject GetTileAtPosition(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out var tile)) return tile;
        return null;
    }

    public GameObject GetTileGroundAtPosition(GameObject _tile)
    {
        if (_tiles_ground.TryGetValue(_tile, out var tile)) return tile;
        return null;
    }
}