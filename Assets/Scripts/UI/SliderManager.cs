using UnityEngine;
using UnityEngine.UI;

public class SliderManager : MonoBehaviour {
    private Slider slider;

    void Start() {
        slider = GetComponent<Slider>();
    }

    // Sliderの値を更新
    public void SetValue(int value) {
        // 値をSliderの範囲内に丸める
        int clamped = Mathf.Clamp(value, (int)slider.minValue, (int)slider.maxValue);

        slider.value = clamped;
    }

    public void SetMaxValue(int maxValue) {
        slider.maxValue = maxValue;
    }
}
