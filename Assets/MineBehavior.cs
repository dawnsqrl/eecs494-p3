using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    float damage_radius = 2.0f;
    int damage = 3;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Citizen")){
            //TODO: mine explosion effect
            foreach (var citizen in CitizenControl.citizenList)
            {
                if (citizen != null)
                {
                    if (Vector3.Distance(citizen.transform.position, transform.position) < damage_radius)
                    {
                        citizen.GetComponentInChildren<HitHealth>().ReduceHealth(damage);
                    }
                }
            }
        }
       
      
    }



}
