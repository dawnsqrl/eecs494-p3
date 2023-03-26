using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VitalityTextControl : MonoBehaviour
{
    private TMPro.TextMeshProUGUI number;
    private void Awake()
    {
        EventBus.Subscribe<ModifyVitalityEvent>(SetVitality);
        number = GetComponent<TMPro.TextMeshProUGUI>();
    }

    private void SetVitality(ModifyVitalityEvent e)
    {
        StartCoroutine(TextChangeAnimation(e));
    }

    IEnumerator TextChangeAnimation(ModifyVitalityEvent e)
    {
        yield return null;
        number.text = Mathf.FloorToInt(e.vitality / 10).ToString() + "<size=50>%";
        number.fontSize = 80;
        yield return new WaitForSeconds(0.3f);
        number.fontSize = 60;
    }
}
