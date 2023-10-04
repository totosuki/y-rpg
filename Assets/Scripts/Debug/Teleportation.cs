using UnityEngine;

public class Teleportation : MonoBehaviour {

    [SerializeField]
    private GameObject player;

    private Transform playerTransform;

    private bool isCommand = false;

    // Vector2
    private Vector2 firstFloor = new Vector2(2, -17); 
    private Vector2 secondFloor = new Vector2(-19, 38);

    void Start() {
        playerTransform = player.transform;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.T)) {
            isCommand = true;
        }

        if (isCommand == true) {
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                playerTransform.position = firstFloor;
                isCommand = false;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2)) {
                playerTransform.position = secondFloor;
                isCommand = false;
            }
        }
    }
}
