using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HitHealth : MonoBehaviour
{
   [SerializeField] private int health;
   [SerializeField] private string enemyTag;
   private void OnTriggerEnter(Collider other)
   {
      if (other.gameObject.CompareTag(enemyTag))
      {
         if (health > 1)
         {
            health -= 1;
         }
         else
         {
            Destroy(transform.parent.gameObject);
         }
      }
   }
}
