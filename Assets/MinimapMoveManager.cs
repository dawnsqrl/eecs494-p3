using System.Collections;
using System.Collections.Generic;
using CodeMonkey.Utils;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class MinimapMoveManager : MonoBehaviour
{
    private Camera _camera;
    private GameObject _main_camera;
    private Coroutine camMovePos;
    
    // Start is called before the first frame update
    void Start()
    {
        _camera = GetComponent<Camera>();
        _main_camera = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && _camera.pixelRect.Contains(Input.mousePosition))
        {
            Vector3 correctedPos = _camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 targetPos = new Vector3(correctedPos.x, correctedPos.y, correctedPos.z);
            if (camMovePos != null)
            {
                StopCoroutine(camMovePos);
            }
            camMovePos = StartCoroutine(MoveCam(targetPos));
            // RaycastHit hit;
            // Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            // Debug.Log(UtilsClass.GetMouseWorldPosition());
            // // Gizmos.DrawRay(ray.origin, ray.direction);
            // if (Physics.Raycast(ray, out hit, LayerMask.GetMask("CameraRayCast")))
            // {
            //     Debug.Log(hit.point);
            //     Debug.Log(ray.origin);
            //     // _main_camera.transform.position = new Vector3(hit.point.x, hit.point.y, _main_camera.transform.position.z);
            // }
        }
    }

    private IEnumerator MoveCam(Vector3 targetPos)
    {
        while ((targetPos - _main_camera.transform.position).magnitude > 2f)
        {
            Vector3 dir = (targetPos - _main_camera.transform.position).normalized;
            _main_camera.transform.position += 50f * Time.deltaTime * dir;
            yield return null;
        }
        _main_camera.transform.position = targetPos;
    }
}
