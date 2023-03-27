using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeMushroomManager : MonoBehaviour
{
    [SerializeField] private HitHealth _hitHealth;
    // Start is called before the first frame update
    void Start()
    {
        // foreach (Transform tileTrans in transform)
        // {
        //     GroundTileManager _groundTileManager = tileTrans.Find("Tile_ground").gameObject.GetComponent<GroundTileManager>();
        //     if (_groundTileManager.growthed)
        //     {
        //         _groundTileManager.SetGrowthed();
        //     }
        // }
    }

    // Update is called once per frame
    void Update()
    {
        if (_hitHealth.health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
