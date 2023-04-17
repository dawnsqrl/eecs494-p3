using System.Collections;
using UnityEngine;

public class MinimapMoveManager : MonoBehaviour
{
    private Camera _camera;
    [SerializeField] private GameObject _main_camera;
    private Coroutine camMovePos;

    // Start is called before the first frame update
    void Start()
    {
        _camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        //Rect r = _camera.pixelRect * 10;
        if (Input.GetMouseButtonDown(0) && _camera.pixelRect.Contains(Input.mousePosition))
        {
            print(_camera.pixelRect);
            print(Screen.currentResolution);
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