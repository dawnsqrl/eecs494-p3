using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailLongDistanceAttack : MonoBehaviour
{
    Vector3 baseCarDirection;

    private void Awake()
    {
        EventBus.Subscribe<BaseCarDirectionEvent>(e => baseCarDirection = e.direction);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            StartCoroutine(StartAttack());
        }
    } 

    IEnumerator StartAttack()
    {
        float progress = 0.0f;
        float attackRange = 5.0f;
        float speed = 0.5f;
        bool attacked = false;

        yield return null;
        GameObject longRangeMucus = Instantiate(Resources.Load<GameObject>("Prefabs/Mucus"), transform.position,
            Quaternion.identity);
        Vector3 init_pos = transform.position;
        Vector3 dest_pos = transform.position + baseCarDirection.normalized * attackRange; // set main direction?
        while (progress < 1)
        {
            progress += Time.deltaTime * speed;

            Vector3 new_position = Vector3.Lerp(init_pos, dest_pos, progress);
            longRangeMucus.transform.position = new_position;

            yield return new WaitForEndOfFrame();

            //GameObject target = findTarget(longRangeMucus.transform.position);
            //if (target != null)
            //{
            //    target.GetComponentInChildren<HitHealth>().GetDamage(1);
            //    progress = 1.1f;
            //    Destroy(longRangeMucus);
            //    attacked = true;
            //}   
        }

        if (attacked)
        {
            //GridManager._tiles[pos].GetComponentInChildren<GroundTileManager>().SetMucus();
            //GridManager._tiles[pos].GetComponentInChildren<GroundTileManager>().RemoveGrowthed();
        }
    }
}
