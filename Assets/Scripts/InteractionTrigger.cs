using UnityEngine;
using UnityEngine.Events;

public class InteractionTrigger : MonoBehaviour
{

    [Tooltip("インタラクト可能か")]
    [SerializeField]private bool interactable;

    [Tooltip("当たり判定内に入っただけでイベントを発火するか")]
    [SerializeField]private bool fireOnCollision;

    // インタラクト時に発動されるイベント
    public UnityEvent onInteract = new UnityEvent();

    // 当たり判定内にいるかどうかの状態保持
    private bool inCollision;

    private bool interacted;

    void Update()
    {
        // 無限ループ防止
        if (interacted)
        {
            return;
        }
        // インタラクト可能 かつ 当たり判定内にいる
        if (interactable && inCollision)
        {
            // クリックでイベントを発動
            // fireOnCollision == trueの時、自動的に発動
            if (Input.GetMouseButtonDown(0) || fireOnCollision)
            {
                onInteract.Invoke();

                if (fireOnCollision)
                {
                    interacted = true;
                }
            }
        }
    }

    // 当たり判定
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            inCollision = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (inCollision)
            {
                inCollision = false;

                if (fireOnCollision)
                {
                    interacted = false;
                }
            }
        }
    }
    
    // 当たり判定内にプレイヤーがいるかどうか
    public bool IsPlayerInCollision()
    {
        return inCollision;
    }
}
