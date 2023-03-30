using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassManager : MonoBehaviour
{
    [SerializeField] private List<Vector3> grassList;
    private void Awake()
    {
        grassList = new List<Vector3>();
        foreach (Transform child in transform)
        {
            grassList.Add(child.gameObject.transform.position);
        }
    }

    public bool CheckRange(Vector2 pos, int min_dis)
    {
        foreach (Vector3 grass in grassList)
        {
            if (Vector2.Distance(pos, new Vector2(grass.x, grass.y)) <= min_dis)
                return true;
        }
        return false;
    }

}
