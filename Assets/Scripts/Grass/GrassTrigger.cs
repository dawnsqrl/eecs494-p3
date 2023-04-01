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
    [SerializeField] private SpriteRenderer _spriteRenderer_mask;
    [SerializeField] private SpriteRenderer _spriteRenderer_trans;
    void Start()
    {
        _animator = transform.Find("Height").GetComponent<Animator>();
        _grassSuound = Resources.Load<AudioClip>("Sound/grass");
        _audioListenerTransform = GameObject.Find("AudioListener").transform;
        int id = UnityEngine.Random.Range(1, 4);
        Sprite grass_sprite = Resources.Load<Sprite>("Sprites/Theme/GrassHide/GrassHide" + id.ToString());
        _spriteRenderer_mask.sprite = grass_sprite;
        _spriteRenderer_trans.sprite = grass_sprite;

    }

    private void Update()
    {
        // if this grid is marked
        if (GridManager._tiles[new Vector2((int)transform.position.x, (int)transform.position.y)].GetComponentInChildren<GroundTileManager>().growthed)
        {
            Destroy(gameObject);
        }
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
