using System.Collections;
using System.Linq;
using UnityEngine;

public class HitHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] public int health;
    [SerializeField] private string enemyTag;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float time_eat_hyphae = 1f;
    [SerializeField] private float hit_cd_time = 0.5f;
    [SerializeField] private float health_recover_rate = 0.1f; //10 s one health


    private bool canGetHit;
    private float deltaHP = 0;
    private float hit_cd;

    bool hitlock;
    

    private void Start()
    {
        hitlock = false;
        hit_cd = hit_cd_time;
        canGetHit = true;
        healthBar.GetComponent<SpriteRenderer>().size =
            new Vector2((float)health / (float)maxHealth * 10, healthBar.GetComponent<SpriteRenderer>().size.y);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag(enemyTag))
        {
            if (health > 0)
            {
                canGetHit = false;
                StartCoroutine(HitEffect());
                if (gameObject.tag == "Building")
                {
                    if (hit_cd > 0)
                    {
                        hit_cd -= Time.deltaTime;
                    }
                    else
                    {
                        health -= 1;
                        hit_cd = hit_cd_time;
                    }
                }
                else {
                    health -= 1;
                }
                
                healthBar.GetComponent<SpriteRenderer>().size =
                    new Vector2((float)health / (float)maxHealth * 10, healthBar.GetComponent<SpriteRenderer>().size.y);
            }
            if (health == 0)
            {
                EventBus.Publish(new BuilderTutorialSnailDeadEvent());
                if (gameObject.tag == "Building")
                {
                    Destroy(gameObject);
                }  
                else
                {
                    Transform parent = transform.parent;
                    parent.GetComponent<SpriteRenderer>().color = Color.red;
                    if (parent.GetComponent<GameEndTrigger>() != null)
                    {
                        parent.GetComponent<GameEndTrigger>().TriggerDeath();
                    }
                }
            }
        }
    }

    private void Update()
    {
        RecoverHealth();
    }

    void RecoverHealth() {
        if (deltaHP > 1) {
            if (health + 1 <= maxHealth) {
                health += 1;
            }
            deltaHP = 0;
            healthBar.GetComponent<SpriteRenderer>().size =
                    new Vector2((float)health / (float)maxHealth * 10, healthBar.GetComponent<SpriteRenderer>().size.y);
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

    public void MushroomGetDamage() {
        if (health > 0)
        {
            canGetHit = false;
            StartCoroutine(HitEffect());
            health -= 1;
            healthBar.GetComponent<SpriteRenderer>().size =
                new Vector2((float)health / (float)maxHealth * 10, healthBar.GetComponent<SpriteRenderer>().size.y);
        }
        if (health == 0)
        {
            Transform parent = transform.parent;
            parent.GetComponent<SpriteRenderer>().color = Color.red;
            if (parent.GetComponent<GameEndTrigger>() != null)
            {
                parent.GetComponent<GameEndTrigger>().TriggerDeath();
            }
            
        }

    }
    private IEnumerator HitEffect()
    {
        if (!hitlock) {
            hitlock = true;
            _spriteRenderer.color = new Color32(0xFF, 0x00, 0x00, 0xFF);
            yield return new WaitForSeconds(0.5f);
            canGetHit = true;
            _spriteRenderer.color = new Color32(0xFF, 0xFF, 0xFF, 0xFF);
            hitlock = false;
        }
        else {
            yield return null;
        }
    }
}