using System.Collections;
using UnityEngine;

public class MinimapMoveManager : MonoBehaviour
{
    private Camera _camera;
    [SerializeField] private GameObject _main_camera;
    private Coroutine camMovePos;
    float x;

    // Start is called before the first frame update
    void Start()
    {
        _camera = GetComponent<Camera>();
        x = ((float)Screen.currentResolution.width) / 1920.0f * 450.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //Rect r = _camera.pixelRect * 10;
        if (Input.GetMouseButtonDown(0) && new Rect(_camera.pixelRect.x, _camera.pixelRect.y, x, x).Contains(Input.mousePosition))
        {
            //print(x);
            //print(Input.mousePosition);
            //print(_camera.pixelRect);
            //print(Screen.currentResolution);
            //Vector3 correctedPos = _camera.ScreenToWorldPoint(Input.mousePosition);
            //Vector3 temp = Camera.main.WorldToScreenPoint(new Vector3(1.0f, 1.0f, 0.0f)); // 1 -> 0.1
            Vector3 targetPos = new Vector3(Input.mousePosition.x / x * 50, Input.mousePosition.y / x * 50, -10.0f);
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