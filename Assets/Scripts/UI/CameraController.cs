using UnityEngine;

public class CameraController : MonoBehaviour {
    private bool opening;

    void Update() {
        if (opening) ScrollMap();
    }

    public void FollowObject(GameObject targetObject)
    {
        // オブジェクトを追従させる
        transform.parent = targetObject.transform;
        transform.localPosition = new Vector3(0, 0, -10);
    }

    void ScrollMap()
    {
        if (transform.position.y >= 3.0f) {
            StopScroll();
        }

        transform.position += new Vector3(0.0f, 0.001f, 0.0f);
    }

    public void StartScroll()
    {
        opening = true;
    }

    public void StopScroll()
    {
        opening = false;
    }
}
