using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialArrowManager : MonoBehaviour
{
    [SerializeField] private GameObject next;
    private void Start()
        {
            if (next)
            {
                next.SetActive(false);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (next)
            {
                next.SetActive(true);
            }
            gameObject.SetActive(false);
        }
}
