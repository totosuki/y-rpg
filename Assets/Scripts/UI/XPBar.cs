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
    }

    void Update() {
        // レベルの表示を更新
        lvText.text = $"Lv.{level}";

        // Sliderを更新
        sliderManager.SetMaxValue(maxXp);
        sliderManager.SetValue(currentXp);
    }
}
