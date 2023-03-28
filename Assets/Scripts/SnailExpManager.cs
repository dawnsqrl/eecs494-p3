using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailExpManager : MonoBehaviour
{
    [SerializeField] private int nextLevelExp;
    [SerializeField] public int currentExp;
    [SerializeField] private GameObject expBar;

    private void Start()
    {
        expBar.GetComponent<SpriteRenderer>().size =
            new Vector2((float)currentExp / (float)nextLevelExp * 10, expBar.GetComponent<SpriteRenderer>().size.y);
    }

    public void AddExpPoints()
    {
        currentExp++;
        expBar.GetComponent<SpriteRenderer>().size =
            new Vector2((float)currentExp / (float)nextLevelExp * 10, expBar.GetComponent<SpriteRenderer>().size.y);
        if (currentExp == nextLevelExp)
        {
            // TODO: Activate next skill
            currentExp = 0;
        }
    }

}
