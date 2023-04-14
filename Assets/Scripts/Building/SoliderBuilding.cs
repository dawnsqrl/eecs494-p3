using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoliderBuilding : MonoBehaviour
{
    private int maxSolider = 2;
    private List<GameObject> autoSoliders;
    private float last_res = 0.0f;

    private VitalityController vitalityController;

    bool isBuilderTutorialActive = false;

    private void Awake()
    {
        EventBus.Subscribe<StartBuilderTutorialEvent>(_ => isBuilderTutorialActive = true);
    }

    private void Start()
    {
        autoSoliders = new List<GameObject>();
        StartCoroutine(GenerateSolider());

        vitalityController = GameObject.Find("VitalityController").GetComponent<VitalityController>();

        vitalityController.decreaseVitality(200);
        vitalityController.decreaseVitalityGrowth(5);
        AudioClip clip = Resources.Load<AudioClip>("Audio/SoilderBuilding");
        AudioSource.PlayClipAtPoint(clip, transform.position);
    }

    IEnumerator GenerateSolider()
    {
        while (true)
        {
            if (autoSoliders.Count < maxSolider)
            {
                GameObject solider;
                yield return new WaitForSeconds(3.0f);
                if (!isBuilderTutorialActive)
                    solider = Instantiate(Resources.Load<GameObject>("Prefabs/Objects/Citizen"),
                        transform.position, Quaternion.identity);
                else
                    solider = Instantiate(Resources.Load<GameObject>("Prefabs/BuilderTutorial/TCitizen"),
                        transform.position, Quaternion.identity);
                GameState.smallMushroomProduced++;
                Vector2 newPos = transform.position + generateRandomVector();
                print(newPos);
                solider.GetComponent<UnitRTS>().MoveTo(newPos);
                solider.GetComponent<CitizenBuildingControl>().change_status(gameObject);
                autoSoliders.Add(solider);
                CitizenControl.citizenList.Add(solider);
            }

            yield return new WaitForSeconds(1.0f);
        }
    }

    private void OnDestroy()
    {
        vitalityController.increaseVitalityGrowth(5);
        GameObject.Find("BuildingCanvas").GetComponent<BuildingController>().unregister_building(gameObject);
        AudioClip clip = Resources.Load<AudioClip>("Audio/BuildingDown");
        AudioSource.PlayClipAtPoint(clip, transform.position);
        if (!DestoryBuildingDrag.selfDestory)
        {
            EventBus.Publish(new AddExpEvent(5));
        }
        else
        {
            DestoryBuildingDrag.selfDestory = false;
        }
    }

    private Vector3 generateRandomVector()
    {
        float new_res1, new_res2;
        while (true)
        {
            new_res1 = Random.Range(-2.0f, 2.0f);
            if (new_res1 != last_res && (new_res1 >= 0.6f || new_res1 <= -0.6f))
            {
                last_res = new_res1;
                break;
            }
        }

        while (true)
        {
            new_res2 = Random.Range(-2.0f, 2.0f);
            if (new_res2 != last_res && (new_res2 >= 0.6f || new_res2 <= -0.6f))
            {
                last_res = new_res2;
                break;
            }
        }

        return new Vector3(new_res1, new_res2, 0.0f);
    }

    public void removeCitizen(GameObject citizen)
    {
        autoSoliders.Remove(citizen);
    }
}