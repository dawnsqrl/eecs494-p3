using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCrossManager : MonoBehaviour
{
   private void OnTriggerStay(Collider other)
   {
      if (other.CompareTag("Citizen"))
      {
         Destroy(gameObject);
      }
   }
}
