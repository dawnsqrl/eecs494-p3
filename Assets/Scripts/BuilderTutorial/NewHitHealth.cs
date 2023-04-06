using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class NewHitHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth;
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
        StartCoroutine(HitEffect());
    }

    private void Update()
    {
        
    }  


    private IEnumerator HitEffect()
    {
        while(true)
        {
            if (!hitlock)
            {
                hitlock = true;
                _spriteRenderer.color = new Color32(0xFF, 0x00, 0x00, 0xFF);
                yield return new WaitForSeconds(UnityEngine.Random.Range(0.3f, 0.5f));
                canGetHit = true;
                _spriteRenderer.color = new Color32(0xFF, 0xFF, 0xFF, 0xFF);
                yield return new WaitForSeconds(UnityEngine.Random.Range(0.3f, 0.5f));
                hitlock = false;
            }
            else
            {
                yield return null;
            }
        }
        
    }

}