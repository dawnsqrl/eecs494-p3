using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

// This script is for GridMap parents. 
public class CaveGridManager : MonoBehaviour
{
    private Dictionary<Vector2, GameObject> _tiles;
    private Dictionary<GameObject, GameObject> _tiles_ground;
    [SerializeField] private Color _groundColor;
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
            GameObject tile_ground = item.transform.Find("Tile_ground").gameObject;
            _tiles_ground.Add(item.gameObject, tile_ground);
            tile_ground.GetComponent<Tile>().SetSelfCoordinate(x, y);
            GroundTileManager _groundTileManager = tile_ground.GetComponent<GroundTileManager>();
            if (_groundTileManager.growthed)
            {
                _groundTileManager.SetGrowthed();
            }
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