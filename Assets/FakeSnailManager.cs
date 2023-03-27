using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeSnailManager : MonoBehaviour
{
    [SerializeField] private HitHealth _hitHealth;
    [SerializeField] private Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_hitHealth.health <= 0)
        {
            _animator.SetBool("is_dead", true);
        }
    }
}
