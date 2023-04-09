using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SnailTrigger : MonoBehaviour
{
    private float collisionTime;
    public float time_eat_hyphae = 3f;
    private SnailExpManager _snailExpManager;

    [SerializeField] private GameObject eatEffect;
    [SerializeField] private GameObject restoreEffect;
    [SerializeField] private HitHealth _hitHealth;
    [SerializeField] private Image eatIndicator;
    private Coroutine eatIndicatorCoroutine;

    private void Start()
    {
        _snailExpManager = GetComponent<SnailExpManager>();
        eatIndicator.fillAmount = 0;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Hyphae") && other.transform.parent.GetComponentInChildren<GroundTileManager>().growthed) {
            if (Time.time - collisionTime > time_eat_hyphae)
            {
                AudioClip clip = Resources.Load<AudioClip>("Audio/Bite");
                AudioSource.PlayClipAtPoint(clip, transform.position);
                _snailExpManager.AddExpPoints(1);
                // other.gameObject.SetActive(false);
                // other.transform.parent.GetComponentInChildren<GroundTileManager>().growthed = false;
                other.transform.parent.GetComponentInChildren<GroundTileManager>().RemoveGrowthed();
                eatEffect.SetActive(false);
            }
            else
            {
                foreach (Transform small_hyphae in other.transform)
                {
                    Color tmp = small_hyphae.gameObject.GetComponent<SpriteRenderer>().color;
                    tmp.a = (time_eat_hyphae - (Time.time - collisionTime)) / time_eat_hyphae;
                    small_hyphae.gameObject.GetComponent<SpriteRenderer>().color = tmp;
                }
            }
            
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
            eatIndicatorCoroutine = StartCoroutine(StartEatIndicator());
        }
    }
    
    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.CompareTag("Hyphae"))
        {
            foreach (Transform small_hyphae in other.transform)
            {
                Color tmp = small_hyphae.gameObject.GetComponent<SpriteRenderer>().color;
                tmp.a = 1;
                small_hyphae.gameObject.GetComponent<SpriteRenderer>().color = tmp;
            }
            eatEffect.SetActive(false);
            StopCoroutine(eatIndicatorCoroutine);
            eatIndicator.fillAmount = 0;
        }
        else if (other.gameObject.CompareTag("GrassHide"))
        {
            GetComponent<HitHealth>().SetHealthRestoreRate(0.1f);
            restoreEffect.SetActive(false);
        }
    }

    private IEnumerator StartEatIndicator()
    {
        float progress = time_eat_hyphae;
        eatIndicator.fillAmount = 1;

        while (progress > 0)
        {
            progress -= Time.deltaTime;
            eatIndicator.fillAmount = progress / time_eat_hyphae;
            yield return null;
        }
    } 

}
