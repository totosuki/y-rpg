using UnityEngine;
using TMPro;

public class XPBar : MonoBehaviour {
    public int level;
    public int currentXp;
    public int maxXp;

    private SliderManager sliderManager;
    private TMP_Text lvText;

    void Start() {
        sliderManager = GetComponentInChildren<SliderManager>();
        lvText = GetComponentInChildren<TMP_Text>();

        sliderManager.SetMaxValue(10);
    }

    // 10.5のように少数で指定してXPバーを調整できる
    public void UpdateLv(float lv)
    {
        int lv_int = Mathf.FloorToInt(lv);
        float lv_r = lv - lv_int;

        lvText.text = $"Lv.{lv_int}";
        sliderManager.SetValue(Mathf.FloorToInt(lv_r * 10));

    }
}
