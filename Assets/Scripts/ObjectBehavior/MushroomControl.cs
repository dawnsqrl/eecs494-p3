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
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider!=null && hit.collider.tag == "Mushroom")
                {
                    
                    isChosen = !isChosen;
                }

            }
        }
        if (isChosen)
        {
            //TODO: spawn seeds

        }
    }
}
