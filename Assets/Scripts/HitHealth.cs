using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HitHealth : MonoBehaviour
{
    [SerializeField] public int maxHealth;
    [SerializeField] public int health;
    // [SerializeField] private string enemyTag;
    [SerializeField] private SpriteRenderer healthBar;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    // [SerializeField] private float time_eat_hyphae = 1f;
    [SerializeField] private float hit_cd_time = 0.5f;
    [SerializeField] private float health_recover_rate = 0.1f; //10 s one health
    [SerializeField] private List<String> enemyTagList;
    [SerializeField] private Animator _animator;
    private bool deadAnimBegan;

    private bool canGetHit;
    private float deltaHP = 0;
    private float original_bar_length;

    bool hitlock;

    private GameObject currentOpponent;
    

    private void Start()
    {
        hitlock = false;
        original_bar_length = healthBar.size.x;
        canGetHit = true;
        healthBar.size =
            new Vector2((float)health / (float)maxHealth * original_bar_length, healthBar.size.y);
        deadAnimBegan = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<HitHealth>() == null || !enemyTagList.Contains(other.gameObject.tag))
        {
            // if this object is not an opponent,
            return;
        }

        if (other.gameObject.GetComponent<HitHealth>().currentOpponent != null)
        {
            if (transform.parent != null && other.gameObject.GetComponent<HitHealth>().currentOpponent != transform.parent.gameObject)
            {
                // if this opponent is not a building, and the other's current opponent is not self
                // make this a current opponent
                GetComponent<HitHealth>().currentOpponent = other.transform.parent.gameObject;
            }
        }
        
        if (health > 0)
        {
            canGetHit = false;
            StartCoroutine(HitEffect(1));
        }
        if (health == 0)
        {
            EventBus.Publish(new TBaseCarDestroy());
            //EventBus.Publish(new BuilderTutorialSnailDeadEvent());
            if (gameObject.tag == "Building")
            {
                if (!deadAnimBegan)
                {
                    deadAnimBegan = true;
                    StartCoroutine(DestroyWithAnim(gameObject));
                    // Destroy(gameObject);
                }
            }  
            else 
            {
                Transform parent = transform.parent;
                if (parent.GetComponent<SpriteRenderer>() != null)
                {
                    parent.GetComponent<SpriteRenderer>().color = Color.red;
                }
                if (parent.GetComponent<GameEndTrigger>() != null)
                {
                    parent.GetComponent<GameEndTrigger>().TriggerDeath();
                }
                else
                {
                    if (!deadAnimBegan)
                    {
                        deadAnimBegan = true;
                        StartCoroutine(DestroyWithAnim(parent.gameObject));
                        // Destroy(parent.gameObject);
                    }
                }
            }
        }
    }

    private void Update()
    {
        if (CompareTag("Citizen") || CompareTag("LittleSnail"))
        {
            return;
        }
        RecoverHealth();
    }

    public void ReduceHealth(int cnt){
        if(health-cnt>0){
            health -= cnt;
            healthBar.size =
                new Vector2((float)health / (float)maxHealth * original_bar_length, healthBar.size.y);
        }else{
            if (!deadAnimBegan)
            {
                deadAnimBegan = true;
                StartCoroutine(DestroyWithAnim(transform.parent.gameObject));
                // Destroy(transform.parent.gameObject);
            }
        }
        
    }


    void RecoverHealth() {
        if (deltaHP > 1) {
            if (health + 1 <= maxHealth) {
                health += 1;
            }
            deltaHP = 0;
            healthBar.size =
                    new Vector2((float)health / (float)maxHealth * original_bar_length, healthBar.size.y);
        }
        else {
            deltaHP += health_recover_rate * Time.deltaTime;
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{

    //    if (collision.gameObject.CompareTag(enemyTag))
    //    {
    //        Debug.Log("hit" + enemyTag);
    //    }
    //}

    // private void OnTriggerStay(Collider other)
    // {
    //     if (other.gameObject.CompareTag("Hyphae"))
    //     {
    //         if (Time.time - collisionTime > time_eat_hyphae)
    //         {
    //             other.gameObject.SetActive(false);
    //         }
    //     }
    // }
    // private void OnCollisionStay(Collision collision)
    // {
    //     if(collision.gameObject.CompareTag("Hyphae")) {
    //         if (Time.time - collisionTime > time_eat_hyphae) {
    //             collision.gameObject.SetActive(false);
    //         }
    //     }
    // }

    public void GetDamage(int damage) {
        // if (health > 0)
        // {
        //     canGetHit = false;
        //     StartCoroutine(HitEffect());
        //     health -= 1;
        //     healthBar.size =
        //         new Vector2((float)health / (float)maxHealth * original_bar_length, healthBar.size.y);
        // }
        // if (health == 0)
        // {
        //     Transform parent = transform.parent;
        //     parent.GetComponent<SpriteRenderer>().color = Color.red;
        //     if (parent.GetComponent<GameEndTrigger>() != null)
        //     {
        //         parent.GetComponent<GameEndTrigger>().TriggerDeath();
        //     }
        //     
        // }
        if (health > 0)
        {
            canGetHit = false;
            StartCoroutine(HitEffect(damage));
        }
        if (health == 0)
        {
            //EventBus.Publish(new BuilderTutorialSnailDeadEvent());
            if (gameObject.tag == "Building")
            {
                if (!deadAnimBegan)
                {
                    deadAnimBegan = true;
                    StartCoroutine(DestroyWithAnim(gameObject));
                    // Destroy(gameObject);
                }
            }  
            else
            {
                Transform parent = transform.parent;
                if (parent.GetComponent<SpriteRenderer>() != null)
                {
                    parent.GetComponent<SpriteRenderer>().color = Color.red;
                }
                if (parent.GetComponent<GameEndTrigger>() != null)
                {
                    parent.GetComponent<GameEndTrigger>().TriggerDeath();
                }
                else
                {
                    if (!deadAnimBegan)
                    {
                        deadAnimBegan = true;
                        StartCoroutine(DestroyWithAnim(parent.gameObject));
                        // Destroy(parent.gameObject);
                    }
                }
            }
        }

    }
    private IEnumerator HitEffect(int damage)
    {
        if (!hitlock) {
            hitlock = true;
            _spriteRenderer.color = new Color32(0xFF, 0x00, 0x00, 0xFF);
            health -= damage;
            healthBar.size =
                new Vector2((float)health / (float)maxHealth * original_bar_length, healthBar.size.y);
            // if (gameObject.CompareTag("BaseCar"))
            // {
            //     GetComponent<BoxCollider>().enabled = false;
            // }
            yield return new WaitForSeconds(1f);
            // if (gameObject.CompareTag("BaseCar"))
            // {
            //     GetComponent<BoxCollider>().enabled = true;
            // }
            canGetHit = true;
            _spriteRenderer.color = new Color32(0xFF, 0xFF, 0xFF, 0xFF);
            hitlock = false;
        }
        else {
            yield return null;
        }
    }

    public void SetHealthRestoreRate(float rate)
    {
        health_recover_rate = rate;
    }

    public void SetCurrentOpponent(GameObject _opponent)
    {
        currentOpponent = _opponent;
    }

    private IEnumerator DestroyWithAnim(GameObject _gameObject)
    {
        _animator.SetTrigger("destroy");
        GetComponent<BoxCollider>().enabled = false;
        Debug.Log("wait");
        yield return new WaitForSeconds(1);
        Debug.Log("wait end");
        Destroy(_gameObject);
    }

}