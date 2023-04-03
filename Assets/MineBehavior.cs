using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    float damage_radius = 5.0f;
    int damage = 3;
    Animator animator;
    bool explode_lock = false;

    private void Awake()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Citizen")){
            if (!explode_lock) {
                animator.SetTrigger("explode");
                transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("explode");
                AudioClip clip = Resources.Load<AudioClip>("Audio/Explosion");
                AudioSource.PlayClipAtPoint(clip, transform.position);
                StartCoroutine(DestoryAll(gameObject));
                explode_lock = true;
            }
            
        }
       
    }

    IEnumerator DestoryAll(GameObject gameObject) {
        yield return new WaitForSeconds(0.3f);
        foreach (var citizen in CitizenControl.citizenList)
        {
            Debug.Log(citizen.name);
            if (citizen != null)
            {
                if (Vector3.Distance(citizen.transform.position, transform.position) < damage_radius)
                {
                    citizen.GetComponentInChildren<HitHealth>().ReduceHealth(damage);
                }
            }
        }
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
   

}
