using UnityEngine;

public class PopupController : MonoBehaviour
{
    [SerializeField] private MessageTrigger messageTrigger;
    
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // 会話可能圏内にいる かつ メッセージを持っている かつ 会話中でない かつ !fireOnCollision の時、ポップアップを表示
        if (messageTrigger.CanInteract() && messageTrigger.message != "" && !messageTrigger.isTalking && !messageTrigger.fireOnCollision)
        {
            spriteRenderer.enabled = true;
        }
        else
        {
            spriteRenderer.enabled = false;
        }
    }
}