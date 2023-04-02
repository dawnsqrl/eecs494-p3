using UnityEngine;

public class ResourceBuilding : MonoBehaviour
{
    private VitalityController vitality;

    private void Start()
    {
        vitality = GameObject.Find("VitalityController").GetComponent<VitalityController>();
        vitality.decreaseVitality(100);
        vitality.increaseVitalityGrowth(20);
        AudioClip clip = Resources.Load<AudioClip>("Audio/VitalityBuilding");
        AudioSource.PlayClipAtPoint(clip, transform.position);
    }

    private void OnDestroy()
    {
        GameObject.Find("BuildingCanvas").GetComponent<BuildingController>().unregister_building(gameObject);
        AudioClip clip = Resources.Load<AudioClip>("Audio/BuildingDown");
        AudioSource.PlayClipAtPoint(clip, transform.position);
    }
}