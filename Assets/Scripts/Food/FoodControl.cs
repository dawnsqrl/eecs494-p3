using UnityEngine;
using UnityEngine.InputSystem;

public class FoodControl : MonoBehaviour
{
    private Camera _camera;
    private Vector2 pos;
    private bool color_changed;
    private bool isDialogBlocking;
    private GameObject banner;
    private GrowthDemo _growthDemo;
    private SpriteRenderer _spriteRenderer;
    private BannerCanvas _bannerCanvas;

    private void Awake()
    {
        EventBus.Subscribe<DialogBlockingEvent>(e => isDialogBlocking = e.status);
        _camera = Camera.main;
    }

    private void Start()
    {
        pos = new Vector2(Mathf.CeilToInt(transform.position.x), Mathf.CeilToInt(transform.position.y));
        color_changed = false;
        isDialogBlocking = false;
        _growthDemo = GameObject.Find("GrowthDemoController").GetComponent<GrowthDemo>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _bannerCanvas = GetComponentInChildren<BannerCanvas>();
    }

    private void Update()
    {
        if (_growthDemo.Position2Growthed(pos) ||
            _growthDemo.FakeGrowthed(pos))
        {
            if (!color_changed)
            {
                print("adawdadwadadaw");
                color_changed = true;
                EventBus.Publish(new DisplayBannerEvent(
                    transform, -4, Color.red + Color.yellow / 2, StringPool.infestPromptBanner
                ));
            }
        }

        DetectObjectWithRaycast();
    }

    public void changeColor()
    {
        _spriteRenderer.color = Color.green;
    }

    public Vector2 getPos()
    {
        return pos;
    }

    public void DetectObjectWithRaycast()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && !isDialogBlocking)
        {
            Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.name == "cherry" && color_changed)
                {
                    changeColor();
                    Destroy(_bannerCanvas.gameObject);
                }
            }
        }
    }
}