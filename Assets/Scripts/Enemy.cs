using UnityEngine;

// バトル用のクラス
public class Enemy : MonoBehaviour {

    public string _name;
    public int lv;
    public int hp;

    public float duration;
    public AnimationCurve animationCurve;
}