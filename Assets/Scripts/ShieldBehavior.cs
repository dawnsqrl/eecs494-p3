using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class ShieldBehavior : MonoBehaviour
{
    public float max_HP = 20;
    public float HP;
    public float cycle = 5;
    SpriteRenderer renderer;
    bool render_lock = false;

    public int numShield;

    bool canShield = false, activated = false;
    [SerializeField] private GameObject shieldGameObject;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private SpriteRenderer healthBar;

    private float original_bar_length;

    // Update is called once per frame
    private void Awake()
    {
        EventBus.Subscribe<SnailShieldEvent>(_ => Shield());
    }

    private void Start()
    {
        HP = max_HP;
        renderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        shieldGameObject.SetActive(false);
        numShield = 0;
        _text.text = numShield.ToString();
        original_bar_length = healthBar.size.x;
        healthBar.size =
            new Vector2((float)HP / (float)max_HP * original_bar_length, healthBar.size.y);
    }

    void Update()
    {
        renderer.color = new Color(1, 1, 1, (80 * Mathf.Sin(2 * Mathf.PI * Time.time / cycle) + 160) / 255);
        if (canShield && activated)
        {
            
            if (HP < 0)
            {
                shieldGameObject.SetActive(false);
                activated = false;
            }
        }
    }

    public void EnableShield()
    {
        canShield = true;
    }

    private void Shield()
    {
        if (numShield > 0)
        {
            numShield--;
            _text.text = numShield.ToString();
            activated = true;
            shieldGameObject.SetActive(true);
            HP = max_HP;
            healthBar.size =
                new Vector2((float)HP / (float)max_HP * original_bar_length, healthBar.size.y);
        }
    }

    public void ReduceHP(float cnt) {
        if (canShield) {
            HP -= cnt;
            healthBar.size =
                new Vector2((float)HP / (float)max_HP * original_bar_length, healthBar.size.y);
        }
            
    }

    public void AddShield()
    {
        numShield++;
        _text.text = numShield.ToString();
    }
}
