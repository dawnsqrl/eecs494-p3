using UnityEngine;

public class HitHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private int health;
    [SerializeField] private string enemyTag;
    [SerializeField] private GameObject healthBar;

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
                health -= 1;
                healthBar.GetComponent<SpriteRenderer>().size =
                    new Vector2((float)health / (float)maxHealth * 10, healthBar.GetComponent<SpriteRenderer>().size.y);
            }
            if (health == 0)
            {
                // Destroy(transform.parent.gameObject);
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