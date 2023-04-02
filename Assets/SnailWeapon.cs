using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailWeapon : MonoBehaviour
{
    [SerializeField] private int current_mines;

    [SerializeField] GameObject mines;
    // Start is called before the first frame update
    void Start()
    {
        current_mines = 5;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)) {
            if (current_mines > 0) {
                Instantiate(mines,transform.position,Quaternion.identity);
                current_mines -= 1;
            }
        }
    }
}
