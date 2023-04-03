using System.Collections;
using TMPro;
using UnityEngine;

public class VitalityTextControl : MonoBehaviour
{
    private TextMeshProUGUI number;

    private void Awake()
    {
        EventBus.Subscribe<ModifyVitalityEvent>(SetVitality);
        number = GetComponent<TextMeshProUGUI>();
    }

    private void SetVitality(ModifyVitalityEvent e)
    {
        StartCoroutine(TextChangeAnimation(e));
    }

    private IEnumerator TextChangeAnimation(ModifyVitalityEvent e)
    {
        yield return null;
        number.text = Mathf.FloorToInt((float)e.vitality / 10).ToString() + "<size=-16>%";
        number.fontSize = 90;
        yield return new WaitForSeconds(0.3f);
        number.fontSize = 80;
    }
}