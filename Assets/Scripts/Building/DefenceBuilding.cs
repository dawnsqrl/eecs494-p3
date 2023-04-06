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

    bool ready = true, ready1 = true, ready2 = true;

    int DefenceRange;
    bool isBuilderTutorialActive = false, time = false;

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
        //StartCoroutine(count_Time());
    }

    private void OnDestroy()
    {
        GameObject.Find("BuildingCanvas").GetComponent<BuildingController>().unregister_building(gameObject);
        AudioClip clip = Resources.Load<AudioClip>("Audio/BuildingDown");
        AudioSource.PlayClipAtPoint(clip, transform.position);
    }

    private void Update()
    {
        float x, y;
        Vector2 pos;
        if (!isBuilderTutorialActive)
           pos = new Vector2(transform.position.x, transform.position.y);
        else
            pos = new Vector2(0.0f, 0.0f);

        float DefencecRangeFloat = (float)DefenceRange;
        if (ready)
        {
            while (true)
            {
                x = Random.Range(pos.x - DefencecRangeFloat, pos.x + DefencecRangeFloat);
                y = Random.Range(pos.y - DefencecRangeFloat, pos.y + DefencecRangeFloat);
                if (Vector2.Distance(new Vector2(x, y), pos) <= DefencecRangeFloat && Vector2.Distance(new Vector2(x, y), pos) >= 1)
                    break;
            }
            bomb_pos = new Vector3(x, y, 0.0f);
            StartCoroutine(BombAnimate(bomb_pos));
        }
        if (ready1)
        {
            while (true)
            {
                x = Random.Range(pos.x - DefencecRangeFloat, pos.x + DefencecRangeFloat);
                y = Random.Range(pos.y - DefencecRangeFloat, pos.y + DefencecRangeFloat);
                if (Vector2.Distance(new Vector2(x, y), pos) <= DefencecRangeFloat && Vector2.Distance(new Vector2(x, y), pos) >= 1)
                    break;
            }
            bomb_pos = new Vector3(x, y, 0.0f);
            StartCoroutine(BombAnimate2(bomb_pos));
        }
        if (ready2)
        {
            while (true)
            {
                x = Random.Range(pos.x - DefencecRangeFloat, pos.x + DefencecRangeFloat);
                y = Random.Range(pos.y - DefencecRangeFloat, pos.y + DefencecRangeFloat);
                if (Vector2.Distance(new Vector2(x, y), pos) <= DefencecRangeFloat && Vector2.Distance(new Vector2(x, y), pos) >= 1)
                    break;
            }
            bomb_pos = new Vector3(x, y, 0.0f);
            StartCoroutine(BombAnimate3(bomb_pos));
        }
    }

    IEnumerator BombAnimate(Vector3 bomb_pos)
    {
        ready = false;
        //yield return new WaitForSeconds(0.25f);
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
        yield return new WaitForSeconds(1.0f);
        Destroy(bomb);
        yield return new WaitForSeconds(0.25f);
        ready = true;
    }

    IEnumerator BombAnimate2(Vector3 bomb_pos)
    {
        ready1 = false;
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
        yield return new WaitForSeconds(1.0f);
        Destroy(bomb);
        yield return new WaitForSeconds(0.25f);
        ready1 = true;
    }

    IEnumerator BombAnimate3(Vector3 bomb_pos)
    {
        ready2 = false;
        yield return new WaitForSeconds(1.0f);
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
        yield return new WaitForSeconds(1.0f);
        Destroy(bomb);
        //yield return new WaitForSeconds(0.25f);
        ready2 = true;
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