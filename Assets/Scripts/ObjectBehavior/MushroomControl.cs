using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomControl : MonoBehaviour
{
    [SerializeField]
    bool isChosen;
    Camera _camera;

    // Start is called before the first frame update
    private void Awake()
    {
        isChosen = false;
        _camera = Camera.main;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isChosen)
            {
                print("choosen state");
                Vector3 Worldpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                print(Worldpos);
                if (Worldpos.x >= 0 && Worldpos.y >= 0 && Worldpos.x <= 30 && Worldpos.y <= 30)
                {
                    Vector2 pos = new Vector2(Mathf.FloorToInt(Worldpos.x + 0.5f), Mathf.FloorToInt(Worldpos.y + 0.5f));
                    GrowthDemo growthDemo = GameObject.Find("GrowthDemoController").GetComponent<GrowthDemo>();
                    if (!growthDemo.Position2Growthed(pos) && !growthDemo.FakeGrowthed(pos))
                    {
                        Instantiate(Resources.Load<GameObject>("Prefabs/Objects/Food"), new Vector3(pos.x, pos.y, -2.0f), Quaternion.identity);
                        growthDemo.Position2GroundManager(pos).SetGrowthed();
                    }
                }
                isChosen = false;
            }
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider!=null && hit.collider.tag == "Mushroom")
                {
                    isChosen = !isChosen;
                    print(isChosen);
                }

            }
        }
    }
}
