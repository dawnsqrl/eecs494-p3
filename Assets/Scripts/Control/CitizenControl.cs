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
        var citizen =
            PrefabUtility.InstantiatePrefab(
                Resources.Load<GameObject>("Prefabs/Objects/Citizen")
            ) as GameObject;
        citizen.transform.position = UtilsClass.GetMouseWorldPosition();
        citizen.transform.rotation = Quaternion.identity;
        citizen.GetComponent<UnitRTS>().MoveTo(citizen.transform.position);
    }
}