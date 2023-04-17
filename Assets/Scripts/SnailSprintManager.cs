using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnailSprintManager : MonoBehaviour
{
    private bool canSprint;
    private float sprintSpeed;
    private float sprintTime;
    private Rigidbody _rigidbody;
    private BasecarController _basecarController;
    
    bool attacklock = false;
    private float maxCoolDownTime;
    private float remainCoolDownTime;
    [SerializeField] private Image coolDownFog;

    private void Awake()
    {
        EventBus.Subscribe<SnailSprintEvent>(_ => Sprint());
        _rigidbody = GetComponent<Rigidbody>();
        _basecarController = GetComponent<BasecarController>();
    }

    private void Start()
    {
        canSprint = false;
        sprintSpeed = 10;
        sprintTime = 0.3f;
        maxCoolDownTime = 5;
        remainCoolDownTime = 0;
        coolDownFog.fillAmount = 0;
    }
    public bool CanSprint()
    {
        return canSprint;
    }
    
    public void EnableSprint()
    {
        canSprint = true;
    }

    public void SprintLevelUp(float _time)
    {
        maxCoolDownTime = Math.Max(2, maxCoolDownTime -= _time);
    }

    public void Sprint()
    {
        if (!canSprint || attacklock || _basecarController.forwardDirection == Vector3.zero)
        {
            return;
        }
        AudioClip clip = Resources.Load<AudioClip>("Audio/Sprint");
        AudioSource.PlayClipAtPoint(clip, AudioListenerManager.audioListenerPos);
        attacklock = true;
        canSprint = false;
        StartCoroutine(SprintProcess());
        StartCoroutine(CoolDown());
    }

    private IEnumerator SprintProcess()
    {
        _basecarController.is_sprint = true;
        _basecarController.speed = sprintSpeed;
        yield return new WaitForSeconds(sprintTime);
        _basecarController.speed = _basecarController.normalSpeed;
        _basecarController.is_sprint = false;
        canSprint = true;
    }
    
    private IEnumerator CoolDown()
    {
        remainCoolDownTime = maxCoolDownTime;
        while (remainCoolDownTime > 0)
        {
            remainCoolDownTime -= Time.deltaTime;
            coolDownFog.fillAmount = remainCoolDownTime / maxCoolDownTime;
            yield return null;
        }
        
        attacklock = false;
        coolDownFog.fillAmount = 0;
        remainCoolDownTime = 0;
    }
}
