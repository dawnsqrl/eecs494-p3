using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class BuildingDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Transform parentAfterDrag;
    [SerializeField] private GameObject holder;
    [SerializeField] private GameObject gameMapPrefab;
    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Mouse.current.position.ReadValue();
        transform.localScale = new Vector2(0.3f, 0.3f);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector3 Worldpos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        if (Worldpos is { x: >= 0 and <= 30, y: >= 0 and <= 30 })
        {
            Vector2 pos = new Vector2(Mathf.FloorToInt(Worldpos.x + 0.5f), Mathf.FloorToInt(Worldpos.y + 0.5f));
            GrowthDemo growthDemo = GameObject.Find("GrowthDemoController").GetComponent<GrowthDemo>();
            //if (!growthDemo.Position2Growthed(pos) && !growthDemo.FakeGrowthed(pos))
            //{
            //Instantiate(Resources.Load<GameObject>("Prefabs/Objects/Food"),
            //    new Vector3(pos.x, pos.y, -2.0f), Quaternion.identity);
            Instantiate(gameMapPrefab, new Vector3(pos.x, pos.y, -2.0f), Quaternion.identity);
            growthDemo.Position2GroundManager(pos).SetGrowthed();
            //}
        }

        holder.GetComponent<SpellCooldown>().reStart();
        transform.SetParent(parentAfterDrag);
        transform.localScale = new Vector2(1, 1);
    }
}