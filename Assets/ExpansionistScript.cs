using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpansionistScript : MonoBehaviour
{

    Transform transform;
    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        GameObject tile = GridManager._tiles[GetSnailPos(transform.position.x, transform.position.y)];
        tile.GetComponentInChildren<Tile>().SetSnailMucus();
    }

    Vector2 GetSnailPos(float x, float y) {
        return new Vector2(Mathf.RoundToInt(x), Mathf.RoundToInt(y));
    }
}
