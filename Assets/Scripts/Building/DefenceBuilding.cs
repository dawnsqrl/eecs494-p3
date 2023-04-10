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

    [SerializeField] private Transform[] routes;

    Vector2 p0, p1, p2, p3;

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
        AttackRangeTransparent.SetActive(false);
        vitality.increaseVitalityGrowth(10);
        GameObject.Find("BuildingCanvas").GetComponent<BuildingController>().unregister_building(gameObject);
        AudioClip clip = Resources.Load<AudioClip>("Audio/BuildingDown");
        AudioSource.PlayClipAtPoint(clip, transform.position);
        EventBus.Publish(new AddExpEvent(5));
    }

    private void Update()
    {
        Vector2 pos = new Vector2(0.0f, 0.0f);
        
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
        if ((Vector2.Distance(pos, new Vector2(transform.position.x, transform.position.y)) < (float)DefenceRange) &&
            ready)
        {
            StartCoroutine(BombAnimate(pos));
        }
    }

    IEnumerator BombAnimate(Vector3 explode_pos)
    {
        float tParam = 0.0f;
        float speedModifier = 4.0f;
        ready = false;
        yield return new WaitForSeconds(0.5f);

        float y = transform.position.y;

        GetComponent<Animator>().SetTrigger("fire");
        yield return new WaitForSeconds(0.5f);
        //while (tParam < 1)
        //{
        //    tParam += Time.deltaTime * speedModifier;

        //    float ymove = Mathf.Lerp(0.0f, 0.67f, tParam);
        //    transform.position = new Vector3(transform.position.x, y - ymove, transform.position.z);

        //    float scale = Mathf.Lerp(1.0f, 0.67f, tParam);
        //    transform.localScale = new Vector3(1.0f, 0.1f, 1.0f);
        //    print(transform.localScale);
        //    yield return null;// new WaitForEndOfFrame();
        //}

        //tParam = 0.0f;
        //y = transform.position.y;
        //while (tParam < 1)
        //{
        //    tParam += Time.deltaTime * speedModifier;

        //    float ymove = Mathf.Lerp(0.0f, 0.67f, tParam);
        //    transform.position = new Vector3(transform.position.x, y + ymove, transform.position.z);

        //    float scale = Mathf.Lerp(0.67f, 1.0f, tParam);
        //    transform.localScale = new Vector3(1.0f, scale, 1.0f);
        //    yield return null;// new WaitForEndOfFrame();
        //}

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

    private void BombHit(GameObject target, Vector2 bomb_pos)
    {
        if (!target.IsDestroyed() &&
            Vector2.Distance(new Vector2(target.transform.position.x, target.transform.position.y), bomb_pos) <= 1)
        {
            target.GetComponentInChildren<HitHealth>().GetDamage(1);
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