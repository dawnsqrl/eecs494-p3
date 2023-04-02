using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SnailTrigger : MonoBehaviour
{
    private float collisionTime;
    private float time_eat_hyphae = 3f;
    private SnailExpManager _snailExpManager;

    [SerializeField] private GameObject eatEffect;
    [SerializeField] private GameObject restoreEffect;

    private void Start()
    {
        _snailExpManager = GetComponent<SnailExpManager>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Hyphae") && Time.time - collisionTime > time_eat_hyphae) {
            _snailExpManager.AddExpPoints(1);
            other.gameObject.SetActive(false);
            other.transform.parent.GetComponentInChildren<GroundTileManager>().growthed = false;
            eatEffect.SetActive(false);
        } 
        else if (other.gameObject.CompareTag("GrassHide"))
        {
            GetComponent<HitHealth>().SetHealthRestoreRate(0.7f);
            restoreEffect.SetActive(true);
        }
       
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Hyphae"))
        {
            eatEffect.SetActive(true);
            collisionTime = Time.time;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.CompareTag("Hyphae"))
        {
            eatEffect.SetActive(false);
        }
        else if (other.gameObject.CompareTag("GrassHide"))
        {
            GetComponent<HitHealth>().SetHealthRestoreRate(0.1f);
            restoreEffect.SetActive(false);
        }
    }



}
