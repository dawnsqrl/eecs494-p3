using CodeMonkey.Utils;
using UnityEditor;
using UnityEngine;

public class CitizenControl : MonoBehaviour
{
    private void Awake()
    {
        EventBus.Subscribe<SpawnCitizenEvent>(_SpawnCitizen);
    }

    private void _SpawnCitizen(SpawnCitizenEvent e)
    {
        // var citizen = Resources.Load<GameObject>("Prefabs/Objects/Citizen");
        GameObject citizen = Instantiate(Resources.Load<GameObject>("Prefabs/Objects/Citizen"), UtilsClass.GetMouseWorldPosition(), Quaternion.identity);
        citizen.GetComponent<UnitRTS>().MoveTo(citizen.transform.position);
    }
}