using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SnailExpManager : MonoBehaviour
{
    [SerializeField] private int nextLevelExp;
    [SerializeField] public int currentExp;
    [SerializeField] private GameObject expBar;
    [SerializeField] private GameObject levelUpBanner;
    [SerializeField] private GameObject optionsBanner;
    [SerializeField] private GameObject mineIcon;
    [SerializeField] private GameObject sprintIcon;
    private Controls _controls;
    private Controls.PlayerActions _playerActions;
    private int pendingLevelUps;
    private bool canSelect;
    private SnailSprintManager _snailSprintManager;
    private SnailWeapon _snailWeapon;

    private void Awake()
    {
        EventBus.Subscribe<SnailLevelUpEvent>(_ => LevelUp());
        EventBus.Subscribe<SnailLevelupOptionEvent_1>(_ => OptionSelect(1));
        EventBus.Subscribe<SnailLevelupOptionEvent_2>(_ => OptionSelect(2));
        _snailSprintManager = transform.parent.gameObject.GetComponent<SnailSprintManager>();
        _snailWeapon = transform.parent.gameObject.GetComponent<SnailWeapon>();
    }

    private void Start()
    {
        expBar.GetComponent<SpriteRenderer>().size =
            new Vector2((float)currentExp / (float)nextLevelExp * 10, expBar.GetComponent<SpriteRenderer>().size.y);
        levelUpBanner.SetActive(false);
        optionsBanner.SetActive(false);
        pendingLevelUps = 0;
        _controls = new Controls();
        _playerActions = _controls.Player;
        canSelect = false;
    }

    public void AddExpPoints(int exp)
    {
        currentExp += exp;
        if (currentExp >= nextLevelExp)
        {
            AudioClip clip = Resources.Load<AudioClip>("Audio/SnailLevelUp");
            AudioSource.PlayClipAtPoint(clip, transform.position);
            pendingLevelUps++;
            currentExp = 0;
        }
        expBar.GetComponent<SpriteRenderer>().size =
            new Vector2((float)currentExp / (float)nextLevelExp * 10, expBar.GetComponent<SpriteRenderer>().size.y);
        if (pendingLevelUps > 0 && !levelUpBanner.activeSelf)
        {
            levelUpBanner.SetActive(true);
        }
    }

    public void LevelUp()
    {
        if (pendingLevelUps > 0)
        {
            // active selection menu
            // wait for input
            optionsBanner.SetActive(true);
            canSelect = true;
            AudioClip clip = Resources.Load<AudioClip>("Audio/SnailLevelUp");
            AudioSource.PlayClipAtPoint(clip, transform.position);
            pendingLevelUps--;
        }
        if (pendingLevelUps <= 0 && levelUpBanner.activeSelf)
        {
            levelUpBanner.SetActive(false);
        }
    }

    public void OptionSelect(int option)
    {
        if (!canSelect)
        {
            return;
        }
        canSelect = false;
        if (option == 1)
        {
            if (!mineIcon.activeSelf)
            {
                mineIcon.SetActive(true);
            }
            _snailWeapon.AddMine(3);
        }
        else if (option == 2)
        {
            if (!_snailSprintManager.CanSprint())
            {
                _snailSprintManager.EnableSprint();
                sprintIcon.SetActive(true);
            }
            else
            {
                _snailSprintManager.AddSprintSpeed(2);
            }
        }
        // add more health and eat speed
        GetComponent<HitHealth>().health += 5;
        GetComponent<HitHealth>().maxHealth += 5;
        GetComponent<SnailTrigger>().time_eat_hyphae *= 0.8f;
        optionsBanner.SetActive(false);
    }

}
