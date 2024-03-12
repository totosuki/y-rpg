using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField] private float cameraSize;

    private bool opening;
    private Camera _camera;

    void Start()
    {
        _camera = GetComponent<Camera>();
        SetCameraSize(cameraSize);
    }

    void FixedUpdate() {
        if (opening) ScrollMap();
    }

    public void FollowObject(GameObject targetObject)
    {
        // オブジェクトを追従させる
        transform.parent = targetObject.transform;
        transform.localPosition = new Vector3(0, 0, -10);
        SetCameraSize(cameraSize);
    }

    void ScrollMap()
    {
        if (transform.position.y >= 3.0f) {
            StopScroll();
        }

        transform.position += new Vector3(0.0f, 0.007f, 0.0f);
    }

    public void StartScroll()
    {
        opening = true;
    }

    public void StopScroll()
    {
        opening = false;
    }

    void SetCameraSize(float size)
    {
        _camera.orthographicSize = size;
    }
}
