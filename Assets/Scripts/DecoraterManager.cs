using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoraterManager : MonoBehaviour
{
    [SerializeField] private List<Sprite> decoratorSpritesList;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform decorator in transform)
        {
            decorator.gameObject.GetComponent<SpriteRenderer>().sprite =
                decoratorSpritesList[Random.Range(0, decoratorSpritesList.Count)];
        }
    }
}
