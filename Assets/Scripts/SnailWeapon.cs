using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class SnailWeapon : MonoBehaviour
{
    [SerializeField] private int current_mines;

    [SerializeField] GameObject mines;

    [SerializeField] private TextMeshProUGUI _mesh;
    // Start is called before the first frame update

    private void Awake()
    {
        EventBus.Subscribe<SnailBombEvent>(_ => UseMine());
    }

    void Start()
    {
        current_mines = 0;
    }
    
    public void UseMine()
    {
        if (current_mines > 0) {
            Instantiate(mines,transform.position,Quaternion.identity);
            current_mines -= 1;
            _mesh.text = current_mines.ToString();
        }
    }

    public void AddMine(int num)
    {
        current_mines += num;
        _mesh.text = current_mines.ToString();
    }
}
