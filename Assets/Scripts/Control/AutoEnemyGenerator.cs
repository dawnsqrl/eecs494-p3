using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CodeMonkey.Utils;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class AutoEnemyGenerator : MonoBehaviour
{
    [SerializeField] private int maxUnit = 1;
    private List<GameObject> unitList;
    private int onGenerationSnailNUm;
    private TextMeshPro _mesh;
    private bool canGenerate;
    private Animator _animator;
    private bool deadAnimBegan;
    private void Start()
    {
        unitList = new List<GameObject>();
        onGenerationSnailNUm = 0;
        _mesh = GetComponentInChildren<TextMeshPro>();
        _mesh.text = (maxUnit - onGenerationSnailNUm).ToString();
        canGenerate = true;
        _animator = GetComponentInChildren<Animator>();
        deadAnimBegan = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BaseCar"))
        {
            // trigger anim
            // generate a snail
            while (onGenerationSnailNUm < maxUnit)
            {
                if (canGenerate)
                {
                    canGenerate = false;
                    StartCoroutine(GenerateNewUnit());
                }
            }
        }
    }

    private void Update()
    {
        for (int i = 0; i < unitList.Count; i++)
        {
            if (unitList[i].IsDestroyed())
            {
                unitList.RemoveAt(i);
                onGenerationSnailNUm--;
            }
        }

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

    private IEnumerator GenerateNewUnit()
    {
        GameObject enemy = Instantiate(Resources.Load<GameObject>("Prefabs/Objects/LittleSnail"), transform.position, Quaternion.identity);
        enemy.GetComponent<UnitRTS>().MoveTo(transform.position);
        AutoEnemyControl.autoSnails_queue.Add(enemy);
        unitList.Add(enemy);
        onGenerationSnailNUm++;
        _mesh.text = (maxUnit - onGenerationSnailNUm).ToString();
        // yield return new WaitForSeconds(3);
        canGenerate = true;
        yield return null;
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
