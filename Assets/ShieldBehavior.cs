using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBehavior : MonoBehaviour
{
    public float max_HP=20;
    public float HP;
    public float cycle = 5;
    SpriteRenderer renderer;
    bool render_lock=false;

    // Update is called once per frame
    private void Start()
    {
        HP = max_HP;
        renderer= GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        renderer.color = new Color(1, 1, 1, (80 * Mathf.Sin(2 * Mathf.PI * Time.time / cycle) + 160)/255);
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

    //IEnumerator BlinkAnimation() {
    //    if (!render_lock) {
    //        render_lock = true;
    //        renderer.color=new Color(255,255,255, 52 * Mathf.Sin(2 * Mathf.PI * Time.time / cycle) + 151);
    //        yield return nu
    //        render_lock = false;
    //    }
    //    else {
    //        yield return null;
    //    }
    //}
}
