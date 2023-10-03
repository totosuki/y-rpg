using UnityEngine;

public class CameraController : MonoBehaviour
{
    private bool opening = true;

    void Update()
    {
        if (opening) ShowMap();
    }

    public void FollowObject(GameObject targetObject)
    {
        // オブジェクトを追従させる
        transform.parent = targetObject.transform;
        transform.localPosition = new Vector3(0, 0, -10);
    }

    public void StopOpening()
    {
        opening = false;
    }

    void ShowMap()
    {
        if (transform.position.y >= 3.0f)
        {
            StopOpening();
        }   

        transform.position += new Vector3(0.0f, 0.001f, 0.0f);
    }
}
