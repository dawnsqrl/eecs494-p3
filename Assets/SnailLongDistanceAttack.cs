using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailLongDistanceAttack : MonoBehaviour
{
    Vector3 baseCarDirection;
    public GridManager gridManager;

    float attackRange = 5.0f;
    bool canSpit = false;
    int damage = 1;

    private void Awake()
    {
        EventBus.Subscribe<BaseCarDirectionEvent>(e => baseCarDirection = e.direction);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    public void EnableSpit()
    {
        canSpit = true;
    }

    public void setDamage(int _damage)
    {
        damage = _damage;
    }

    public int getDamage()
    {
        return damage;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha8) && canSpit)
        {
            StartCoroutine(StartAttack());
        }
    } 

    IEnumerator StartAttack()
    {
        float progress = 0.0f;
        float speed = 0.5f;
        bool attacked = false;

        yield return null;
        GameObject longRangeMucus = Instantiate(Resources.Load<GameObject>("Prefabs/Spit"), transform.position,
            Quaternion.identity);
        longRangeMucus.transform.eulerAngles = new Vector3(0.0f, 0.0f, setRotation(baseCarDirection.normalized.x, baseCarDirection.normalized.y));
        Vector3 init_pos = transform.position;
        Vector3 dest_pos = transform.position + baseCarDirection.normalized * attackRange;
        print(dest_pos);
        while (progress < 1)
        {
            progress += Time.deltaTime * speed;

            Vector3 new_position = Vector3.Lerp(init_pos, dest_pos, progress);
            if (new_position.x > 49.0f || new_position.y > 49.0f || new_position.x < 0.0f || new_position.y < 0.0f)
                progress = 1.1f;
            else
            {
                longRangeMucus.transform.position = new_position;
            }
            yield return new WaitForEndOfFrame();

            GameObject target = findTarget(longRangeMucus.transform.position);
            if (target != null)
            {
                target.GetComponentInChildren<HitHealth>().GetDamage(damage);
                progress = 1.1f;
                Destroy(longRangeMucus);
                attacked = true;
            }   
        }

        if (!attacked)
        {
            Destroy(longRangeMucus);
            Vector2 pos = new Vector2(Mathf.FloorToInt(dest_pos.x), Mathf.CeilToInt(dest_pos.y)); // may be uncorrect
            gridManager.GetTileAtPosition(pos).GetComponentInChildren<GroundTileManager>().SetMucus();
            gridManager.GetTileAtPosition(pos).GetComponentInChildren<GroundTileManager>().RemoveGrowthed();
        }
    }

    private GameObject findTarget(Vector3 pos)
    {
        print("tagpos");
        print(pos);
        float detect_dis = 2.0f;
        GameObject nearestBuilding = BuildingController.NearestBuilding(pos);
        float building_dis;

        if (nearestBuilding == null)
            building_dis = 100.0f;
        else
        {
            if(nearestBuilding.name == "Mushroom")
                building_dis = Vector3.Distance(new Vector3(pos.x, pos.y, 0.0f), nearestBuilding.transform.position);
            else
                building_dis = Vector3.Distance(new Vector3(pos.x, pos.y, -2.0f), nearestBuilding.transform.position);
        }
            

        //print(nearestBuilding.transform.position);

        GameObject nearestCitizen = CitizenControl.NearestCitizen(pos);
        float citizen_dis;

        if (nearestCitizen == null)
            citizen_dis = 100.0f;
        else
            citizen_dis = Vector3.Distance(new Vector3(pos.x, pos.y, -2.0f), nearestCitizen.transform.position);

        if (building_dis < citizen_dis)
        {
            if (building_dis < detect_dis)
                return nearestBuilding;
            else
                return null;
        }
        else
        {
            if (citizen_dis < detect_dis)
                return nearestCitizen;
            else
                return null;
        } 
    }

    private float setRotation(float x, float y)
    {
        if (x == 0) { return y * 90.0f; }
        else if (y == 0) { if (x < 0) { return 180.0f; } }
        else if (x * y > 0) { return x > 0 ? 45.0f : -135.0f; }
        else { return x > 0 ? -45.0f : 135.0f; }

        return 0.0f;
    }
}
