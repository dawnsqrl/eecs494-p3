using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellCooldown : MonoBehaviour
{
    [SerializeField] private Image imageCooldown;

    [SerializeField] private TMP_Text textCooldown;
    [SerializeField] private int buildingType; //1 -> resource, 2 -> citizen, 3 -> defence
    //[SerializeField] private Image imageEdge;

    //variable for looking after the cooldown
    private bool isCoolDown = false;
    [SerializeField] private float cooldownTime = 5.0f;
    private float cooldownTimer = 0.0f;

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
        imageCooldown.fillAmount = 0.0f;

        temp_cool_down_time = cooldownTime;
        
    }

    private void Update()
    {
        if(GameProgressControl.isGameActive)
        {
            if (start)
            {
                if (buildingType == 1)
                    temp_cool_down_time = 4.0f / 500.0f * (float)vitality + 18.0f / 5.0f; // 300 -> 6, 800 -> 10
                else if (buildingType == 2)
                    temp_cool_down_time = -15.0f / 900.0f * (float)vitality + 65.0f / 3.0f; // 100 -> 20, 1000 -> 5
                else
                    temp_cool_down_time = 99.0f;

                cooldownTime = temp_cool_down_time;
                imageCooldown.gameObject.SetActive(true);
                UseSpell();
                start = false;
            }

            if (isCoolDown)
            {
                ApplyCooldown();
            }
        }
        
    }

    void ApplyCooldown()
    {
        if ((buildingType == 1 && vitality < 100) || (buildingType == 2 && vitality < 200))
            return;
        cooldownTimer -= SimulationSpeedControl.GetSimulationSpeed() * Time.deltaTime;
        if (cooldownTimer < 0.0f)
        {
            if ((buildingType == 1 && vitality < 100) || (buildingType == 2 && vitality < 200))
                return;

            isCoolDown = false;
            textCooldown.gameObject.SetActive(false);
            //imageEdge.gameObject.SetActive(false);
            imageCooldown.fillAmount = 0.0f;
            imageCooldown.gameObject.SetActive(false);
        }
        else
        {
            if ((buildingType == 1 && vitality < 100) || (buildingType == 2 && vitality < 200))
                return;
            textCooldown.text = Mathf.RoundToInt(cooldownTimer).ToString();
            imageCooldown.fillAmount = cooldownTimer / cooldownTime;

            //imageEdge.transform.localEulerAngles = new Vector3(0, 0, 360.0f * (cooldownTimer / cooldownTime));
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
            imageCooldown.fillAmount = 1.0f;

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