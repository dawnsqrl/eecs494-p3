using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = System.Random;
[SelectionBase]
public class GroundTileManager : MonoBehaviour
{
    [SerializeField] public bool growthed = false;
    [SerializeField] public bool mucused = false;

    private bool blank_tile = false;
    private int blank_rate = 2;

    private GameObject fog_builder;
    private GameObject fog_enemy;

    //[SerializeField] private Animator _animator;
    //private int blank_regrowth_rate = 10;
    // Start is called before the first frame update
    void Start()
    {
        set_blank();
        // fog_builder = transform.parent.gameObject.transform.Find("Tile_fog_builder").gameObject;
        // fog_enemy = transform.parent.gameObject.transform.Find("Tile_fog_enemy").gameObject;
    }
    
    private void set_blank()
    {
        if (UnityEngine.Random.Range(0, 101) < blank_rate)
        {
            blank_tile = true;
        }
    }

    public void SetFogVisible(bool visible, string fog_type)
    {
        if (!blank_tile)
        {
            GameObject fog = fog_type == "builder" ? fog_builder : fog_enemy;
            if (fog)
            {
                fog.SetActive(visible);
            }
        }     
    }
    
    public void SetFogVisible_LongTerm(bool visible)
    {
        if (!blank_tile)
        {
            if (fog_builder)
            {
                fog_builder.SetActive(visible);
                transform.parent.GetComponent<TileManager>().builderFogLongTermDisabled = true;
            }
        }     
    }

    public void SetGrowthed()
    {
        GameObject hyphae = transform.parent.gameObject.transform.Find("Hyphae").gameObject;
        hyphae.SetActive(true);
        // hyphae.transform.Rotate (Vector3.forward * UnityEngine.Random.Range(-10, 10));
        foreach (Transform small_hyphae in hyphae.transform)
        {
            enableAnimator(small_hyphae.gameObject);
            small_hyphae.gameObject.GetComponent<Animator>().SetTrigger("appear");
            StartCoroutine(disableAnimator(small_hyphae.gameObject));
        }
        growthed = true;
    }

    IEnumerator disableAnimator(GameObject hyphae)
    {
        yield return new WaitForSeconds(1.0f);
        hyphae.GetComponent<Animator>().enabled = false;
    }

    void enableAnimator(GameObject hyphae)
    {
        hyphae.GetComponent<Animator>().enabled = true;
    }

    public void SetMucus()
    {
        GameObject mucus = transform.parent.gameObject.transform.Find("Mucus").gameObject;
        mucus.SetActive(true);
        // mucus.transform.Rotate (Vector3.forward * UnityEngine.Random.Range(-10, 10));
        foreach (Transform small_hyphae in mucus.transform)
        {
            enableAnimator(small_hyphae.gameObject);
            small_hyphae.gameObject.GetComponent<Animator>().SetTrigger("appear");
            disableAnimator(small_hyphae.gameObject);
        }
        mucused = true;
    }
    
    public void RemoveMucus()
    {
        GameObject mucus = transform.parent.gameObject.transform.Find("Mucus").gameObject;
        mucused = false;
        mucus.SetActive(false);
        
    }
    
    public void RemoveGrowthed()
    {
        GameObject hyphae = transform.parent.gameObject.transform.Find("Hyphae").gameObject;
        growthed = false;
        hyphae.SetActive(false);
    }

    public bool CheckGrowthed()
    {
        return growthed;
    }

    public bool CheckMucused()
    {
        return mucused;
    }

    public bool BuilderFogLongTermDisabled()
    {
        return transform.parent.GetComponent<TileManager>().builderFogLongTermDisabled;
    }

}
