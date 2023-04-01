using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellCooldown : MonoBehaviour
{
    [SerializeField] private Image imageCooldown;

    [SerializeField] private TMP_Text textCooldown;

    [SerializeField] private int buildingType;
    [SerializeField] private ViewDragging vd;
    //[SerializeField] private Image imageEdge;

    //variable for looking after the cooldown
    private bool isCoolDown = false;
    [SerializeField] private float cooldownTime = 5;
    private float cooldownTimer = 0;

    private float temp_cool_down_time;

    private bool start = true;
    private int vitality;

    private void Awake()
    {
        EventBus.Subscribe<ModifyVitalityEvent>(e => vitality = e.vitality);
    }

    private void Start()
    {
        textCooldown.gameObject.SetActive(false);
        //imageEdge.gameObject.SetActive(false);
        imageCooldown.fillAmount = 0;

        temp_cool_down_time = cooldownTime;
    }

    private void Update()
    {
        if (GameProgressControl.isGameActive)
        {
            if (start)
            {
                vd.enabled = false;
                switch (buildingType)
                {
                    case 0: // spread; 200 -> 25, 1000 -> 15
                        temp_cool_down_time = -10f / 800 * vitality + 55f / 2;
                        break;
                    case 1: // resource; 300 -> 6, 800 -> 10
                        temp_cool_down_time = 4f / 500 * vitality + 18f / 5;
                        break;
                    case 2: // citizen; 100 -> 20, 1000 -> 5
                        temp_cool_down_time = -15f / 900 * vitality + 65f / 3;
                        break;
                    case 3: // citizen; 100 -> 20, 1000 -> 5
                        temp_cool_down_time = -15f / 900 * vitality + 65f / 3;
                        break;
                    default:
                        temp_cool_down_time = 1;
                        break;
                }

                cooldownTime = temp_cool_down_time;
                imageCooldown.gameObject.SetActive(true);
                UseSpell();
                start = false;
                vd.enabled = true;
            }

            if (isCoolDown)
            {
                //vd.enabled = false;
                ApplyCooldown();
                //vd.enabled = true;
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
            if ((buildingType == 1 && vitality < 100) || (buildingType == 2 && vitality < 200) || (buildingType == 3 && vitality < 300))
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