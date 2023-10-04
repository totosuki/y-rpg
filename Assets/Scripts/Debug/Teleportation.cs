using UnityEngine;

public class Teleportation : MonoBehaviour {

    [SerializeField]
    private GameObject player;

    void Update() {
        if (Input.GetKey(KeyCode.T)) {
            player.transform.position = new Vector2(-17, 21);
        }
    }
}
