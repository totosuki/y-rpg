using UnityEngine;

public class ToggleDisplay : MonoBehaviour {
    public float waitDuration;
    public bool display;

    public void InvokeToggle(float duration) {
        gameObject.SetActive(!display);
        Invoke("Toggle", duration);
    }

    private void Toggle() {
        gameObject.SetActive(display);
    }

    public void Init()
    {
        gameObject.SetActive(false);
    }
}
