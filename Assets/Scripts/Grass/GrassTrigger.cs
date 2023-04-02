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
    private bool deadAnimBegan;
    void Start()
    {
        _animator = transform.Find("Height").GetComponent<Animator>();
        _grassSuound = Resources.Load<AudioClip>("Audio/grass");
        _audioListenerTransform = GameObject.Find("AudioListener").transform;
        int id = UnityEngine.Random.Range(1, 4);
        Sprite grass_sprite = Resources.Load<Sprite>("Sprites/Theme/GrassHide/GrassHide" + id.ToString());
        _spriteRenderer_mask.sprite = grass_sprite;
        _spriteRenderer_trans.sprite = grass_sprite;
        deadAnimBegan = false;

    }

    private void Update()
    {
        // if this grid is marked
        Vector2 loc = new Vector2((int)transform.position.x, (int)transform.position.y);
        if (GridManager._tiles.ContainsKey(loc) && GridManager._tiles[loc].GetComponentInChildren<GroundTileManager>().growthed)
        {
            if (!deadAnimBegan)
            {
                deadAnimBegan = true;
                StartCoroutine(DestroyWithAnim(gameObject));
                // Destroy(gameObject);
            }
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
    
    private IEnumerator DestroyWithAnim(GameObject _gameObject)
    {
        _animator.SetTrigger("destroy");
        yield return new WaitForSeconds(1);
        Destroy(_gameObject);
    }
}
