using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialArrowManager : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BaseCar"))
        {
            EventBus.Publish(new DisplayHintEvent(
                "Consuming a block of hyphae grants you exp points. You will gain new skills when exp bar is filled. [Enter]"
            ));
        }
    }
}
