using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DefenceBuilding : MonoBehaviour
{
    private VitalityController vitality;
    private GameObject AttackRange, BaseCar;
    Vector3 denfenceOriginScale;
    bool OnDrag = false;
    Vector3 bomb_pos;

    bool ready = true;

    int DefenceRange;

    private void Awake()
    {
        EventBus.Subscribe<StartBuildingDragEvent>(_ => OnDrag = true);
        EventBus.Subscribe<EndBuildingDragEvent>(_ => OnDrag = false);
    }

    private void Start()
    {
        vitality = GameObject.Find("VitalityController").GetComponent<VitalityController>();
        vitality.decreaseVitality(300);
        BaseCar = GameObject.Find("BaseCar");
    }

    private void Update()
    {
        Vector2 pos = new Vector2(BaseCar.transform.position.x, BaseCar.transform.position.y);
        float DefencecRangeFloat = (float)DefenceRange;
        if (Vector2.Distance(pos, new Vector2(transform.position.x, transform.position.y)) < DefencecRangeFloat && ready)
        {
            bomb_pos = BaseCar.transform.position;
            StartCoroutine(BombAnimate(bomb_pos));
        }
    }

    IEnumerator BombAnimate(Vector3 bomb_pos)
    {
        ready = false;
        yield return new WaitForSeconds(1.0f);
        if (Vector2.Distance(new Vector2(BaseCar.transform.position.x, BaseCar.transform.position.y), bomb_pos) <= 1)
        {
            BaseCar.GetComponentInChildren<HitHealth>().MushroomGetDamage();
        }
        GameObject bomb = Instantiate(Resources.Load<GameObject>("Prefabs/Buildings/Bomb"), bomb_pos, Quaternion.identity);
        yield return new WaitForSeconds(1.0f);
        Destroy(bomb);
        yield return new WaitForSeconds(1.0f);
        ready = true;
    }

    public void SetPosition(Vector3 pos, int range)
    {
        DefenceRange = range;
        AttackRange = Instantiate(Resources.Load<GameObject>("Prefabs/Buildings/RangeCircle"), pos, Quaternion.identity);
        AttackRange.SetActive(false);
        AttackRange.transform.localScale = AttackRange.transform.localScale * range;
    }

    private void OnMouseEnter()
    {
        if (!OnDrag)
            AttackRange.SetActive(true);
    }

    private void OnMouseExit()
    {
        AttackRange.SetActive(false);
    }
}
