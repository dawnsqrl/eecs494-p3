using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SnailSkillCooldown : MonoBehaviour
{
    [SerializeField] private Image imageCooldown;

    [SerializeField] private TMP_Text textCooldown;

    [SerializeField] private int buildingType;

    [SerializeField] private bool canUseSkill;
    //[SerializeField] private Image imageEdge;

    //variable for looking after the cooldown
    private bool isCoolDown = false;
    [SerializeField] private float cooldownTime = 5;
    private float cooldownTimer = 0;

    private float temp_cool_down_time;

    private bool start = true;
    private int vitality;

    int buildingNum, maxBuildingNum;

    private void Start()
    {
        textCooldown.gameObject.SetActive(false);
        //imageEdge.gameObject.SetActive(false);
        imageCooldown.fillAmount = 0;

        temp_cool_down_time = cooldownTime;
        canUseSkill = false;
    }

    private void Update()
    {
        
        if (GameProgressControl.isGameActive)
        {
            if ((buildingNum == maxBuildingNum && buildingType != 0) || (buildingType == 0 && vitality < 400) || (buildingType == 1 && vitality < 100) || (buildingType == 2 && vitality < 200) || (buildingType == 3 && vitality < 300))
            {
                if (!isCoolDown)
                {
                    textCooldown.gameObject.SetActive(false);
                    imageCooldown.gameObject.SetActive(true);
                    imageCooldown.fillAmount = 1.0f;
                }
            }
            else
            {
                if (!isCoolDown)
                {
                    imageCooldown.gameObject.SetActive(false);
                    imageCooldown.fillAmount = 0.0f;
                }
            }
            
            if (start)
            {
                canUseSkill = false;
                temp_cool_down_time = 1;
                cooldownTime = temp_cool_down_time;
                imageCooldown.gameObject.SetActive(true);
                UseSpell();
                start = false;
                canUseSkill = true;
            }

            if (isCoolDown)
            {
                ApplyCooldown();
            }
        }
    }

    void ApplyCooldown()
    {
        //if ((buildingType == 1 && vitality < 100) || (buildingType == 2 && vitality < 200) || (buildingType == 3 && vitality < 300))
        //    return;
        cooldownTimer -= SimulationSpeedControl.GetSimulationSpeed() * Time.deltaTime;
        if (cooldownTimer < 0)
        {
            if ((buildingType == 0 && vitality < 400) || (buildingType == 1 && vitality < 100) || (buildingType == 2 && vitality < 200) || (buildingType == 3 && vitality < 300))
            {
                textCooldown.gameObject.SetActive(false);
                imageCooldown.fillAmount = 1.0f;
                return;
            }
                

            isCoolDown = false;
            textCooldown.gameObject.SetActive(false);
            //imageEdge.gameObject.SetActive(false);
            imageCooldown.fillAmount = 0;
            imageCooldown.gameObject.SetActive(false);
        }
        else
        {
            textCooldown.gameObject.SetActive(true);
            textCooldown.text = Mathf.RoundToInt(cooldownTimer).ToString();
            imageCooldown.fillAmount = cooldownTimer / cooldownTime;

            //imageEdge.transform.localEulerAngles = new Vector3(0, 0, 360 * (cooldownTimer / cooldownTime));
        }
    }

    public bool UseSpell()
    {
        if (isCoolDown)
        {
            return false;
        }
        else
        {
            isCoolDown = true;
            textCooldown.gameObject.SetActive(true);
            cooldownTimer = cooldownTime;
            textCooldown.text = Mathf.RoundToInt(cooldownTimer).ToString();
            imageCooldown.fillAmount = 1;

            //imageEdge.gameObject.SetActive(true);
            return true;
        }
    }

    public void reStart()
    {
        start = true;
        isCoolDown = false;
    }

    public void setCoolDownTime(float new_time)
    {
        temp_cool_down_time = new_time;
    }
}