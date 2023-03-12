using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;
    private Vector2 _selfCoordinate;

    public void SetSelfCoordinate(int x, int y)
    {
        _selfCoordinate = new Vector2(x, y);
    }

    public Vector2 GetSelfCoordinate(int x, int y)
    {
        return _selfCoordinate;
    }

    public void Init(bool isOffset)
    {
        _renderer.color = isOffset ? _offsetColor : _baseColor;
        _highlight.SetActive(false);
    }

    void OnMouseEnter()
    {
        if (!PauseControl.isPaused)
        {
            _highlight.SetActive(true);
        }
    }

    void OnMouseExit()
    {
        _highlight.SetActive(false);
    }
}