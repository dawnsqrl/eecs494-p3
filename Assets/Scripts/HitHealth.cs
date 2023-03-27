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
    private void Start()
    {
        health = maxHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(enemyTag))
        {
            if (health > 0)
            {
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

    private IEnumerator HitEffect()
    {
        _spriteRenderer.color = new Color32(0xFF, 0x00, 0x00, 0xFF);
        yield return new WaitForSeconds(0.5f);
        _spriteRenderer.color = new Color32(0xFF, 0xFF, 0xFF, 0xFF);
    }
}