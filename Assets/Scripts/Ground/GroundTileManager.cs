using System.Collections;
using UnityEngine;

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
            small_hyphae.gameObject.SetActive(true);
            Color tmp = small_hyphae.gameObject.GetComponent<SpriteRenderer>().color;
            tmp.a = 1;
            small_hyphae.gameObject.GetComponent<SpriteRenderer>().color = tmp;
            enableAnimator(small_hyphae.gameObject);
            small_hyphae.gameObject.GetComponent<Animator>().SetTrigger("appear");
            StartCoroutine(disableAnimator(small_hyphae.gameObject));
        }

        if (!growthed)
        {
            GameState.myceliumProduced++;
        }

        growthed = true;
        StartCoroutine(disableSmallHyphaeAndEnableWholeSprite());
    }

    IEnumerator disableAnimator(GameObject hyphae)
    {
        yield return new WaitForSeconds(1.0f);
        hyphae.GetComponent<Animator>().enabled = false;
    }

    IEnumerator disableSmallHyphaeAndEnableWholeSprite()
    {
        yield return new WaitForSeconds(1.0f);
        GameObject hyphae = transform.parent.gameObject.transform.Find("Hyphae").gameObject;
        hyphae.SetActive(false);
        GameObject hyphae_whole = transform.parent.gameObject.transform.Find("Hyphae_whole").gameObject;
        hyphae_whole.SetActive(true);
        Color tmp = hyphae_whole.gameObject.GetComponent<SpriteRenderer>().color;
        tmp.a = 1;
        hyphae_whole.gameObject.GetComponent<SpriteRenderer>().color = tmp;
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

        if (!mucused)
        {
            GameState.mucusProduced++;
        }

        mucused = true;
    }

    public void RemoveMucus()
    {
        GameObject mucus = transform.parent.gameObject.transform.Find("Mucus").gameObject;
        if (mucused)
        {
            GameState.mucusDestroyed++;
        }

        mucused = false;
        mucus.SetActive(false);
    }

    public void RemoveGrowthed()
    {
        GameObject hyphae = transform.parent.gameObject.transform.Find("Hyphae_whole").gameObject;
        if (growthed)
        {
            GameState.myceliumDestroyed++;
        }

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