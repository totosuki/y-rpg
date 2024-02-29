using UnityEngine;
using UnityEngine.Events;

public class InteractionTrigger : MonoBehaviour
{

    [Tooltip("インタラクト可能か")]
    [SerializeField] protected bool interactable;

    [Tooltip("当たり判定内に入っただけでイベントを発火するか")]
    [SerializeField] protected bool fireOnCollision;

    // インタラクト時に発動されるイベント
    [HideInInspector] public UnityEvent onInteract = new UnityEvent();

    // 当たり判定内にいるかどうかの状態保持
    private bool inCollision;

    protected bool interacted;

    void Update()
    {
        // 無限ループ防止
        if (interacted)
        {
            return;
        }
        if (interactable && inCollision)
        {
            // クリックでイベントを発動
            // fireOnCollision == trueの時、自動的に発動
            if (Input.GetMouseButtonDown(0) || fireOnCollision)
            {
                onInteract.Invoke();
                interacted = true;
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
                interacted = false;
            }
        }
    }
    
    // インタラクト可能かどうか
    public bool CanInteract()
    {
        return interactable && inCollision;
    }

    public void SetInteractable(bool flag)
    {
        interactable = flag;
    }
}
