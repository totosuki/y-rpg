using Fungus;
using UnityEngine;
using TMPro;

public class AreaInfo : MonoBehaviour {

    [SerializeField]private TMP_Text areaName;
    [SerializeField]private TMP_Text subtitle;

    [Header("Fader設定")]
    public float speed;

    [Range(0f, 1f)]
    public float t;
    public AnimationCurve curve;

    private CanvasGroup canvasGroup;
    private bool doFade;
    private float target = 1f;

    void Start()
    {
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
    }

    void FixedUpdate()
    {
        if (doFade)
        {
            float append = speed * 0.005f;
            if (target == 1f)
            {
                t += append;

                if (t >= 1f)
                {
                    target = 0f;
                }
            }
            else
            {
                t -= append;

                if (t <= 0f)
                {
                    doFade = false;
                    t = 0f;
                    target = 1f;
                }
            }

            UpdateAlpha(curve.Evaluate(t));
        }
    }

    public void Show()
    {
        // すでに実行中の場合はリセット
        if (doFade)
        {
            target = 1f;
            t = 0f;
        }
        else
        {
            doFade = true;
        }
    }

    private void UpdateAlpha(float newAlpha)
    {
        canvasGroup.alpha = newAlpha;
    }

    public void SetInfo(string _areaName, string _subtitle)
    {
        areaName.text = _areaName;
        subtitle.text = _subtitle;
    }
}
