using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Fader : MonoBehaviour
{
    public float waitDuration;
    public float speed;

    private bool startFade;

    public Color startColor;
    public Color endColor;

    [Range(0f, 1f)]
    public float t;
    public AnimationCurve curve;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (startFade) {
            t += speed * 0.005f;

            if (t >= 1f)
            {
                startFade = false;
            }

            ChangeColorOfGameObject(gameObject, Color.Lerp(startColor,endColor,curve.Evaluate(t)));
        }
    }

    public void InvokeFade()
    {
        Invoke("StartFade", waitDuration);
    }

    private void StartFade() {
        startFade = true;
    }

    private void ChangeColorOfGameObject(GameObject targetObject, Color color)
    {
    //入力されたオブジェクトのRendererを全て取得し、さらにそのRendererに設定されている全Materialの色を変える
        targetObject.GetComponent<Image>().color = color;
    }
}
