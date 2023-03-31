using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DefenceBuilding : MonoBehaviour
{
    private VitalityController vitality;
    private GameObject AttackRange;
    Vector3 denfenceOriginScale;
    bool OnDrag = false;
    Vector3 bomb_pos;

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
    }

    private void Update()
    {
        float DefencecRangeFloat = (float)DefenceRange;
        while(true)
        {
            bomb_pos = new Vector3(Random.Range(transform.position.x - DefencecRangeFloat, transform.position.x + DefencecRangeFloat), Random.Range(transform.position.y - DefencecRangeFloat, transform.position.y + DefencecRangeFloat), -2.0f);
            if (Vector2.Distance(bomb_pos, transform.position) < DefencecRangeFloat)
                break;
        }
        StartCoroutine(BombAnimate(bomb_pos));
        
    }

    IEnumerator BombAnimate(Vector3 bomb_pos)
    {
        GameObject bomb = Instantiate(Resources.Load<GameObject>("Prefabs/Buildings/Bomb"), bomb_pos, Quaternion.identity);
        yield return new WaitForSeconds(1.0f);
        Destroy(bomb);
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
