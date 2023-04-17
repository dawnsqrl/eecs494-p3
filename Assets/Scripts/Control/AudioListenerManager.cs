using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioListenerManager : MonoBehaviour
{
    public static Vector3 audioListenerPos;
    // Start is called before the first frame update
    void Start()
    {
        audioListenerPos = transform.position;
    }
    
}
