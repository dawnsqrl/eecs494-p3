using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeMushroomManager : MonoBehaviour
{
    [SerializeField] private HitHealth _hitHealth;
    [SerializeField] private Animator _animator;

    private bool deadAnimBegan;
    // Start is called before the first frame update
    void Start()
    {
        deadAnimBegan = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_hitHealth.health <= 0)
        {
            if (!deadAnimBegan)
            {
                deadAnimBegan = true;
                StartCoroutine(DestroyWithAnim(gameObject));
                // Destroy(transform.parent.gameObject);
            }
        }
    }
    private IEnumerator DestroyWithAnim(GameObject _gameObject)
    {
        _animator.SetTrigger("destroy");
        Debug.Log("wait");
        yield return new WaitForSeconds(1);
        Debug.Log("wait end");
        Destroy(_gameObject);
    }
}
