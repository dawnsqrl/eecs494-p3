using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SnailExpManager : MonoBehaviour
{
    [SerializeField] private int nextLevelExp;
    [SerializeField] public int currentExp;
    [SerializeField] private GameObject expBar, upgradeIcon;
    [SerializeField] private GameObject levelUpBanner;
    [SerializeField] private GameObject optionsBanner;
    [SerializeField] private GameObject mineIcon;
    [SerializeField] private GameObject sprintIcon, skillsChooseCanvas;
    [SerializeField] private GameObject sheild;
    private Controls _controls;
    private Controls.PlayerActions _playerActions;
    private int pendingLevelUps;
    private bool canSelect;
    private SnailSprintManager _snailSprintManager;
    private SnailWeapon _snailWeapon;

    [SerializeField] private Transform[] routes;

    private Vector2 objectPosition;
    public GameObject title;

    float tParam = 0f, speedModifier = 0.5f;
    bool coroutineAllowed = true;

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
        //levelUpBanner.SetActive(false);
        optionsBanner.SetActive(false);
        pendingLevelUps = 0;
        _controls = new Controls();
        _playerActions = _controls.Player;
        canSelect = false;
        sheild = transform.parent.gameObject.transform.Find("Shield").gameObject;

        skillsChooseCanvas.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            tParam = 0f;
            speedModifier = 0.5f;
            coroutineAllowed = true;
            StartCoroutine(levelUpAnimation());
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            StartCoroutine(skillChooseAnimation());
        }
    }

    IEnumerator skillChooseAnimation()
    {
        skillsChooseCanvas.SetActive(true);
        //yield return new WaitForSeconds(0.5f);
        float progress = 0.0f;
        float speed = 0.5f;
        Vector3 initial_pos = upgradeIcon.transform.position;
        Vector3 dest_pos = transform.position;
        

        while (progress < 1)
        {
            progress += Time.deltaTime * speed;

            //title.transform.position = objectPosition;
            float scale = Mathf.Lerp(0.0f, 1.0f, progress);
            skillsChooseCanvas.transform.localScale = new Vector3(scale, scale, scale);

            Vector3 new_position = Vector3.Lerp(initial_pos, dest_pos, progress);
            skillsChooseCanvas.transform.position = new_position;

            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator levelUpAnimation()
    {
        yield return new WaitForSeconds(0.0f);
        title.SetActive(true);
        GetComponent<TextRevealer>().RevealText(title);
        yield return new WaitForSeconds(4.0f);

        coroutineAllowed = false;
        Vector2 p0 = routes[0].position;
        Vector2 p1 = routes[1].position;
        Vector2 p2 = routes[2].position;
        Vector2 p3 = routes[3].position;

        while (tParam < 1)
        {
            if (tParam > 0.9)
                upgradeIcon.SetActive(true);
            tParam += Time.deltaTime * speedModifier;

            objectPosition = Mathf.Pow(1 - tParam, 3) * p0 + 3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 + 3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 + Mathf.Pow(tParam, 3) * p3;

            title.transform.position = objectPosition;
            float scale = Mathf.Lerp(1.0f, 0.1f, tParam);
            title.transform.localScale = new Vector3(scale, scale, scale); 
            yield return new WaitForEndOfFrame();
        }

        tParam = 0f;

        coroutineAllowed = true;
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
        }else if (option == 3) {
            if (!sheild.activeSelf) {
                sheild.SetActive(true);
            }
            else {
                sheild.GetComponent<ShieldBehavior>().GetFullHP();
            }
        }
        // add more health and eat speed
        GetComponent<HitHealth>().health += 5;
        GetComponent<HitHealth>().maxHealth += 5;
        GetComponent<SnailTrigger>().time_eat_hyphae *= 0.8f;
        optionsBanner.SetActive(false);
    }

}
