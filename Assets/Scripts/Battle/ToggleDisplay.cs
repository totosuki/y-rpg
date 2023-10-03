using UnityEngine;

public class ToggleDisplay : MonoBehaviour {
    public float waitDuration;
    public bool display;

    public void InvokeToggle() {
        gameObject.SetActive(!display);
        Invoke("Toggle", waitDuration);
    }

    private void Toggle() {
        gameObject.SetActive(display);
    }
}
