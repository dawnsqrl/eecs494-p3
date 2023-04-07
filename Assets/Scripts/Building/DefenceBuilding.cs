using System.Collections;
using Unity.VisualScripting;
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
    bool isBuilderTutorialActive = false;

    private void Awake()
    {
        EventBus.Subscribe<StartBuilderTutorialEvent>(_ => isBuilderTutorialActive = true);
        EventBus.Subscribe<StartBuildingDragEvent>(_ => OnDrag = true);
        EventBus.Subscribe<EndBuildingDragEvent>(_ => OnDrag = false);
        AudioClip clip = Resources.Load<AudioClip>("Audio/DefenseBuilding");
        AudioSource.PlayClipAtPoint(clip, transform.position);
    }

    private void Start()
    {
        vitality = GameObject.Find("VitalityController").GetComponent<VitalityController>();
        vitality.decreaseVitality(300);
        vitality.decreaseVitalityGrowth(10);
        BaseCar = GameObject.Find("BaseCar");
    }

    private void OnDestroy()
    {
        AttackRange.SetActive(false);
        vitality.increaseVitalityGrowth(10);
        GameObject.Find("BuildingCanvas").GetComponent<BuildingController>().unregister_building(gameObject);
        AudioClip clip = Resources.Load<AudioClip>("Audio/BuildingDown");
        AudioSource.PlayClipAtPoint(clip, transform.position);
    }

    private void Update()
    {
        Vector2 pos = new Vector2(0.0f, 0.0f);;
        if (!isBuilderTutorialActive)
        {
            pos = new Vector2(BaseCar.transform.position.x, BaseCar.transform.position.y);
        }

        if (!(Vector2.Distance(pos, new Vector2(transform.position.x, transform.position.y)) < (float)DefenceRange))
        {
            if (AutoEnemyControl.NearestEnemy(transform.position) != null)
            {
                pos = AutoEnemyControl.NearestEnemy(transform.position).transform.position;
            }
        }

        if (pos == new Vector2(0.0f, 0.0f))
        {
            return;
        }

        // float DefencecRangeFloat = (float)DefenceRange;
        if ((Vector2.Distance(pos, new Vector2(transform.position.x, transform.position.y)) < (float)DefenceRange) && ready)
        {
            StartCoroutine(BombAnimate(pos));
        }
    }

    IEnumerator BombAnimate(Vector3 bomb_pos)
    {
        ready = false;
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < AutoEnemyControl.foundSnails.Count; i++)
        {
            BombHit(AutoEnemyControl.foundSnails[i]);
        }

        for (int i = 0; i < AutoEnemyControl.autoSnails.Count; i++)
        {
            BombHit(AutoEnemyControl.autoSnails[i]);
        }
        BombHit(BaseCar);
        GameObject bomb = Instantiate(Resources.Load<GameObject>("Prefabs/Buildings/Bomb"), bomb_pos, Quaternion.identity);
        yield return new WaitForSeconds(0.75f);
        Destroy(bomb);
        yield return new WaitForSeconds(0.5f);
        ready = true;
    }

    private void BombHit(GameObject target)
    {
        if (!target.IsDestroyed() && Vector2.Distance(new Vector2(target.transform.position.x, target.transform.position.y), bomb_pos) <= 1)
        {
            target.GetComponentInChildren<HitHealth>().GetDamage(1);
        }
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