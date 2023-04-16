using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class SnailExpManager : MonoBehaviour
{
    [SerializeField] private int nextLevelExp;
    [SerializeField] public int currentExp;
    [SerializeField] public int currentLevel;
    [SerializeField] private GameObject expBar, upgradeIcon;
    [SerializeField] private GameObject levelUpBanner;
    [SerializeField] private GameObject optionsBanner;
    [SerializeField] private GameObject option1Object, option2Object, option3Object;
    [SerializeField] private GameObject mineIcon, spitIcon, sprintIcon, shieldIcon;
    [SerializeField] private GameObject skillsChooseCanvas, title_pointer, skillCanvas;
    [SerializeField] private Sprite mineSprite, spitSprite, sprintSprite, shieldSprite;
    private GameObject shield;
    private Controls _controls;
    private Controls.PlayerActions _playerActions;
    private int pendingLevelUps;
    private bool canSelect, levelUpAnimationAllowed = true;
    private SnailSprintManager _snailSprintManager;
    private SnailLongDistanceAttack _snailSpitManager;
    public ShieldBehavior _snailShieldManager;
    private SnailWeapon _snailWeapon;

    [SerializeField] private Transform[] routes;
    [SerializeField] private Camera basecarCamera;
    [SerializeField] private Animator levelUpNoteAnimator;
    [SerializeField] private Animator addExpNoteAnimator;
    private int skillCounter = 0;

    private Vector2 objectPosition;
    public GameObject title;

    float tParam = 0f, speedModifier = 0.5f;
    bool coroutineAllowed = true;

    private float max_time_eat_hyphae = 1.0f;
    private int snail_max_health = 20;

    bool levelUpLock = false;

    [SerializeField] private TextMeshProUGUI levelValue;

    // option: 1 -> sprint, 2 -> mine, 3 -> shield, 4 -> spit
    private int randomOption1 = 0, randomOption2 = 0, randomOption3 = 0;

    private void Awake()
    {
        EventBus.Subscribe<SnailLevelUpEvent>(_ => LevelUp());
        EventBus.Subscribe<SnailLevelupOptionEvent_1>(_ => OptionSelect(1));
        EventBus.Subscribe<SnailLevelupOptionEvent_2>(_ => OptionSelect(2));
        EventBus.Subscribe<SnailLevelupOptionEvent_3>(_ => OptionSelect(3));
        EventBus.Subscribe<AddExpEvent>(e => AddExpPoints(e.exp));
        _snailSprintManager = transform.parent.gameObject.GetComponent<SnailSprintManager>();
        _snailSpitManager = transform.parent.gameObject.GetComponent<SnailLongDistanceAttack>();
        _snailWeapon = transform.parent.gameObject.GetComponent<SnailWeapon>();
    }

    private void Start()
    {
        expBar.GetComponent<SpriteRenderer>().size =
            new Vector2((float)currentExp / (float)nextLevelExp * 10, expBar.GetComponent<SpriteRenderer>().size.y);
        //levelUpBanner.SetActive(false);
        // optionsBanner.SetActive(false);
        pendingLevelUps = 0;
        _controls = new Controls();
        _playerActions = _controls.Player;
        canSelect = false;
        shield = transform.parent.gameObject.transform.Find("Shield").gameObject;

        skillsChooseCanvas.SetActive(false);
        currentExp = 0;
        currentLevel = 0;
    }

    private void Update()
    {
        if (levelValue != null)
            levelValue.text = $"Lv.{currentLevel}";
    }

    public void AddExpPoints(int exp)
    {
        currentExp += exp;
        addExpNoteAnimator.SetTrigger("AddExp");
        if (currentExp >= nextLevelExp)
        {
            AudioClip clip = Resources.Load<AudioClip>("Audio/SnailLevelUp");
            AudioSource.PlayClipAtPoint(clip, transform.position);
            pendingLevelUps++;
            currentExp = 0;
            if (coroutineAllowed)
                StartCoroutine(levelUpAnimation());
        }

        expBar.GetComponent<SpriteRenderer>().size =
            new Vector2((float)currentExp / (float)nextLevelExp * 10, expBar.GetComponent<SpriteRenderer>().size.y);
        //if (pendingLevelUps > 0 && !levelUpBanner.activeSelf)
        //{
        //    levelUpBanner.SetActive(true);
        //}
    }

    public void LevelUp()
    {
        if(!levelUpLock)
        {
            levelUpLock = true;
            if (pendingLevelUps > 0)
            {
                currentLevel++;
                nextLevelExp = 10 + 2 * currentLevel;
                // active selection menu
                // wait for input
                // optionsBanner.SetActive(true);
                // add more health and eat speed

                GetComponent<HitHealth>().AddSnailHealth(currentLevel, snail_max_health);
                GetComponent<SnailTrigger>().time_eat_hyphae = math.max(max_time_eat_hyphae, GetComponent<SnailTrigger>().time_eat_hyphae *= 0.8f);

                if (GetComponent<SnailTrigger>().time_eat_hyphae == max_time_eat_hyphae)
                    levelUpNoteAnimator.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Max Health +5";
                if (GetComponent<HitHealth>().get_max_health() == snail_max_health)
                    levelUpNoteAnimator.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Eat Speed +20%";

                if (GetComponent<SnailTrigger>().time_eat_hyphae == max_time_eat_hyphae && GetComponent<HitHealth>().get_max_health() == snail_max_health)
                    levelUpNoteAnimator.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "";

                levelUpNoteAnimator.SetTrigger("LevelUp");

                if (levelUpAnimationAllowed)
                {
                    StartCoroutine(skillChooseAnimation());
                    // upgradeIcon.SetActive(false);
                    // skillsChooseCanvas.SetActive(true);
                    // generate_random_skill_choose();
                    AudioClip clip = Resources.Load<AudioClip>("Audio/SnailLevelUp");
                    AudioSource.PlayClipAtPoint(clip, transform.position);
                }
            }

            if (pendingLevelUps <= 0)
            {
                levelUpLock = false;
                upgradeIcon.SetActive(false);
                title.SetActive(false);
            }
        }
        
    }

    IEnumerator skillChooseAnimation()
    {
        upgradeIcon.SetActive(false);
        levelUpAnimationAllowed = false;
        canSelect = false;
        skillsChooseCanvas.SetActive(true);
        //yield return new WaitForSeconds(0.5f);
        float progress = 0.0f;
        float speed = 0.5f;
        Vector3 initial_pos = upgradeIcon.transform.position;
        Vector3 dest_pos = transform.position;

        generate_random_skill_choose();

        // while (progress < 1)
        // {
        //     progress += Time.deltaTime * speed;
        //
        //     //title.transform.position = objectPosition;
        //     float scale = Mathf.Lerp(0.0f, 1.0f, progress);
        //     skillsChooseCanvas.transform.localScale = new Vector3(scale, scale, scale);
        //
        //     initial_pos = upgradeIcon.transform.position;
        //     dest_pos = transform.position;
        //
        //     Vector3 new_position = Vector3.Lerp(new Vector3(initial_pos.x, initial_pos.y, -2.0f),
        //         new Vector3(dest_pos.x, dest_pos.y, -2.0f), progress);
        //     skillsChooseCanvas.transform.position = new_position;
        //
        //     yield return new WaitForEndOfFrame();
        // }

        canSelect = true;
        
        //upgradeIcon.SetActive(false);
        yield return null;
    }

    IEnumerator levelUpAnimation()
    {
        tParam = 0f;
        speedModifier = 0.5f;

        //Vector3 title_ini_pos = title.transform.position;
        // GetComponent<TextRevealer>().RevealText(title);
        title.SetActive(true);
        upgradeIcon.SetActive(true);
        yield return new WaitForSeconds(4.0f);

        coroutineAllowed = false;
        Vector2 p0 = routes[0].position;
        Vector2 p1 = routes[1].position;
        Vector2 p2 = routes[2].position;
        Vector2 p3 = routes[3].position;

        Vector3 cameraPos, cameraOldPos;

        cameraOldPos = basecarCamera.transform.position;
        // while (tParam < 1)
        // {
        //     cameraPos = basecarCamera.transform.position;
        //     if (tParam > 0.9 && !upgradeIcon.activeSelf)
        //         upgradeIcon.SetActive(true);
        //     tParam += Time.deltaTime * speedModifier;
        //
        //     p0 = routes[0].position;
        //     p1 = routes[1].position;
        //     p2 = routes[2].position;
        //     p3 = routes[3].position;
        //     objectPosition = Mathf.Pow(1 - tParam, 3) * p0 + 3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 + 3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 + Mathf.Pow(tParam, 3) * p3;
        //
        //     title.transform.position = new Vector3(objectPosition.x, objectPosition.y, 0.0f);// - cameraPos + cameraOldPos;
        //     cameraOldPos = cameraPos;
        //     float scale = Mathf.Lerp(1.0f, 0.1f, tParam);
        //     title.transform.localScale = new Vector3(scale, scale, scale);
        //     yield return null;// new WaitForEndOfFrame();
        // }

        tParam = 0f;
        // upgradeIcon.SetActive(true);
        coroutineAllowed = true;
        title.SetActive(false);
        title.transform.position = title_pointer.transform.position;
        title.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        title.GetComponent<TMPro.TextMeshProUGUI>().text = "Level up!";
    }

    public void OptionSelect(int selectedOptionButton)
    {
        if (!canSelect)
        {
            return;
        }

        levelUpNoteAnimator.SetTrigger("LevelUpEnd");
        canSelect = false;
        Upgrade_or_Activate(selectedOptionButton);

        //optionsBanner.SetActive(false);
        skillsChooseCanvas.SetActive(false);
        levelUpLock = false;

        pendingLevelUps--;
        if (pendingLevelUps <= 0)
        {
            upgradeIcon.SetActive(false);
            title.SetActive(false);
        }
        else
        {
            upgradeIcon.SetActive(true);
        }
        levelUpAnimationAllowed = true;
    }

    private void Upgrade_or_Activate(int selectedOptionButton)
    {
        if (selectedOptionButton == 1)
        {
            processRealOption(randomOption1);
        }
        else if (selectedOptionButton == 2)
        {
            processRealOption(randomOption2);
        }
        else if (selectedOptionButton == 3)
        {
            processRealOption(randomOption3);
        }
    }

    private void processRealOption(int realOption)
    {
        // option: 1 -> sprint, 2 -> mine, 3 -> shield, 4 -> spit
        if (realOption == 1)
        {
            if (!_snailSprintManager.CanSprint())
            {
                _snailSprintManager.EnableSprint();
                sprintIcon.SetActive(true);
                set_icon_position(realOption);
            }
            else
            {
                _snailSprintManager.SprintLevelUp(0.5f);
            }
        }
        else if (realOption == 2)
        {
            if (!mineIcon.activeSelf)
            {
                mineIcon.SetActive(true);
                set_icon_position(realOption);
            }

            int mineAddition = 0;
            if (currentLevel < 8)
            {
                mineAddition = Mathf.Min(3, currentLevel);
            }
            else if (currentLevel < 15)
            {
                mineAddition = 4;
            }
            else
            {
                mineAddition = 5;
            }
            _snailWeapon.AddMine(mineAddition);
        }
        else if (realOption == 3)
        {
            if (!shieldIcon.activeSelf)
            {
                shieldIcon.SetActive(true);
                _snailShieldManager.EnableShield();
                set_icon_position(realOption);
                shield.GetComponent<ShieldBehavior>().AddShield();
            }
            else
            {
                shield.GetComponent<ShieldBehavior>().AddShield();
            }
        }
        else if (realOption == 4)
        {
            if (!spitIcon.activeSelf)
            {
                spitIcon.SetActive(true);
                _snailSpitManager.EnableSpit();
                set_icon_position(realOption);
            }
            else
            {
                _snailSpitManager.setDamage(math.min(3, _snailSpitManager.getDamage() + 1));
            }
        }
    }

    // option: 1 -> sprint, 2 -> mine, 3 -> shield, 4 -> spit
    private void set_icon_position(int option)
    {
        GameObject aim = null;
        List<Vector2> pos = new List<Vector2>();

        pos.Add(skillCanvas.transform.GetChild(0).position);
        pos.Add(skillCanvas.transform.GetChild(1).position);
        pos.Add(skillCanvas.transform.GetChild(2).position);
        pos.Add(skillCanvas.transform.GetChild(3).position);

        if (option == 1)
        {
            aim = skillCanvas.transform.Find("Sprint").gameObject;
        }
        else if (option == 2)
        {
            aim = skillCanvas.transform.Find("Mine").gameObject;
        }
        else if (option == 3)
        {
            aim = skillCanvas.transform.Find("Shield").gameObject;
        }
        else if (option == 4)
        {
            aim = skillCanvas.transform.Find("Spit").gameObject;
        }

        aim.transform.SetSiblingIndex(skillCounter);

        for (int i = 0; i < skillCanvas.transform.childCount; i++)
        {
            skillCanvas.transform.GetChild(i).position = pos[i];
        }

        skillCounter++;
    }


    private void generate_random_skill_choose()
    {
        int maxRand = 5;
        if (_snailSpitManager.getDamage() == 3)
        {
            maxRand = 4;
        }
        randomOption1 = UnityEngine.Random.Range(1, maxRand);
        while (true)
        {
            randomOption2 = UnityEngine.Random.Range(1, maxRand);
            if (randomOption2 != randomOption1)
                break;
        }

        while (true)
        {
            randomOption3 = UnityEngine.Random.Range(1, maxRand);
            if (randomOption3 != randomOption1 && randomOption2 != randomOption3)
                break;
        }

        set_option_canvas(option1Object, randomOption1);
        set_option_canvas(option2Object, randomOption2);
        set_option_canvas(option3Object, randomOption3);
    }

    private void set_option_canvas(GameObject optionObject, int option)
    {
        string acquireOption = "Acquire!";
        string upgradeOption = "Upgrade!";
        // option: 1 -> sprint, 2 -> mine, 3 -> shield, 4 -> spit
        if (option == 1)
        {
            setImage(optionObject, sprintSprite);
            setName(optionObject, "Sprint");
            if (!sprintIcon.activeSelf)
                setFunction(optionObject, acquireOption);
            else
                setFunction(optionObject, upgradeOption);
        }
        else if (option == 2)
        {
            setImage(optionObject, mineSprite);
            setName(optionObject, "Bomb");
            if (!mineIcon.activeSelf)
                setFunction(optionObject, acquireOption);
            else
                setFunction(optionObject, upgradeOption);
        }
        else if (option == 3)
        {
            setImage(optionObject, shieldSprite);
            setName(optionObject, "Shield");
            if (!shieldIcon.activeSelf)
                setFunction(optionObject, acquireOption);
            else
                setFunction(optionObject, upgradeOption);
        }
        else if (option == 4)
        {
            setImage(optionObject, spitSprite);
            setName(optionObject, "Spit");
            if (!spitIcon.activeSelf)
                setFunction(optionObject, acquireOption);
            else
                setFunction(optionObject, upgradeOption);
        }
    }

    private void setImage(GameObject optionObject, Sprite _image)
    {
        optionObject.transform.Find("optionIconHolder").transform.Find("optionImage").gameObject.GetComponent<Image>()
            .sprite = _image;
    }

    private void setName(GameObject optionObject, String nameStr)
    {
        optionObject.transform.Find("name").gameObject.transform.Find("Text").gameObject
            .GetComponent<TMPro.TextMeshProUGUI>().text = nameStr;
    }

    private void setKey(GameObject optionObject, String keyStr)
    {
        optionObject.transform.Find("Key").gameObject.transform.Find("Text").gameObject
            .GetComponent<TMPro.TextMeshProUGUI>().text = keyStr;
    }

    private void setFunction(GameObject optionObject, String function)
    {
        optionObject.transform.Find("function").gameObject.transform.Find("Text").gameObject
            .GetComponent<TMPro.TextMeshProUGUI>().text = function;
    }
}