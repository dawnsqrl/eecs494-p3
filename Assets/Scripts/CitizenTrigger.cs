using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CitizenTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("BaseCar"))
        {
            CitizenControl.citizenList.Remove(transform.parent.gameObject);
            if(transform.parent.gameObject.GetComponent<CitizenBuildingControl>().get_status())
                transform.parent.gameObject.GetComponent<CitizenBuildingControl>().remove_from_list();
            Destroy(transform.parent.gameObject);
        }
    }
}