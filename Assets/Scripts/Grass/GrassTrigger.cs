using System.Collections;
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
        bool isDead = false;
        Vector2 loc = new Vector2((int)transform.position.x, (int)transform.position.y);
        if (BasecarController.is_tutorial)
        {
            loc = new Vector2((int)transform.position.x + 60, (int)transform.position.y);
            isDead = CaveGridManager._tiles.ContainsKey(loc) &&
                     CaveGridManager._tiles[loc].GetComponentInChildren<GroundTileManager>().growthed;
        }
        else
        {
            isDead = GridManager._tiles.ContainsKey(loc) &&
                     GridManager._tiles[loc].GetComponentInChildren<GroundTileManager>().growthed;
        }
        // if this grid is marked

        if (isDead)
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
        AudioClip clip = Resources.Load<AudioClip>("Audio/BushDies");
        AudioSource.PlayClipAtPoint(clip, transform.position);
        GameObject snail = GameObject.Find("BaseCar").gameObject;
        if (snail.GetComponentInChildren<SnailTrigger>().currentGrass == _gameObject)
        {
            snail.GetComponentInChildren<HitHealth>().SetHealthRestoreRate(0.1f);
            snail.GetComponentInChildren<SnailTrigger>().restoreEffect.SetActive(false);
            snail.GetComponentInChildren<SnailTrigger>().currentGrass = null;
        }

        GameState.grassDestroyed++;
        Destroy(_gameObject);
    }
}