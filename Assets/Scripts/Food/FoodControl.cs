using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class FoodControl : MonoBehaviour
{
    [SerializeField] private GameObject text;
    private Vector2 pos;

    private Camera _camera;
    private bool color_changed = false;
    private void Start()
    {
        _camera = Camera.main;
        pos = new Vector2(Mathf.CeilToInt(transform.position.x), Mathf.CeilToInt(transform.position.y));
    }

    private void Update()
    {
        if (GameObject.Find("GrowthDemoController").GetComponent<GrowthDemo>().Position2Growthed(pos) || GameObject.Find("GrowthDemoController").GetComponent<GrowthDemo>().FakeGrowthed(pos))
        {
            if (!color_changed)
            {
                changeColor();
                text.SetActive(true);
            }
        }
        DetectObjectWithRaycast();
    }

    public void changeColor()
    {
        color_changed = true;
        GetComponent<SpriteRenderer>().color = new Color(0.0f, 1.0f, 0.0f, 1);
    }

    public Vector2 getPos()
    {
        return pos;
    }

    public void DetectObjectWithRaycast()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.name == "cherry" && color_changed)
                    text.SetActive(false);
            }
        }
    }


}