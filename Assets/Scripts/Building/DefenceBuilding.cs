using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class DefenceBuilding : MonoBehaviour
{
    private VitalityController vitality;
    private GameObject AttackRange, AttackRangeTransparent, BaseCar;
    Vector3 denfenceOriginScale;
    bool OnDrag = false;
    // Vector3 bomb_pos;

    bool ready = true;

    int DefenceRange;
    bool isBuilderTutorialActive = false;

    //private GameObject bomb;
    private GameObject explode;
    
    [SerializeField] private Transform[] routes;

    Vector2 p0, p1, p2, p3;

    bool ready1 = true, ready2 = true;

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
        if (explode != null)
        {
            Destroy(explode);
        }
        
        AttackRange.SetActive(false);
        AttackRangeTransparent.SetActive(false);
        vitality.increaseVitalityGrowth(10);
        if (GameObject.Find("BuildingCanvas") != null)
            GameObject.Find("BuildingCanvas").GetComponent<BuildingController>().unregister_building(gameObject);
        
        if (!DestoryBuildingDrag.selfDestory)
        {
            EventBus.Publish(new AddExpEvent(5));
        }
        else
        {
            DestoryBuildingDrag.selfDestory = false;
        }
    }

    private void Update()
    {
        float x, y;
        Vector2 pos, bomb_pos;
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

    IEnumerator BombAnimate(Vector3 explode_pos)
    {
        float tParam = 0.0f;
        float speedModifier = 4.0f;
        ready = false;

        float y = transform.position.y;

        GetComponent<Animator>().SetTrigger("fire");
        yield return new WaitForSeconds(0.5f);

        GameObject bomb = Instantiate(Resources.Load<GameObject>("Prefabs/Buildings/Bomb"), routes[0].position,
            Quaternion.identity);

        tParam = 0.0f;
        while (tParam < 1)
        {
            tParam += Time.deltaTime * speedModifier;

            p0 = routes[0].position;
            p1 = routes[1].position;
            p2 = new Vector2(explode_pos.x, explode_pos.y + 3.0f);
            p3 = explode_pos;
            Vector2 objectPosition = Mathf.Pow(1 - tParam, 3) * p0 + 3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 + 3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 + Mathf.Pow(tParam, 3) * p3;

            bomb.transform.position = new Vector3(objectPosition.x, objectPosition.y, 0.0f);// - cameraPos + cameraOldPos;
            yield return null;// new WaitForEndOfFrame();
        }

        Destroy(bomb);

        foreach (GameObject snail in AutoEnemyControl.foundSnails)
        {
            BombHit(snail, explode_pos);
        }

        foreach (GameObject snail in AutoEnemyControl.autoSnails)
        {
            BombHit(snail, explode_pos);
        }

        BombHit(BaseCar, explode_pos);
        GameObject explode = Instantiate(Resources.Load<GameObject>("Prefabs/Buildings/explode"), explode_pos,
            Quaternion.identity);
        yield return new WaitForSeconds(0.75f);
        Destroy(explode);
        yield return new WaitForSeconds(0.5f);
        ready = true;
    }

    IEnumerator BombAnimate2(Vector3 explode_pos)
    {
        float tParam = 0.0f;
        float speedModifier = 4.0f;
        ready1 = false;
        yield return new WaitForSeconds(0.5f);
        yield return new WaitForSeconds(0.5f);

        float y = transform.position.y;

        GetComponent<Animator>().SetTrigger("fire");
        yield return new WaitForSeconds(0.5f);

        GameObject bomb = Instantiate(Resources.Load<GameObject>("Prefabs/Buildings/Bomb"), routes[0].position,
            Quaternion.identity);

        tParam = 0.0f;
        while (tParam < 1)
        {
            tParam += Time.deltaTime * speedModifier;

            p0 = routes[0].position;
            p1 = routes[1].position;
            p2 = new Vector2(explode_pos.x, explode_pos.y + 3.0f);
            p3 = explode_pos;
            Vector2 objectPosition = Mathf.Pow(1 - tParam, 3) * p0 + 3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 + 3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 + Mathf.Pow(tParam, 3) * p3;

            bomb.transform.position = new Vector3(objectPosition.x, objectPosition.y, 0.0f);// - cameraPos + cameraOldPos;
            yield return null;// new WaitForEndOfFrame();
        }

        Destroy(bomb);

        foreach (GameObject snail in AutoEnemyControl.foundSnails)
        {
            BombHit(snail, explode_pos);
        }

        foreach (GameObject snail in AutoEnemyControl.autoSnails)
        {
            BombHit(snail, explode_pos);
        }

        BombHit(BaseCar, explode_pos);
        GameObject explode = Instantiate(Resources.Load<GameObject>("Prefabs/Buildings/explode"), explode_pos,
            Quaternion.identity);
        yield return new WaitForSeconds(0.75f);
        Destroy(explode);
        yield return new WaitForSeconds(0.5f);
        ready1 = true;
    }

    IEnumerator BombAnimate3(Vector3 explode_pos)
    {
        float tParam = 0.0f;
        float speedModifier = 4.0f;
        ready2 = false;
        yield return new WaitForSeconds(1.5f);

        float y = transform.position.y;

        GetComponent<Animator>().SetTrigger("fire");
        yield return new WaitForSeconds(0.5f);

        GameObject bomb = Instantiate(Resources.Load<GameObject>("Prefabs/Buildings/Bomb"), routes[0].position,
            Quaternion.identity);

        tParam = 0.0f;
        while (tParam < 1)
        {
            tParam += Time.deltaTime * speedModifier;

            p0 = routes[0].position;
            p1 = routes[1].position;
            p2 = new Vector2(explode_pos.x, explode_pos.y + 3.0f);
            p3 = explode_pos;
            Vector2 objectPosition = Mathf.Pow(1 - tParam, 3) * p0 + 3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 + 3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 + Mathf.Pow(tParam, 3) * p3;

            bomb.transform.position = new Vector3(objectPosition.x, objectPosition.y, 0.0f);// - cameraPos + cameraOldPos;
            yield return null;// new WaitForEndOfFrame();
        }

        Destroy(bomb);

        foreach (GameObject snail in AutoEnemyControl.foundSnails)
        {
            BombHit(snail, explode_pos);
        }

        foreach (GameObject snail in AutoEnemyControl.autoSnails)
        {
            BombHit(snail, explode_pos);
        }

        BombHit(BaseCar, explode_pos);
        GameObject explode = Instantiate(Resources.Load<GameObject>("Prefabs/Buildings/explode"), explode_pos,
            Quaternion.identity);
        yield return new WaitForSeconds(0.75f);
        Destroy(explode);
        yield return new WaitForSeconds(0.5f);
        ready2 = true;
    }

    private void BombHit(GameObject target, Vector2 bomb_pos)
    {
        if (!target.IsDestroyed() &&
            Vector2.Distance(new Vector2(target.transform.position.x, target.transform.position.y), bomb_pos) <= 2.0f)
        {
            target.GetComponentInChildren<HitHealth>().GetDamage(2);
        }
    }

    public void SetPosition(Vector3 pos, int range)
    {
        DefenceRange = range;
        AttackRange = Instantiate(Resources.Load<GameObject>("Prefabs/Buildings/RangeCircle"), pos,
            Quaternion.identity);
        AttackRange.SetActive(false);
        AttackRange.transform.localScale = AttackRange.transform.localScale * range;
        AttackRangeTransparent = Instantiate(Resources.Load<GameObject>("Prefabs/Buildings/RangeCircleSnail"), pos,
            Quaternion.identity);
        AttackRangeTransparent.SetActive(true);
        AttackRangeTransparent.transform.localScale = AttackRangeTransparent.transform.localScale * range;
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