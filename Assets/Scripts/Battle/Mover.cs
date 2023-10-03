using UnityEngine;

public class Mover : MonoBehaviour {
    private RectTransform rectTransform;
    private Vector2 defaultPos;

    public float waitDuration;
    public float speed;

    private bool startMove;

    public Vector2 startPos;
    public Vector2 endPos;

    [Range(0f, 1f)]
    public float t;
    public AnimationCurve curve;

    // Start is called before the first frame update
    void Start() {
        rectTransform = GetComponent<RectTransform>();
        defaultPos = rectTransform.anchoredPosition;
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (startMove) {
            MoveGameObject(Vector2.Lerp(startPos,endPos,curve.Evaluate(t)));
            t += speed * 0.005f;

            if (t >= 1f) {
                startMove = false;
            }
        }
    }

    public void InvokeMove() {
        Invoke("StartMove", waitDuration);
    }

    private void StartMove() {
        startMove = true;
    }

    private void MoveGameObject(Vector2 pos) {
        //入力されたオブジェクトのRendererを全て取得し、さらにそのRendererに設定されている全Materialの色を変える
        rectTransform.anchoredPosition = defaultPos + pos;
    }
}
