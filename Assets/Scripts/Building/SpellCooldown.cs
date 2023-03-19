using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellCooldown : MonoBehaviour
{
    [SerializeField] private Image imageCooldown;

    [SerializeField] private TMP_Text textCooldown;
    //[SerializeField] private Image imageEdge;

    //variable for looking after the cooldown
    private bool isCoolDown = false;
    [SerializeField] private float cooldownTime = 5.0f;
    private float cooldownTimer = 0.0f;

    private bool start = true;

    private void Start()
    {
        textCooldown.gameObject.SetActive(false);
        //imageEdge.gameObject.SetActive(false);
        imageCooldown.fillAmount = 0.0f;
    }

    private void Update()
    {
        if (start)
        {
            imageCooldown.gameObject.SetActive(true);
            UseSpell();
            start = false;
        }

        if (isCoolDown)
        {
            ApplyCooldown();
        }
    }

    void ApplyCooldown()
    {
        cooldownTimer -= SimulationSpeedControl.GetSimulationSpeed() * Time.deltaTime;
        if (cooldownTimer < 0.0f)
        {
            isCoolDown = false;
            textCooldown.gameObject.SetActive(false);
            //imageEdge.gameObject.SetActive(false);
            imageCooldown.fillAmount = 0.0f;
            imageCooldown.gameObject.SetActive(false);
        }
        else
        {
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
}