using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassTrigger : MonoBehaviour
{
    private Animator _animator;
    private Transform _audioListenerTransform;
    private AudioClip _grassSuound;
    // Start is called before the first frame update
    void Start()
    {
        _animator = transform.Find("Height").GetComponent<Animator>();
        _grassSuound = Resources.Load<AudioClip>("Sound/grass");
        _audioListenerTransform = GameObject.Find("AudioListener").transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BaseCar"))
        {
            _animator.SetTrigger("SnailEnter");
            AudioSource.PlayClipAtPoint(_grassSuound, _audioListenerTransform.position);
        }
    }
}
