using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    float damage_radius = 2.0f;
    int damage = 3;
    Animator animator;
    bool explode_lock = false;
    float delay_time = 3f;
    float start_time;

   
    private void Start()
    {
        animator = GetComponent<Animator>();
        start_time = Time.time;
    }

    private void Update()
    {
        if (Time.time - start_time > delay_time) {
            if (!explode_lock) {
                explode_lock = true;
                animator.SetTrigger("explode");
                AudioClip clip = Resources.Load<AudioClip>("Audio/Explosion");
                AudioSource.PlayClipAtPoint(clip, transform.position);
                StartCoroutine(DestoryAll(gameObject));
            }
        }
    }


    IEnumerator DestoryAll(GameObject gameObject) {
        yield return new WaitForSeconds(0.4f);
        print("step 1");
        if (CitizenControl.citizenList != null)
        {
            foreach (var citizen in CitizenControl.citizenList)
            {
                Debug.Log(citizen.name);
                if (citizen != null)
                {
                    if (Vector3.Distance(citizen.transform.position, transform.position) < damage_radius)
                    {
                        citizen.GetComponentInChildren<HitHealth>().GetDamage(damage);
                    }
                }
            }
        }
        print("step 2");
        GameObject nearestBuilding = BuildingController.NearestBuilding(transform.position, false);
        while (nearestBuilding != null && Vector2.Distance(new Vector2(transform.position.x, transform.position.y), new Vector2(nearestBuilding.transform.position.x, nearestBuilding.transform.position.y))<damage_radius) {
            Destroy(nearestBuilding);
            yield return null;
            nearestBuilding= BuildingController.NearestBuilding(transform.position, false);
        }
        print("step 3");
        List<Vector2> tile_list = Get_tiles_in_range(new Vector2(transform.position.x, transform.position.y), damage_radius);
        foreach (Vector2 pos in tile_list) {
            if (GridManager._tiles.ContainsKey(pos)) {
                GridManager._tiles[pos].GetComponentInChildren<GroundTileManager>().SetMucus();
                GridManager._tiles[pos].GetComponentInChildren<GroundTileManager>().RemoveGrowthed();
            }
        }
        print("step 4");
        yield return new WaitForSeconds(0.4f);
        Destroy(gameObject);
    }

    List<Vector2> Get_tiles_in_range(Vector2 pos, float radius) {
        List<Vector2> pos_list = new List<Vector2>();
        for(int i=Mathf.RoundToInt(pos.x-radius);i< Mathf.RoundToInt(pos.x + radius); ++i) {
            for(int j= Mathf.RoundToInt(pos.y - radius); j < Mathf.RoundToInt(pos.y + radius); ++j) {
                if (Vector2.Distance(new Vector2(i, j), pos) < radius) {
                    pos_list.Add(new Vector2(i, j));
                }
            }
        }
        return pos_list;
    }

}
