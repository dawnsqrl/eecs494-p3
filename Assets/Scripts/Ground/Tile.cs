using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;

    private Vector2 _selfCoordinate;
    private bool isDialogBlocking;

    private void Start()
    {
        isDialogBlocking = false;
    }

    public void SetDialogBlockingState(bool status)
    {
        isDialogBlocking = status;
    }

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

    private void OnMouseEnter()
    {
        if (!(isDialogBlocking || EventSystem.current.IsPointerOverGameObject()))
        {
            _highlight.SetActive(true);
        }
    }

    private void OnMouseExit()
    {
        _highlight.SetActive(false);
    }
}