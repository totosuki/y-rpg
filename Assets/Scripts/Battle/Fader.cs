using UnityEngine.UI;
using UnityEngine;

public class Fader : MonoBehaviour {
    public float waitDuration;
    public float speed;

    private bool startFade;

    public Color startColor;
    public Color endColor;

    [Range(0f, 1f)]
    public float t;
    public AnimationCurve curve;

    private Image image;

    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (startFade) {
            t += speed * 0.005f;
            if (t >= 1f) {
                startFade = false;
            }
            ChangeColorOfGameObject(Color.Lerp(startColor,endColor,curve.Evaluate(t)));
        }
    }

    public void InvokeFade(float duration) {
        Invoke("StartFade", duration);
    }

    private void StartFade() {
        startFade = true;
    }

    private void ChangeColorOfGameObject(Color color) {
        image.color = color;
    }

    public void Init()
    {
        ChangeColorOfGameObject(startColor);
        startFade = false;
        t = 0f;
    }
}
