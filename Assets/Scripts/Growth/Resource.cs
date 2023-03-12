using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField] private int gap_time = 2;
    [SerializeField] private int growth_time = 3;

    private float resource = 100.0f;
    private float growth_amount = 1000.0f;
    private void Start()
    {
        StartCoroutine(PrintResource(gap_time));
        StartCoroutine(ResourceGrowth(growth_time));
    }

    private void Update()
    {
        
    }

    public void change_growth_amount(float new_level)
    {
        growth_amount = new_level;
    }

    public float get_growth_amount()
    {
        return growth_amount;
    }

    public void change_resource(float new_level)
    {
        resource = new_level;
    }

    public float get_resource()
    {
        return resource;
    }

    IEnumerator PrintResource(int gap_time)
    {
        while (true)
        {
            print(resource);
            yield return new WaitForSeconds(gap_time);
        }
        
    }

    IEnumerator ResourceGrowth(int growth_time)
    {
        while(true)
        {
            yield return new WaitForSeconds(growth_time);
            resource += growth_amount;
        }
        
    }
}