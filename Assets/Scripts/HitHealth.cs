using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
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
    [SerializeField] private GameObject lowHealthEffect;
    private bool canSwitchOpponent;
    private bool startSwitchOpponent;

    private void Start()
    {
        hitlock = false;
        original_bar_length = healthBar.size.x;
        canGetHit = true;
        healthBar.size =
            new Vector2((float)health / (float)maxHealth * original_bar_length, healthBar.size.y);
        deadAnimBegan = false;
        startSwitchOpponent = false;
        canSwitchOpponent = true;
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
            if (transform.parent != null && other.gameObject.GetComponent<HitHealth>().currentOpponent !=
                transform.parent.gameObject)
            {
                return;
            }
        }

        if (transform.parent != null && GetComponent<HitHealth>().currentOpponent != other.transform.parent.gameObject)
        {
            // if this opponent is not a building, and the other's current opponent is not self
            // make this a current opponent
            if (!startSwitchOpponent)
            {
                startSwitchOpponent = true;
                GetComponent<HitHealth>().currentOpponent = other.transform.parent.gameObject;
                print("Change opponent successful to " + other.transform.parent.gameObject);
                StartCoroutine(SwitchOpponentCoolDown(0.3f));
            }
        }

        if (health > 0)
        {
            canGetHit = false;
            StartCoroutine(HitEffect(1));
        }

        if (health <= 0)
        {
            if (gameObject.CompareTag("BaseCar"))
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

        if (lowHealthEffect != null && health > maxHealth * 0.25f && lowHealthEffect.activeSelf)
        {
            lowHealthEffect.SetActive(false);
        }

        if (lowHealthEffect != null && health <= maxHealth * 0.25f && !lowHealthEffect.activeSelf)
        {
            lowHealthEffect.SetActive(true);
        }

        RecoverHealth();
    }

    public void ReduceHealth(int cnt)
    {
        //if the object is snail and has sheild

        if (health - cnt > 0)
        {
            health -= cnt;
            healthBar.size =
                new Vector2((float)health / (float)maxHealth * original_bar_length, healthBar.size.y);
        }
        else
        {
            if (!deadAnimBegan)
            {
                deadAnimBegan = true;
                StartCoroutine(DestroyWithAnim(transform.parent.gameObject));
                // Destroy(transform.parent.gameObject);
            }
        }
    }


    void RecoverHealth()
    {
        if (deltaHP > 1)
        {
            if (health + 1 <= maxHealth)
            {
                health += 1;
            }

            deltaHP = 0;
            healthBar.size =
                new Vector2((float)health / (float)maxHealth * original_bar_length, healthBar.size.y);
        }
        else
        {
            deltaHP += health_recover_rate * Time.deltaTime;
        }
    }


    public void GetDamage(int damage)
    {
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


        if (gameObject.tag == "BaseCar" && transform.parent.gameObject.transform.Find("Shield").transform
                .Find("ShieldObject").gameObject.activeSelf)
        {
            transform.parent.gameObject.transform.Find("Shield").gameObject.GetComponent<ShieldBehavior>()
                .ReduceHP(damage);
            return;
        }

        if (health > 0)
        {
            canGetHit = false;
            StartCoroutine(HitEffect(damage));
        }

        if (health <= 0)
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
        if (!hitlock)
        {
            if (gameObject.tag == "BaseCar" && transform.parent.gameObject.transform.Find("Shield").gameObject.transform
                    .Find("ShieldObject").gameObject.activeSelf)
            {
                hitlock = true;
                transform.parent.gameObject.transform.Find("Shield").gameObject.GetComponent<ShieldBehavior>()
                    .ReduceHP(damage);
                yield return new WaitForSeconds(1f);
                hitlock = false;
            }
            else
            {
                hitlock = true;
                _spriteRenderer.color = new Color32(0xFF, 0x00, 0x00, 0xFF);
                health -= damage;
                healthBar.size =
                    new Vector2((float)health / (float)maxHealth * original_bar_length, healthBar.size.y);
                // if (gameObject.CompareTag("BaseCar"))
                // {s
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
        }
        else
        {
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
        if (_gameObject.CompareTag("Citizen"))
        {
            GameState.smallMushroomKilled++;
        }
        else if (_gameObject.CompareTag("LittleSnail"))
        {
            GameState.smallSnailKilled++;
        }
        else if (_gameObject.CompareTag("Building"))
        {
            GameState.buildingDestroyed++;
        }

        Destroy(_gameObject);
    }

    public void AddSnailHealth(int curr_level, int max_health)
    {
        maxHealth = math.min(max_health, maxHealth += Mathf.Clamp(curr_level + 1, 0, 5));
        health = math.min(health, health += Mathf.Clamp(curr_level + 1, 0, 5));
    }

    private IEnumerator SwitchOpponentCoolDown(float time)
    {
        yield return new WaitForSeconds(time);
        startSwitchOpponent = false;
    }

    public int get_max_health()
    {
        return maxHealth;
    }
}