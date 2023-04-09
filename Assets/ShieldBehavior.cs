using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBehavior : MonoBehaviour
{
    public float max_HP=20;
    public float HP;

    // Update is called once per frame
    private void Start()
    {
        HP = max_HP;
    }
    void Update()
    {
        if (HP < 0) {
            HP = max_HP;
            gameObject.SetActive(false);
        }
    }

    public void ReduceHP(float cnt) {
        HP -= cnt;
    }

    public void GetFullHP() {
        HP = max_HP;
    }
}
