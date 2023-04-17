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
    [SerializeField] public GameObject restoreEffect;
    [SerializeField] private HitHealth _hitHealth;
    [SerializeField] private Image eatIndicator;
    private Coroutine eatIndicatorCoroutine;
    public GameObject currentGrass;
    private Rigidbody _rigidbody;

    private void Start()
    {
        _snailExpManager = GetComponent<SnailExpManager>();
        eatIndicator.fillAmount = 0;
        currentGrass = null;
        _rigidbody = transform.parent.gameObject.GetComponent<Rigidbody>();
        eatIndicatorCoroutine = null;
    }

    private void Update()
    {
        // if (_rigidbody.velocity.magnitude > 1 && eatIndicator.fillAmount > 0)
        // {
        //     eatEffect.SetActive(false);
        //     StopCoroutine(eatIndicatorCoroutine);
        //     eatIndicator.fillAmount = 0;
        // }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Hyphae") && other.transform.parent.GetComponentInChildren<GroundTileManager>().growthed) {
            if (Time.time - collisionTime > time_eat_hyphae)
            {
                AudioClip clip = Resources.Load<AudioClip>("Audio/Bite");
                AudioSource.PlayClipAtPoint(clip, GameProgressControl.audioListenerPos, 0.5f);
                _snailExpManager.AddExpPoints(1);
                // other.gameObject.SetActive(false);
                // other.transform.parent.GetComponentInChildren<GroundTileManager>().growthed = false;
                other.transform.parent.GetComponentInChildren<GroundTileManager>().RemoveGrowthed();
                eatEffect.SetActive(false);
            }
            else
            {
                if (other.gameObject.GetComponent<SpriteRenderer>() != null)
                {
                    Color tmp = other.gameObject.GetComponent<SpriteRenderer>().color;
                    tmp.a = (time_eat_hyphae - (Time.time - collisionTime)) / time_eat_hyphae;
                    other.gameObject.GetComponent<SpriteRenderer>().color = tmp;
                }
                // foreach (Transform small_hyphae in other.transform)
                // {
                //     Color tmp = small_hyphae.gameObject.GetComponent<SpriteRenderer>().color;
                //     tmp.a = (time_eat_hyphae - (Time.time - collisionTime)) / time_eat_hyphae;
                //     small_hyphae.gameObject.GetComponent<SpriteRenderer>().color = tmp;
                // }
            }
            
        } 
        else if (other.gameObject.CompareTag("GrassHide"))
        {
            GetComponent<HitHealth>().SetHealthRestoreRate(0.7f);
            restoreEffect.SetActive(true);
            currentGrass = other.gameObject;
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
            if (other.gameObject.GetComponent<SpriteRenderer>() != null)
            {
                Color tmp = other.gameObject.GetComponent<SpriteRenderer>().color;
                tmp.a = 1;
                other.gameObject.GetComponent<SpriteRenderer>().color = tmp;
            }
            // foreach (Transform small_hyphae in other.transform)
            // {
            //     Color tmp = small_hyphae.gameObject.GetComponent<SpriteRenderer>().color;
            //     tmp.a = 1;
            //     small_hyphae.gameObject.GetComponent<SpriteRenderer>().color = tmp;
            // }
            eatEffect.SetActive(false);
            StopCoroutine(eatIndicatorCoroutine);
            eatIndicator.fillAmount = 0;
        }
        else if (other.gameObject.CompareTag("GrassHide"))
        {
            GetComponent<HitHealth>().SetHealthRestoreRate(0.1f);
            restoreEffect.SetActive(false);
            currentGrass = null;
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
