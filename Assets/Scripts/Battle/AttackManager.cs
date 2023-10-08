using Fungus;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    [SerializeField] private GameObject bar;
    [SerializeField] private GameObject pointer;

    // *** Enemyから操作

    [Tooltip("往復にかかる秒数(s)")]
    public float duration;

    [Tooltip("注意！'往復'のアニメーションを設定")]
    public AnimationCurve animationCurve;

    [Tooltip("バーのHeightをはみ出さないように目視でゆとりを設定")]
    public float offsetY;

    // ***

    private Flowchart flowchart;

    private float startTime;
    private Vector2 startPosition;
    private Vector2 endPosition;

    private bool isAttacking;

    // Start is called before the first frame update
    void Start()
    {
        flowchart = GetComponentInParent<BattleLoader>().flowchart;

        print(flowchart);

        float barHeight = bar.GetComponent<RectTransform>().sizeDelta.y;

        Vector2 pointerPos = pointer.transform.localPosition;

        startPosition = new Vector2(pointerPos.x, pointerPos.y - (barHeight / 2) + offsetY);
        endPosition = new Vector2(pointerPos.x, pointerPos.y + (barHeight / 2) - offsetY);

        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isAttacking)
        {
            float diff = Time.timeSinceLevelLoad - startTime;
            float rate = diff / duration;

            pointer.transform.localPosition = Vector2.Lerp(startPosition, endPosition, animationCurve.Evaluate(rate));

            if (Input.GetMouseButton(0))
            {
                flowchart.SendFungusMessage("attacked");
                isAttacking = false;
            }
        }
    }

    public void DoAttack()
    {
        isAttacking = true;

        startTime = Time.timeSinceLevelLoad;
        pointer.transform.localPosition = startPosition;

        gameObject.SetActive(true);
    }
}
