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

    private bool canGetHit;



    private void Start()
    {
        health = maxHealth;
        canGetHit = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag(enemyTag))
        {
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
                if (gameObject.tag == "Building")
                    Destroy(gameObject);
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
        _spriteRenderer.color = new Color32(0xFF, 0x00, 0x00, 0xFF);
        yield return new WaitForSeconds(0.5f);
        canGetHit = true;
        _spriteRenderer.color = new Color32(0xFF, 0xFF, 0xFF, 0xFF);
    }
}