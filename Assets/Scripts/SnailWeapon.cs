using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SnailWeapon : MonoBehaviour
{
    [SerializeField] private int current_mines;

    [SerializeField] GameObject mines;

    [SerializeField] private TextMeshProUGUI _mesh;
    // Start is called before the first frame update
    private float remainCoolDownTime = 0;
    private float maxCoolDownTime = 1;
    [SerializeField] private Image mineCoolDownFog;
    private bool attacklock;

    private void Awake()
    {
        EventBus.Subscribe<SnailBombEvent>(_ => UseMine());
    }

    void Start()
    {
        current_mines = 0;
        attacklock = false;
    }

    public void UseMine()
    {
        if (current_mines > 0 && !attacklock)
        {
            attacklock = true;
            StartCoroutine(CoolDown());
            Instantiate(mines, transform.position, Quaternion.identity);
            current_mines -= 1;
            _mesh.text = current_mines.ToString();
        }
    }

    public void AddMine(int num)
    {
        current_mines += num;
        _mesh.text = current_mines.ToString();
    }

    private IEnumerator CoolDown()
    {
        remainCoolDownTime = maxCoolDownTime;
        while (remainCoolDownTime > 0)
        {
            remainCoolDownTime -= Time.deltaTime;
            mineCoolDownFog.fillAmount = remainCoolDownTime / maxCoolDownTime;
            yield return null;
        }

        attacklock = false;
        mineCoolDownFog.fillAmount = 0;
        remainCoolDownTime = 0;
    }
}
