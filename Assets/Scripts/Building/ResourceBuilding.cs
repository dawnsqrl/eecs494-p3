using UnityEngine;

public class ResourceBuilding : MonoBehaviour
{
    private VitalityController vitality;

    private void Start()
    {
        vitality = GameObject.Find("VitalityController").GetComponent<VitalityController>();
        vitality.decreaseVitality(200);
        vitality.increaseVitalityGrowth(15);
        AudioClip clip = Resources.Load<AudioClip>("Audio/VitalityBuilding");
        AudioSource.PlayClipAtPoint(clip, transform.position, 10);
    }

    private void OnDestroy()
    {
        vitality.decreaseVitalityGrowth(15);
        if (GameObject.Find("BuildingCanvas") != null)
            GameObject.Find("BuildingCanvas").GetComponent<BuildingController>().unregister_building(gameObject);
        
        if (!DestoryBuildingDrag.selfDestory)
        {
            EventBus.Publish(new AddExpEvent(5));
        }
        else
        {
            DestoryBuildingDrag.selfDestory = false;
        }
    }
}