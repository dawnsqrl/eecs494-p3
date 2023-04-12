using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizenBuildingControl : MonoBehaviour
{
    private bool is_building_citizen = false;
    GameObject building;

    public void change_status(GameObject _building)
    {
        building = _building;
        is_building_citizen = true;
    }

    public bool get_status()
    {
        return is_building_citizen;
    }

    public void remove_from_list()
    {
        building.GetComponent<SoliderBuilding>().removeCitizen(gameObject);
    }

    private void OnDestroy()
    {
        remove_from_list();
    }
}
